using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookingTennisCourts.Contracts;
using Microsoft.AspNetCore.Identity;
using BookingTennisCourts.Repositories;

namespace BookingTennisCourts.Pages.Reservations
{
    public class CreateModel : PageModel
    {
        private readonly ICourtsRepository _courtsRepository;
        private readonly IReservationsRepository _reservationsRepository;
        private readonly UserManager<User> _userManager;

        public CreateModel(ICourtsRepository courtsRepository, IReservationsRepository reservationsRepository, UserManager<User> userManager)
        {
            _courtsRepository = courtsRepository;
            _reservationsRepository = reservationsRepository;
            _userManager = userManager;
        }

        [BindProperty]
        public Reservation Reservation { get; set; }
        public SelectList Courts { get; set; }
        public List<TimeSpan> AvailableTimes { get; set; } = new List<TimeSpan>();
        public List<TimeSpan> AvailableTimesEnd { get; set; } = new List<TimeSpan>();
        public List<(TimeSpan Hour, string Status)> AvailabilityAndOccupancy { get; set; } = new List<(TimeSpan Hour, string Status)>();

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int courtId, DateTime date)
        {
            Reservation = new Reservation { CourtId = courtId, Date = date };
            Courts = new SelectList(await _courtsRepository.GetAll(), "Id", "Name");

            Refresh();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Reservation.StartTime > Reservation.EndTime)
            {
                ErrorMessage = "Godzina rozpoczęcia nie może być późniejsza niż godzina zakończenia.";
                Refresh();
                return Page();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            bool isReservationAvailable = _reservationsRepository.CheckReservation(Reservation);

            if (!isReservationAvailable)
            {
                ErrorMessage = "Rezerwacja koliduje z istniejącymi rezerwacjami. Wybierz inne godziny.";
                Refresh();
                return Page();
            }

            if (currentUser != null)
            {
                var courtId = Reservation.CourtId;
                var court = await _courtsRepository.Get(courtId);

                // Obliczenie pełnej ceny
                double hours = (Reservation.EndTime - Reservation.StartTime).TotalHours;
                float fullPrice = (float)(hours * court.PricePerHour);

                Reservation.UserId = currentUser.Id;
                Reservation.FullPrice = fullPrice;

                await _reservationsRepository.Insert(Reservation);
                await _reservationsRepository.SaveChanges();

                TempData["ReservationMessage"] = "Rezerwacja dokonana pomyślnie!";
                return RedirectToPage("/Reservations/Index");
            }
            else
            {
                return Unauthorized();
            }
        }

        private void Refresh()
        {
            AvailableTimes = _reservationsRepository.GetAvailableTimes(Reservation.CourtId, Reservation.Date);
            AvailableTimesEnd = AvailableTimes.Select(time => time.Add(TimeSpan.FromHours(1))).ToList();
            AvailabilityAndOccupancy = _reservationsRepository.GetAvailabilityAndOccupancy(Reservation.CourtId, Reservation.Date);
        }
    }
}
