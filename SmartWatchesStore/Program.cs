using Microsoft.EntityFrameworkCore;
using NToastNotify;
using SmartWatchesStore.Data;
using System;

namespace SmartWatchesStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Here add new services
            builder.Services.AddMvc();
            builder.Services.AddControllersWithViews();
      
            // Connection 
            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("SqliteDbConnection")));

            // Notify
            builder.Services.AddMvc().AddNToastNotifyToastr(new ToastrOptions()
            {
                ProgressBar = true,
                PositionClass = ToastPositions.TopRight,
                PreventDuplicates = true,
                CloseButton = true,
            });

            var app = builder.Build();

            //Here add middleware pipline
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
                );

            app.Run();
        }
    }
}
