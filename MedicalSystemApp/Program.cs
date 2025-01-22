using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MedicalSystemApp.Data;
using MedicalSystemApp.Repositories; // For the repos
using Npgsql; // If you need any Npgsql specifics (optional)

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
                options.UseNpgsql(connectionString)
                       // Enable lazy loading
                       .UseLazyLoadingProxies();
            });

            // 3) Register the RepositoryFactory + all your repositories
            builder.Services.AddScoped<RepositoryFactory>();

            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
            builder.Services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
            builder.Services.AddScoped<IExaminationRepository, ExaminationRepository>();
            builder.Services.AddScoped<IExaminationImageRepository, ExaminationImageRepository>();

            // 4) Add MVC services
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // 5) Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

            // 6) Set up the default MVC route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );

            // 7) Run the application
            app.Run();
        }
    }
}
