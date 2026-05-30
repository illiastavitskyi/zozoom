using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZoZoom.Data;
using Microsoft.AspNetCore.Http.Features;

namespace ZoZoom
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Set max upload size to 500 MB
            const long MaxUploadBytes = 500L * 1024L * 1024L;

            // Kestrel max request body size
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = MaxUploadBytes;
            });

            // Increase form multipart body limit
            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = MaxUploadBytes;
            });

            // Add Database Context
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Add Identity Services
            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Add support for MVC Controllers and Views
            builder.Services.AddControllersWithViews();

            // Add Razor Pages (Required for Identity UI)
            builder.Services.AddRazorPages();

            // Add SignalR
            builder.Services.AddSignalR();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            // Map SignalR hubs
            app.MapHub<Hubs.ChatHub>("/chatHub");

            app.Run();
        }
    }
}