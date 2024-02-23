using BookingTennisCourts.Contracts;
using BookingTennisCourts.Repositories;

namespace BookingTennisCourts.Installers
{
    public class RepositoriesInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IReservationsRepository, ReservationsRepository>();
            services.AddScoped<ICourtsRepository, CourtsRepository>();
        }
    }
}
