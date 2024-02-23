using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookingTennisCourts.Contracts;


namespace BookingTennisCourts.Pages.Courts
{
    public class EditModel : PageModel
    {
        private readonly ICourtsRepository _courtsRepository;

        public EditModel(ICourtsRepository courtsRepository)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _courtsRepository.Update(Court);
            await _courtsRepository.SaveChanges();

            return RedirectToPage("./Index");
        }
    }
}
