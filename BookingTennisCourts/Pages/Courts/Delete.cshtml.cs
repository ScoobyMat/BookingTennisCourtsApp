using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BookingTennisCourts.Data;

namespace BookingTennisCourts.Pages.Courts
{
    public class DeleteModel : PageModel
    {
        private readonly BookingTennisCourts.Data.BookingTennisCourtsAppDbContext _context;

        public DeleteModel(BookingTennisCourts.Data.BookingTennisCourtsAppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Court Court { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Courts == null)
            {
                return NotFound();
            }

            var court = await _context.Courts.FirstOrDefaultAsync(m => m.Id == id);

            if (court == null)
            {
                return NotFound();
            }
            else 
            {
                Court = court;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Courts == null)
            {
                return NotFound();
            }
            var court = await _context.Courts.FindAsync(id);

            if (court != null)
            {
                Court = court;
                _context.Courts.Remove(Court);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
