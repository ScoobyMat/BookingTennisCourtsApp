using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookingTennisCourts.Contracts; 

namespace BookingTennisCourts.Pages.Courts
{
    public class DetailsModel : PageModel
    {
        private readonly ICourtsRepository _courtsRepository; 

        public DetailsModel(ICourtsRepository courtsRepository)
        {
            _courtsRepository = courtsRepository;
        }

        public Court Court { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Court = await _courtsRepository.Get(id.Value); 

            if (Court == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
