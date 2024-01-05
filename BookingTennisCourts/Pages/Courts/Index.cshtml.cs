using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BookingTennisCourts.Data;
using Microsoft.AspNetCore.Authorization;

namespace BookingTennisCourts.Pages.Courts
{
    [Authorize(Roles ="Admin")]

    public class IndexModel : PageModel
    {
        private readonly BookingTennisCourts.Data.BookingTennisCourtsAppDbContext _context;

        public IndexModel(BookingTennisCourts.Data.BookingTennisCourtsAppDbContext context)
        {
            _context = context;
        }

        public IList<Court> Court { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Courts != null)
            {
                Court = await _context.Courts.ToListAsync();
            }
        }
    }
}
