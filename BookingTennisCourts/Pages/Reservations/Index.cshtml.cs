using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BookingTennisCourts.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BookingTennisCourts.Data.Identity;

namespace BookingTennisCourts.Pages.Reservations
{
    [Authorize] 
    public class IndexModel : PageModel
    {
        private readonly BookingTennisCourts.Data.BookingTennisCourtsAppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(BookingTennisCourts.Data.BookingTennisCourtsAppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Reservation> Reservation { get; set; } = new List<Reservation>();

        public async Task OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (_context.Reservations != null)
            {
                if (User.IsInRole("Admin"))
                {
                    Reservation = await _context.Reservations
                    .Include(r => r.Court).ToListAsync();
                }
                else if (currentUser != null)
                {
                    Reservation = await _context.Reservations
                        .Where(r => r.UserId == currentUser.Id)
                        .Include(r => r.Court).ToListAsync();
                }
            }
        }

    }
}
