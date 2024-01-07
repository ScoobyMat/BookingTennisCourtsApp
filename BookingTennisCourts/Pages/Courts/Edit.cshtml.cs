using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BookingTennisCourts.Data;
using BookingTennisCourts.Repositories.Contracts;

namespace BookingTennisCourts.Pages.Courts
{
    public class EditModel : PageModel
    {
        private readonly IGenericRepository<Court> _repository;

        public EditModel(IGenericRepository<Court> repository)
        {
            _repository = repository;
        }

        [BindProperty]
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _repository.Update(Court);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await MakeExistsAsync(Court.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private async Task<bool> MakeExistsAsync(int id)
        {
            return await _repository.Exists(id);
        }

    }
}
