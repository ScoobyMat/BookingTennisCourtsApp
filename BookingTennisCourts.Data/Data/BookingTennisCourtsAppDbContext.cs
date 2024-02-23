using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookingTennisCourts.Data.Data
{
    public class BookingTennisCourtsAppDbContext : IdentityDbContext<User>
    {
        public BookingTennisCourtsAppDbContext(DbContextOptions<BookingTennisCourtsAppDbContext> options) : base(options)
        {
        }

        public DbSet<Court> Courts { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
    }
}
