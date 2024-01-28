using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookingTennisCourts.Repositories.Contracts;
using BookingTennisCourts.Data.Entities;

namespace BookingTennisCourts.Pages.Reservations
{
    public class DetailsModel : PageModel
    {
        private readonly IReservationsRepository _repository;

        public DetailsModel(IReservationsRepository repository)
        {
            _repository = repository;
        }

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
    }
}
