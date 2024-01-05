using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookingTennisCourts.Data;

namespace BookingTennisCourts.Pages.Courts
{
    public class CreateModel : PageModel
    {
        private readonly BookingTennisCourts.Data.BookingTennisCourtsAppDbContext _context;

        public CreateModel(BookingTennisCourts.Data.BookingTennisCourtsAppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Court Court { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Courts == null || Court == null)
            {
                return Page();
            }

            _context.Courts.Add(Court);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
