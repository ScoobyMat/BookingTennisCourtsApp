using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookingTennisCourts.Data.Entities;
using BookingTennisCourts.Data.Entities.Identity;
using BookingTennisCourts.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookingTennisCourts.Pages.Reservations
{
    public class CreateModel : PageModel
    {
        private readonly IReservationsRepository _reservationRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateModel(IReservationsRepository reservationRepository, UserManager<ApplicationUser> userManager)
        {
            _reservationRepository = reservationRepository;
            _userManager = userManager;
        }

        [BindProperty]
        public Reservation Reservation { get; set; }

        public List<TimeSpan> AvailableTimes { get; set; } = new List<TimeSpan>();
        public List<TimeSpan> AvailableTimesEnd { get; set; } = new List<TimeSpan>();

        public async Task<IActionResult> OnGetAsync(int courtId, string date)
        {
            Reservation = new Reservation
            {
                CourtId = courtId,
                Data = DateTime.Parse(date),
            };

            // Pobierz dostępne godziny dla danego kortu i daty
            AvailableTimes = _reservationRepository.GetAvailableTimes(Reservation.CourtId, Reservation.Data);

            AvailableTimesEnd = AvailableTimes.Select(time => time.Add(TimeSpan.FromHours(1))).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Użytkownik nie został znaleziony");
            }

            Reservation.UserId = user.Id;

            await _reservationRepository.Insert(Reservation);

            return RedirectToPage("/Index");
        }




    }
}
