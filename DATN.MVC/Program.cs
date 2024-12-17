using DATN.MVC.Hubs;
using DATN.MVC.Middleware;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DATN.MVC
{
    public class Program
    {
        [Area("Admin")]
        [Route("Admin/[controller]/[action]")]
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("https://localhost:7260")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod()
                                      .AllowCredentials());
            });
            builder.Services.AddHttpClient();
            builder.Services.AddControllersWithViews()
                                .AddNewtonsoftJson();

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSignalR(options =>
            {
                options.MaximumReceiveMessageSize = 104857600; // Tăng giới hạn lên 10MB
            });

            builder.Services.AddControllers()
                .AddNewtonsoftJson();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 104857600; // Hạn chế dung lượng file upload (100MB)
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();

            }
         


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseRouting();
            app.UseSession();
            app.UseMiddleware<TokenMiddleware>();
            app.UseAuthorization();
            app.MapControllerRoute(
                 name: "areas",
                 pattern: "{area:exists}/{controller=HomeAdmin}/{action=Index}/{id?}");


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chatHub");
            });

            app.Run();
        }
    }
}
