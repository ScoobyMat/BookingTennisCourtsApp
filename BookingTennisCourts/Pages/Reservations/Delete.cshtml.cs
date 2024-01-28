using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookingTennisCourts.Repositories.Contracts;
using BookingTennisCourts.Data.Entities;

namespace BookingTennisCourts.Pages.Reservations
{
    public class DeleteModel : PageModel
    {
        private readonly IReservationsRepository _repository;

        public DeleteModel(IReservationsRepository repository)
        {
            _repository = repository;
        }

        [BindProperty]
        public Reservation Reservation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Reservation = await _repository.Get(id.Value);

            if (Reservation == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _repository.Delete(id.Value);

            return RedirectToPage("./Index");
        }
    }
}
