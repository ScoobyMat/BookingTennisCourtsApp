using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingTennisCourts.Data.Entities;
using BookingTennisCourts.Data.Entities.Identity;
using BookingTennisCourts.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookingTennisCourts.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IReservationsRepository _reservationRepository;
        private readonly ICourtsRepository _courtsRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(IReservationsRepository reservationRepository, ICourtsRepository courtsRepository, UserManager<ApplicationUser> userManager)
        {
            _reservationRepository = reservationRepository;
            _courtsRepository = courtsRepository;
            _userManager = userManager;
        }

        public IEnumerable<SelectListItem> Courts { get; set; } = new List<SelectListItem>();  // Zmiana na IEnumerable<SelectListItem>

        [BindProperty]
        public Reservation Reservation { get; set; }

        public List<TimeSpan> AvailableTimes { get; set; } = new List<TimeSpan>();

        public async Task<IActionResult> OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return NotFound();
            }

            await LoadInitialData();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadInitialData();
                return Page();
            }

            var userId = _userManager.GetUserId(User);
            Reservation.UserId = userId;

            // Przejdź do widoku z formularzem rezerwacji
            return RedirectToPage("./Reservations/Create", new { courtId = Reservation.CourtId, date = Reservation.Data.ToString("yyyy-MM-dd") });
        }

        private async Task LoadInitialData()
        {
            var courts = await _courtsRepository.GetAll();

            Courts = courts.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name });
        }
    }
}
