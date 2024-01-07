using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookingTennisCourts.Data;
using BookingTennisCourts.Repositories.Contracts;

namespace BookingTennisCourts.Pages.Courts
{
    public class DeleteModel : PageModel
    {
        private readonly IGenericRepository<Court> _repository;

        public DeleteModel(IGenericRepository<Court> repository)
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

            HasReservations = await _repository.HasReservations(id.Value);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            HasReservations = await _repository.HasReservations(id.Value);

            if (HasReservations)
            {
                Court = await _repository.Get(id.Value);
                return Page();
            }

            await _repository.Delete(id.Value);
            return RedirectToPage("./Index");
        }
    }
}
