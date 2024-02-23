using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookingTennisCourts.Contracts;
using Microsoft.AspNetCore.Identity;

namespace BookingTennisCourts.Pages.Reservations
{
    public class DetailsModel : PageModel
    {
        private readonly IReservationsRepository _reservationsRepository;
        private readonly ICourtsRepository _courtsRepository;
        private readonly UserManager<User> _userManager;

        public DetailsModel(IReservationsRepository reservationsRepository, ICourtsRepository courtsRepository, UserManager<User> userManager)
        {
            _reservationsRepository = reservationsRepository;
            _courtsRepository = courtsRepository;
            _userManager = userManager;
        }

        public Reservation Reservation { get; set; }
        public string CourtName { get; set; }
        public string UserFullName { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Reservation = await _reservationsRepository.Get(id.Value);

            if (Reservation == null)
            {
                return NotFound();
            }

            CourtName = await _courtsRepository.GetCourtName(Reservation.CourtId);
            var user = await _userManager.FindByIdAsync(Reservation.UserId);
            UserFullName = $"{user.FirstName} {user.LastName}";
            return Page();
        }
    }
}
