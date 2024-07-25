using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Middlewares;

namespace WebApplication1
{
    public class Startup
    {
        
        // Метод вызывается средой ASP.NET.
        // Используйте его для подключения сервисов приложения
        // Документация:  https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Добавляем авторизацию
            services.AddAuthorization();
        }

        public Startup(IWebHostEnvironment env)
        {
            Startup.env = env;
        }
        protected static IWebHostEnvironment env;

        /// <summary>
        ///  Обработчик для страницы About
        /// </summary>
        private static void About(IApplicationBuilder app )
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync($"{env.ApplicationName} - ASP.Net Core tutorial project");
            });
        }

        /// <summary>
        ///  Обработчик для главной страницы
        /// </summary>
        private static void Config(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync($"App name: {env.ApplicationName}. App running configuration: {env.EnvironmentName}");
            });
        }

        // Метод вызывается средой ASP.NET.
        // Используйте его для настройки конвейера запросов
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseStatusCodePages("text/plain", "Error, status code: {0}");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/config", async context =>
                {
                    await context.Response.WriteAsync($"App name: {env.ApplicationName}. App running configuration: {env.EnvironmentName}");
                });

                endpoints.MapGet("/about", async context =>
                {
                    await context.Response.WriteAsync("This is the about page.");
                });

                app.Run(async context =>
                {
                    context.Response.StatusCode = 404;
                    await context.Response.WriteAsync("Page not found");
                });

                app.Run(async context =>
                {
                    await context.Response.WriteAsync($"Welcome to the {env.ApplicationName}!");
                });
                app.UseAuthorization();
            });
        }
    }
}
