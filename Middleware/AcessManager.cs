using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_SPA.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AccessManager
    {
        private readonly RequestDelegate _next;

        public AccessManager(RequestDelegate next)  // DI еще рано
        {
            _next = next;
        }

        public Task Invoke(
            HttpContext httpContext 
            /*, Data.StockContext stockContext*/)  // DI через метод
        {
            var stockContext =        // Прямой запрос к контейнеру DI
                (Data.StockContext)
                httpContext
                .RequestServices
                .GetService(typeof(Data.StockContext));

            stockContext.Logs.Add(
                new Models.Log
                {
                    Url = httpContext.Request.Path,
                    Moment = DateTime.Now
                }
            );
            stockContext.SaveChanges();

            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AccessManagerExtensions
    {
        public static IApplicationBuilder UseAccessManager(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AccessManager>();
        }
    }
}
