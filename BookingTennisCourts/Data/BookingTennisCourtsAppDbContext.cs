using BookingTennisCourts.Data.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookingTennisCourts.Data
{
    public class BookingTennisCourtsAppDbContext : IdentityDbContext<ApplicationUser>
    {
        public BookingTennisCourtsAppDbContext(DbContextOptions<BookingTennisCourtsAppDbContext> options) : base(options)
        {
        }

        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Court> Courts { get; set; }
    }
}
