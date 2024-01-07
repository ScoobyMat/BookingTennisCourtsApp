using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookingTennisCourts.Data;
using BookingTennisCourts.Repositories.Contracts;

namespace BookingTennisCourts.Pages.Courts
{
    public class CreateModel : PageModel
    {
        private readonly IGenericRepository<Court> _repository;

        public CreateModel(IGenericRepository<Court> repository)
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