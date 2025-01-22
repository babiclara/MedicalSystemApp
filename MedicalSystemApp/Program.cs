using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MedicalSystemApp.Data; // Adjust if your DbContext is in a different namespace

namespace MedicalSystemApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1) Read the connection string from appsettings.json
            var connectionString = builder.Configuration.GetConnectionString("MedicalDatabase");

            // 2) Register your DbContext, using Npgsql for Postgres
            builder.Services.AddDbContext<MedicalDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
                // If you want lazy loading, add:
                // options.UseLazyLoadingProxies();
            });

            // 3) Add services to the container (MVC)
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // 4) Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();  // The default HSTS value is 30 days.
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

            // 5) Set up default MVC route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );

            // 6) Run the application
            app.Run();
        }
    }
}
