using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_SPA.Models
{
    /**
     * ORM для записей журнала (логов) посещения сайта
     * 
     */
    public class Log
    {
        public int Id { get; set; }
        public String Url { get; set; }
        public DateTime Moment { get; set; }
    }
}
