using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MVC_SPA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private Data.StockContext stockContext;

        public OrderController(Data.StockContext stockContext)
        {
            this.stockContext = stockContext;
        }

        private int GetUserId()
        {
            int userId;
            // Есть ли в сессии IdUser?
            if (HttpContext.Session.Keys.Contains("IdUser"))
            {
                userId = HttpContext.Session.GetInt32("IdUser").Value;
            }
            else
            {
                userId = (int)DateTime.Now.Ticks;
                HttpContext.Session.SetInt32("IdUser", userId);
            }
            return userId;
        }

        // GET: api/<OrderController>
        [HttpGet]
        public IEnumerable<Models.StockOrder> Get()
        {
            int userId = GetUserId();
            return stockContext.StockOrders.Where(
                item => item.IdUser == userId).ToList();
        }

        // GET api/<OrderController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value " + id;
        }

        // POST api/<OrderController>
        [HttpPost]
        public String Post([FromBody] Models.StockOrder value)
        {
            try
            {
                stockContext.StockOrders.Add(
                    new Models.StockOrder
                    {
                        IdItem = value.IdItem,
                        Cnt = value.Cnt,
                        Moment = DateTime.Now,
                        IdUser = GetUserId()
                    }
                );
                stockContext.SaveChanges();
                return "OK";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        // PUT api/<OrderController>/5
        [HttpPut("{id}")]
        public IEnumerable<string> Put(int id, [FromBody] string value)
        {
            return new string[] { "put", value, id.ToString() };
        }

        // DELETE api/<OrderController>/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            return id + " deleted";
        }
    }
}

/*
 *  Web-API / ApiControllers 
 *  API контроллеры создаются с адресом /api/<имя>, где
 *  <имя> - часть имени контроллера (до слова Controller)
 *  В отличие от MVC контроллеров, у которых разные методы
 *  создают разные страницы, у API контроллеров методы класса
 *  разделяются по методам запроса
 */