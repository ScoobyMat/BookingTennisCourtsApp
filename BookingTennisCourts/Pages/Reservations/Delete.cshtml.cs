using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookingTennisCourts.Contracts;

namespace BookingTennisCourts.Pages.Reservations
{
    public class DeleteModel : PageModel
    {
        private readonly IReservationsRepository _reservationsRepository;
        private readonly ICourtsRepository _courtsRepository;

        public DeleteModel(IReservationsRepository reservationsRepository, ICourtsRepository courtsRepository)
        {
            _reservationsRepository = reservationsRepository;
            _courtsRepository = courtsRepository;
        }

        [BindProperty]
        public Reservation Reservation { get; set; }

        public string CourtName { get; set; }

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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _reservationsRepository.Delete(id.Value); 
            await _reservationsRepository.SaveChanges();

            return RedirectToPage("./Index");
        }
    }
}
