using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_SPA.Models
{
    public class StockOrder
    {
        public int Id { get; set; }
        public int IdItem { get; set; }
        public int Cnt { get; set; }
        public DateTime Moment { get; set; }
        public int IdUser { get; set; }
    }
}
