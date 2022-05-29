using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVC_SPA.Models;

namespace MVC_SPA.Controllers
{
    public class HomeController : Controller
    {
        private Random rnd = new Random();

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult Logs()
        {
            return Json(
                (HttpContext.RequestServices
                .GetService(typeof(MVC_SPA.Data.StockContext))
                as MVC_SPA.Data.StockContext)?.Logs.ToList()
            );
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public ViewResult Lesson2()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RndValue(int? id)
        {
            if (id.HasValue)
            {
                var ret = new List<object>();
                ret.Add(new { val = rnd.Next().ToString() } );
                ret.Add(new { val = rnd.Next().ToString() } );
                ret.Add(new { val = rnd.Next().ToString() } );
                return Json(ret);
            }
            else
            {
                
                var ret = new { val = rnd.Next().ToString() };
                return Json(ret);
            }
        }

        public class Data
        {
            public int Base { get; set; }
            public int Quantity { get; set; }
        };

        [HttpPost]
        public IActionResult RndValue([FromBody] Data data, [FromQuery] String Base)
        {
            // [FromForm] String Base  - "Content-Type": "application/x-www-form-urlencoded"
            // [FromBody] Data data - "Content-Type": "application/json" (Data - model)
            // [FromQuery] String Base - из запроса (?base=2000)
            // атрибуты [FromQuery] и другие можно смешивать
            //// String b = (String) ((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JProperty)(data as Newtonsoft.Json.Linq.JObject).First).Value).Value;
            // случайное число в пределах data.Base
            return Content("Post works! " + data.Base + " " + data.Quantity );
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
