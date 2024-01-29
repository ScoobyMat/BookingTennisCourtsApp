using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookingTennisCourts.Repositories.Contracts;
using BookingTennisCourts.Repositories.Repositories;
using BookingTennisCourts.Data.Entities;
using Microsoft.AspNetCore.Identity;
using BookingTennisCourts.Data.Entities.Identity;

namespace BookingTennisCourts.Pages.Reservations
{
    public class EditModel : PageModel
    {
        private readonly IReservationsRepository _reservationRepository;
        private readonly ICourtsRepository _courtsRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditModel(IReservationsRepository reservationRepository, ICourtsRepository courtsRepository, UserManager<ApplicationUser> userManager)
        {
            _reservationRepository = reservationRepository;
            _courtsRepository = courtsRepository;
            _userManager = userManager;
        }

        public SelectList Courts { get; set; }
        public string UserFullName { get; set; }

        [BindProperty]
        public Reservation Reservation { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Reservation = await _reservationRepository.Get(id.Value);

            if (Reservation == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(Reservation.UserId);
            UserFullName = $"{user.FirstName} {user.LastName}";

            await LoadInitialData();
            return Page();
        }


        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadInitialData();
                return Page();
            }

            // Sprawdź dostępność nowego terminu rezerwacji
            var isReservationAvailable = _reservationRepository.CheckReservation(Reservation);
            if (!isReservationAvailable)
            {
                ModelState.AddModelError(string.Empty, "Wybrany termin jest już zarezerwowany.");
                await LoadInitialData();
                return Page();
            }

            await _reservationRepository.Update(Reservation);

            return RedirectToPage("./Index");
        }


        private async Task LoadInitialData()
        {
            Courts = new SelectList(await _courtsRepository.GetAll(), "Id", "Name");
        }
    }
}
