using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace MVC_SPA.Controllers
{
    public class StockController : Controller
    {
        private Data.StockContext stockContext;
        private IHostingEnvironment hostingEnvironment;

        public StockController(
            Data.StockContext stockContext,
            IHostingEnvironment hostingEnvironment)            
        {
            this.stockContext = stockContext;
            this.hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            if (!stockContext.StockItems.Any())
            {
                stockContext.FillItems();
            }
            return View();
        }

        /*
         * Задание: обработка неполного запроса /Stock/Items?count (нет =...)
         * 
         */

        public JsonResult Items(int? id)
        {
            if (id.HasValue)
            {
                return Json(stockContext.StockItems.Find(id.Value));
            }

            if (Request.Query.ContainsKey("count"))
            {
                return Json(
                    new { count = stockContext.StockItems.Count() }
                    );
            }
            else
            {
                var list = stockContext.StockItems.ToList();
                foreach(Models.StockItem item in list)
                {
                    if( ! System.IO.File.Exists(
                        Path.Combine(
                            hostingEnvironment.WebRootPath, 
                            "img", 
                            item.LogoFilename)))                        
                    {
                        item.LogoFilename = "nologo.png";
                    }
                }
                return Json(list);
            }
        }

        // Корзина-заказы
        public ViewResult Order()
        {
            return View();
        }



        // Админка
        public ViewResult Admin()
        {
            return View();
        }

        [HttpDelete]
        public JsonResult Admin(int id)
        {
            var updated = stockContext.StockItems.Find(id);
            if (updated != null)
            {
                updated.IsVisible = false;
                stockContext.SaveChanges();
            }
            return Json(new { Id = id, status = "deleted" });
        }

        [HttpPost]
        public JsonResult Update(int id, [FromBody] object Item)
        {
            var isVisibleField = (JValue) ((JObject)Item)["isVisible"];
            var amountField    = (JValue) ((JObject)Item)["amount"];

            var updated = stockContext.StockItems.Find(id);
            if (updated != null)
            {
                if (isVisibleField != null
                    && isVisibleField.Value.Equals(true))
                {
                    updated.IsVisible = true;                    
                }
                if(amountField != null)
                {
                    try {
                        updated.Amount = Convert.ToInt32(amountField.Value);
                    }
                    catch {
                        return Json(new { id, status = "error in amount" });
                    }
                }
                stockContext.SaveChanges();
                return Json(new { id, status = "posted" });
            }
            return Json(new { id, status = "error in id" });
        }

        [BindProperty]
        public IFormFile LogoFile { get; set; }

        [HttpPost]
        public JsonResult UpdateLogo(int id)
        {
            // проверяем передачу файла
            if(LogoFile == null)
            {   // если файла нет - отправляем "ошибку"
                return Json(new { 
                    Status = "Error",
                    Description = "No logo file"
                });
            }
            // проверяем правильность id
            var updated = stockContext.StockItems.Find(id);
            if (updated == null)
            {
                return Json(new
                {
                    Status = "Error",
                    Description = "No id record"
                });
            }
            // Проверка на то, что файл - изображение (по расширению)
            String newName = LogoFile.FileName;  // ошибки с спецзнаками
                // = System.Net.WebUtility.UrlEncode(LogoFile.FileName); - не помогает
            // Определяем расширение
            String ext = newName.Substring(newName.LastIndexOf("."));
            // Разрешенные расширения
            String[] allowedExt = { ".png", ".jpg", ",jpeg", ".jfif", ".bmp" };
            // Проверяем соответствие
            if ( ! allowedExt.Contains(ext))
            {
                return Json(new
                {
                    Status = "Error",
                    Description = "Invalid MIME type"
                });
            }

            // если файла с таким именем нет, просто сохраняем
            String dirPath = hostingEnvironment.WebRootPath;
            bool isUnsafe;  // признак наличия спец(опасных) символов
            if (newName.Equals(System.Net.WebUtility.UrlEncode(LogoFile.FileName)))
            {
                // В имени нет специальных символов, оставляем как есть
                isUnsafe = false;
            }
            else
            {
                // Есть спецсимволы - будем генерировать случайное имя 
                isUnsafe = true;
            }

            String newFullName = Path.Combine(dirPath, "img", newName);
            if (System.IO.File.Exists(newFullName) || isUnsafe)
            {   // Файл с таким именем существует. Генерируем новое
                
                // Генерируем случайное имя
                do
                {
                    newName = DateTime.Now.Ticks.ToString() + ext;
                    newFullName = Path.Combine(dirPath, "img", newName);
                } while (System.IO.File.Exists(newFullName));
                // после цикла - newName новое имя несуществ. файла
                // newFullName - с учетом пути
            }

            using( var newStream = new FileStream(newFullName, FileMode.Create))
            {
                LogoFile.CopyTo(newStream);
            }

            // Удаляем старый файл 
            // Обнаружен баг - если в БД то же имя, удаляет свежий файл
            if (!updated.LogoFilename.Equals(newName))
            {
                System.IO.File.Delete(
                    Path.Combine(dirPath, "img", updated.LogoFilename));
            }
            // обновляем в БД имя файла
            updated.LogoFilename = newName;
            stockContext.SaveChanges();

            return Json(new { 
                Id = id, 
                Status = "Updated Logo", 
                Name = LogoFile.Name
                
            });
        }
    }
}

/*  Проект "Биржа"
 *  Основу составляют акции/активы/валюты, которые можно покупать и продавать
 *  Информация:
 *      название
 *      изображение-логотип
 *      количество акций (для продажи)
 *      цена продажи
 *      цена покупки
 *      описание
 *  Функционал пользователя:
 *      просмотр (с пагинацией от англ. pagination - разбиение на страницы)
 *      покупка 
 *      продажа
 *  Функционал администратора:
 *      добавление/редактирование/удаление/блокировка  названий
 *      корректирование количества доступных единиц
 *      
 */