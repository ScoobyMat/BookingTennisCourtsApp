using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookingTennisCourts.Repositories.Contracts;
using BookingTennisCourts.Data.Entities;

namespace BookingTennisCourts.Pages.Courts
{
    public class CreateModel : PageModel
    {
        private readonly ICourtsRepository _repository;

        public CreateModel(ICourtsRepository repository)
        {
            this._repository = repository;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Court Court { get; set; } = new Court();

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _repository == null || Court == null)
            {
                return Page();
            }

            await _repository.Insert(Court);

            return RedirectToPage("./Index");
        }
    }
}