using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_SPA.Models
{
    public class StockItem
    {
        public int Id { get; set; }
        public String Title { get; set; }   // название
        public String LogoFilename { get; set; }   // изображение-логотип
        public int Amount { get; set; }   // количество акций(для продажи)
        public double SellRate { get; set; }   // цена продажи
        public double BuyRate { get; set; }   // цена покупки
        public String Description { get; set; }   // описание
        public bool IsVisible { get; set; }  // для "удаления"
    }
}
