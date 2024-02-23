using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookingTennisCourts.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BookingTennisCourts.Pages.Reservations
{
    public class IndexModel : PageModel
    {
        private readonly IReservationsRepository _reservationsRepository;
        private readonly ICourtsRepository _courtsRepository;
        private readonly UserManager<User> _userManager;

        public IndexModel(IReservationsRepository repository, ICourtsRepository courtsRepository, UserManager<User> userManager)
        {
            _reservationsRepository = repository;
            _courtsRepository = courtsRepository;
            _userManager = userManager;
        }

        public IList<Reservation> Reservation { get; set; }

        public async Task OnGetAsync(string lastName, DateTime? reservationDate)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (User.IsInRole("Admin"))
            {
                if (!string.IsNullOrEmpty(lastName))
                {
                    Reservation = await _reservationsRepository.GetReservationsByUserLastName(lastName);
                }
                else if (reservationDate.HasValue)
                {
                    Reservation = await _reservationsRepository.GetReservationsByDate(reservationDate.Value);
                }
                else
                {
                    Reservation = await _reservationsRepository.GetAll();
                }
            }
            else if (currentUser != null)
            {
                Reservation = await _reservationsRepository.GetReservationsByUserId(currentUser.Id);
            }
        }


        public async Task<string> GetCourtName(int courtId)
        {
            var court = await _courtsRepository.GetCourtName(courtId);
            return court;
        }

        public async Task<string> GetUserName(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return $"{user.FirstName} {user.LastName}";
        }

    }
}
