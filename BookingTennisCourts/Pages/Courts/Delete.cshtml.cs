using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookingTennisCourts.Contracts;

namespace BookingTennisCourts.Pages.Courts
{
    public class DeleteModel : PageModel
    {
        private readonly ICourtsRepository _courtsRepository;

        public DeleteModel(ICourtsRepository courtsRepository)
        {
            _courtsRepository = courtsRepository;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _courtsRepository.Delete(id.Value);
            await _courtsRepository.SaveChanges(); 

            return RedirectToPage("./Index");
        }
    }
}
