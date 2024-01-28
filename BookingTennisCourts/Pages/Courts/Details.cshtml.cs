using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookingTennisCourts.Repositories.Contracts;
using BookingTennisCourts.Data.Entities;

namespace BookingTennisCourts.Pages.Courts
{
    public class DetailsModel : PageModel
    {
        private readonly ICourtsRepository _repository;

        public DetailsModel(ICourtsRepository repository)
        {
            _repository = repository;
        }

        public Court Court { get; set; } = default!;

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

            return Page();
        }
    }
}
