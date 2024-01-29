using BookingTennisCourts.Data.Data;
using BookingTennisCourts.Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace BookingTennisCourts.Installers
{
    public class IdentityInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDefaultIdentity<ApplicationUser>(options => { options.SignIn.RequireConfirmedAccount = false; })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<BookingTennisCourtsAppDbContext>();

            services.AddAuthentication();
            services.AddRazorPages(o => { o.Conventions.AuthorizeFolder("/"); });
        }
    }
}
