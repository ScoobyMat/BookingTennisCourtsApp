using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookingTennisCourts.Contracts; 

namespace BookingTennisCourts.Pages.Courts
{
    public class CreateModel : PageModel
    {
        private readonly ICourtsRepository _courtsRepository; 
        private readonly IReservationsRepository _reservationsRepository; 

        public CreateModel(ICourtsRepository courtsRepository, IReservationsRepository reservationsRepository) 
        {
            _courtsRepository = courtsRepository;
            _reservationsRepository = reservationsRepository;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Court Court { get; set; } = new Court();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _courtsRepository.Insert(Court);
            await _courtsRepository.SaveChanges(); 

            return RedirectToPage("./Index");
        }
    }
}
