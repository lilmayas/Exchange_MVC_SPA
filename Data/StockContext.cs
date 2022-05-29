using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MVC_SPA.Data
{
    public class StockContext : DbContext
    {
        public DbSet<Models.StockItem> StockItems { get; set; }
        public DbSet<Models.StockOrder> StockOrders { get; set; }
        public DbSet<Models.Log> Logs { get; set; }

        public StockContext( DbContextOptions<StockContext> options )
            : base(options)
        {

        }

        public void FillItems()
        {
            StockItems.Add( new Models.StockItem
            {
                Title = "Bitcoin",
                LogoFilename = "bitcoin.png",
                Amount = 100,
                SellRate = 50500,
                BuyRate = 49125,
                Description = "Bitcoin is the world’s first cryptocurrency which works on a completely decentralized network known as the blockchain. "
            });

            StockItems.Add(new Models.StockItem
            {
                Title = "Ethereum",
                LogoFilename = "ethereum.png",
                Amount = 200,
                SellRate = 3379,
                BuyRate = 3235,
                Description = "Ethereum is basically an open software platform based on the blockchain technology which allows developers to building several decentralized applications called DAPPS."
            });

            StockItems.Add(new Models.StockItem
            {
                Title = "Cardano",
                LogoFilename = "cardano.png",
                Amount = 300,
                SellRate = 2.96,
                BuyRate = 2.69,
                Description = "Cardano (ADA) is a networked computing platform which provides payment services for individual, institutional and governmental financial applications."
            });

            SaveChanges();
        }
    }
}
