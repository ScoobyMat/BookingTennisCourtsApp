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

            builder.Services.AddDbContext<BookingTennisCourtsAppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

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

            app.Run();
        }
    }
}