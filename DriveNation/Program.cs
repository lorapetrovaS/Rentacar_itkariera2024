using DriveNation.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace DriveNation
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<RentACarUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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

            CultureInfo ukCulture = new CultureInfo("en-GB");
            CultureInfo.DefaultThreadCurrentCulture = ukCulture;
            CultureInfo.DefaultThreadCurrentUICulture = ukCulture;

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var roles = new[] { "Admin" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            }

            //Creates AdminAdminov2
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<RentACarUser>>();

                string email = "lorakostova_b19@schoolmath.eu";
                string progileName = "lorakostova";
                string firstname = "Lora";
                string lastname = "Kostova";
                string password = "1W3erfd$@!q";
                string personal_id = "0525746542";
                string phone = "+359877648854";

                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new RentACarUser();
                    user.UserName = email;
                    user.Email = email;
                    user.ProfileName = progileName;
                    user.FirstName = firstname;
                    user.LastName = lastname;
                    user.Personal_Id = personal_id;
                    user.PhoneNumber = phone;
                    user.EmailConfirmed = false;

                    await userManager.CreateAsync(user, password);

                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            //Creates IvanIvanov
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<RentACarUser>>();

                string email = "vyaraatanasova_v19@schoolmath.eu";
                string progileName = "vyarkazvqrka";
                string firstname = "Vyara";
                string lastname = "Zvyarka";
                string password = "35FH$#we4!";
                string personal_id = "0636135584";
                string phone = "+359123456789";

                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new RentACarUser();
                    user.UserName = email;
                    user.Email = email;
                    user.ProfileName = progileName;
                    user.FirstName = firstname;
                    user.LastName = lastname;
                    user.Personal_Id = personal_id;
                    user.PhoneNumber = phone;
                    user.EmailConfirmed = false;

                    await userManager.CreateAsync(user, password);
                }
            }

            app.Run();
        }
    }
}