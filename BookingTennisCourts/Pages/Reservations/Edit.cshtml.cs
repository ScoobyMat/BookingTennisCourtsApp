using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookingTennisCourts.Repositories.Contracts;
using BookingTennisCourts.Repositories.Repositories;
using BookingTennisCourts.Data.Entities;

namespace BookingTennisCourts.Pages.Reservations
{
    public class EditModel : PageModel
    {
        private readonly IReservationsRepository _reservationRepository;
        private readonly ICourtsRepository _courtsRepository;

        public EditModel(IReservationsRepository reservationRepository, ICourtsRepository courtsRepository)
        {
            _reservationRepository = reservationRepository;
            _courtsRepository = courtsRepository;
        }

        public SelectList Courts { get; set; } = default!;

        [BindProperty]
        public Reservation Reservation { get; set; } = default!;

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

            await _reservationRepository.Update(Reservation);

            return RedirectToPage("./Index");
        }

        private async Task LoadInitialData()
        {
            Courts = new SelectList(await _courtsRepository.GetAll(), "Id", "Name");
        }
    }
}
