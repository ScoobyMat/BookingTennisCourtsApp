using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookingTennisCourts.Data;
using System.Security.Claims;

namespace BookingTennisCourts.Pages.Reservations
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
        ViewData["CourtId"] = new SelectList(_context.Courts, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Reservation Reservation { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Pobierz aktualnie zalogowanego użytkownika
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Reservation.UserId = userId;

            _context.Reservations.Add(Reservation);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }
}
