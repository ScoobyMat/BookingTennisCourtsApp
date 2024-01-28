using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BookingTennisCourts.Repositories.Repositories;
using BookingTennisCourts.Repositories.Contracts;
using BookingTennisCourts.Data.Entities.Identity;
using BookingTennisCourts.Data.Data;

namespace BookingTennisCourts
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<BookingTennisCourtsAppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IReservationsRepository, ReservationsRepository>();
            builder.Services.AddScoped<ICourtsRepository, CourtsRepository>();


            builder.Services.AddDefaultIdentity<ApplicationUser>(options => { options.SignIn.RequireConfirmedAccount = false; })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<BookingTennisCourtsAppDbContext>();

            builder.Services.AddAuthentication();

            builder.Services.AddRazorPages(o => { o.Conventions.AuthorizeFolder("/"); });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapRazorPages();

            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                try
                {
                    var roles = new[] { "Admin", "User" };

                    foreach (var role in roles)
                    {
                        if (!await roleManager.RoleExistsAsync(role))
                        {
                            await roleManager.CreateAsync(new IdentityRole(role));
                            Console.WriteLine($"Rola {role} zosta³a pomyœlnie utworzona.");
                        }
                        else
                        {
                            Console.WriteLine($"Rola {role} ju¿ istnieje.");
                        }
                    }

                    string email = "admin@admin.com";
                    string password = "Admin123$";
                    string FirstName = "Admin";
                    string LastName = "Admin";
                    DateTime DateOfBirth = new DateTime(2000, 1, 1);

                    if (await userManager.FindByEmailAsync(email) == null)
                    {
                        var user = new ApplicationUser
                        {
                            UserName = email,
                            Email = email,
                            FirstName = FirstName,
                            LastName = LastName,
                            DateOfBirth = DateOfBirth
                        };

                        var result = await userManager.CreateAsync(user, password);

                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(user, "Admin");
                            Console.WriteLine("U¿ytkownik Admin zosta³ pomyœlnie utworzony.");
                        }
                        else
                        {
                            Console.WriteLine("B³¹d przy tworzeniu u¿ytkownika:");
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine(error.Description);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("U¿ytkownik Admin ju¿ istnieje.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wyst¹pi³ b³¹d: {ex.Message}");
                }
            }

            app.Run();
        }
    }
}