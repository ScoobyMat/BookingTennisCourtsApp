using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookingTennisCourts.Repositories.Contracts;
using BookingTennisCourts.Data.Entities;
using Microsoft.AspNetCore.Identity;
using BookingTennisCourts.Data.Entities.Identity;

namespace BookingTennisCourts.Pages.Reservations
{
    public class DeleteModel : PageModel
    {
        private readonly IReservationsRepository _reservationsRepository;
        private readonly ICourtsRepository _courtsRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteModel(IReservationsRepository reservationsRepository, ICourtsRepository courtsRepository, UserManager<ApplicationUser> userManager)
        {
            _reservationsRepository = reservationsRepository;
            _courtsRepository = courtsRepository;
            _userManager = userManager;
        }

        [BindProperty]
        public Reservation Reservation { get; set; } = default!;

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

            // Pobierz nazwę kortu
            CourtName = await _courtsRepository.GetCourtName(Reservation.CourtId);

            // Pobierz pełne imię i nazwisko użytkownika
            var user = await _userManager.FindByIdAsync(Reservation.UserId);
            UserFullName = $"{user.FirstName} {user.LastName}";

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _reservationsRepository.Delete(id.Value);

            return RedirectToPage("./Index");
        }
    }
}
