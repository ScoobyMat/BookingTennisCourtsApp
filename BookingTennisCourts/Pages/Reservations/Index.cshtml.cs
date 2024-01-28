using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookingTennisCourts.Repositories.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BookingTennisCourts.Repositories.Repositories;
using BookingTennisCourts.Data.Entities;
using BookingTennisCourts.Data.Entities.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookingTennisCourts.Pages.Reservations
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IReservationsRepository _reservationsRepository;
        private readonly ICourtsRepository _courtsRepository;

        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(IReservationsRepository repository, ICourtsRepository courtsRepository, UserManager<ApplicationUser> userManager)
        {
            _reservationsRepository = repository;
            _courtsRepository = courtsRepository;
            _userManager = userManager;
        }

        public IList<Reservation> Reservation { get; set; } = new List<Reservation>();

        [BindProperty(SupportsGet = true)]
        public int? SelectedCourtId { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? Date { get; set; }

        public SelectList Courts { get; set; }

        public async Task OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (User.IsInRole("Admin"))
            {
                Reservation = await _reservationsRepository.GetAll();
            }
            else if (currentUser != null)
            {
                Reservation = await _reservationsRepository.GetReservationsByUserId(currentUser.Id);
            }

            Courts = new SelectList(await _courtsRepository.GetAll(), "Id", "Name");
        }

        public async Task<string> GetCourtName(int courtId)
        {
            var court = await _courtsRepository.GetCourtName(courtId);
            return court;
        }
    }
}
