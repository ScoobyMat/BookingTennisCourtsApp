using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookingTennisCourts.Repositories.Contracts;
using BookingTennisCourts.Data.Entities;

namespace BookingTennisCourts.Pages.Courts
{
    public class DeleteModel : PageModel
    {
        private readonly ICourtsRepository _repository;

        public DeleteModel(ICourtsRepository repository)
        {
            _repository = repository;
        }

        [BindProperty]
        public Court Court { get; set; } = default!;

        public bool HasReservations { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Court = await _repository.Get(id.Value);

            if (Court == null)
            {
                return NotFound();
            }

            HasReservations = await _repository.CourtHasReservations(id.Value);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Court = await _repository.Get(id.Value);

            if (Court == null)
            {
                return NotFound();
            }

            HasReservations = await _repository.CourtHasReservations(id.Value);

            if (HasReservations)
            {
                return Page();
            }

            await _repository.Delete(id.Value);
            return RedirectToPage("./Index");
        }
    }
}
