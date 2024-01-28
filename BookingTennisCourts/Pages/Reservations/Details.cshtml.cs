using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookingTennisCourts.Repositories.Contracts;
using BookingTennisCourts.Data.Entities;
using BookingTennisCourts.Repositories.Repositories;
using BookingTennisCourts.Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookingTennisCourts.Pages.Reservations
{
    public class DetailsModel : PageModel
    {
        private readonly IReservationsRepository _reservationsRepository;
        private readonly ICourtsRepository _courtsRepository;

        private readonly UserManager<ApplicationUser> _userManager;

        public DetailsModel(IReservationsRepository repository, ICourtsRepository courtsRepository, UserManager<ApplicationUser> userManager)
        {
            _reservationsRepository = repository;
            _courtsRepository = courtsRepository;
            _userManager = userManager;
        }

        public SelectList Courts { get; set; }

        public Reservation Reservation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Reservation = await _reservationsRepository.Get(id.Value);

            if (Reservation == null)
            {
                return NotFound();
            }

            Courts = new SelectList(await _courtsRepository.GetAll(), "Id", "Name");

            return Page();
        }

        public async Task<string> GetCourtName(int courtId)
        {
            var court = await _courtsRepository.GetCourtName(courtId);
            return court;
        }
    }
}
