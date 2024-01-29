using BookingTennisCourts.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace BookingTennisCourts.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<BookingTennisCourtsAppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        }
    }
}