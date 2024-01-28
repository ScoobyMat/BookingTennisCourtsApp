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

        public List<(TimeSpan Hour, string Status)> AvailabilityAndOccupancy { get; set; } = new List<(TimeSpan Hour, string Status)>();

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int courtId, string date, string errorMessage = null)
        {
            Reservation = new Reservation
            {
                CourtId = courtId,
                Data = DateTime.Parse(date),
            };

            // Sprawdź, czy jest błąd i przypisz komunikat
            if (!string.IsNullOrEmpty(errorMessage))
            {
                ErrorMessage = errorMessage;

                // Ponownie pobierz listy dostępnych godzin
                RefreshAvailableTimes();
                AvailabilityAndOccupancy = _reservationRepository.GetAvailabilityAndOccupancy(Reservation.CourtId, Reservation.Data);

                return Page();
            }

            AvailableTimes = _reservationRepository.GetAvailableTimes(Reservation.CourtId, Reservation.Data);
            AvailableTimesEnd = AvailableTimes.Select(time => time.Add(TimeSpan.FromHours(1))).ToList();

            // Dodaj kod do pobrania informacji o dostępności i zajętych godzinach
            AvailabilityAndOccupancy = _reservationRepository.GetAvailabilityAndOccupancy(Reservation.CourtId, Reservation.Data);

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Ponownie pobierz listy dostępnych godzin
                RefreshAvailableTimes();
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Użytkownik nie został znaleziony");
            }

            Reservation.UserId = user.Id;

            // Sprawdź, czy rezerwacja koliduje z innymi
            bool isReservationAvailable = _reservationRepository.CheckReservation(Reservation);

            // Dodaj log, aby zobaczyć wynik sprawdzania dostępności
            Console.WriteLine($"Is Reservation Available: {isReservationAvailable}");

            if (!isReservationAvailable)
            {
                // Rezerwacja koliduje, przypisz komunikat błędu
                ErrorMessage = "Rezerwacja koliduje z istniejącymi rezerwacjami. Wybierz inne godziny.";

                // Ponownie pobierz listy dostępnych godzin
                RefreshAvailableTimes();

                return Page();
            }

            // Rezerwacja nie koliduje, dodaj do bazy danych
            await _reservationRepository.Insert(Reservation);

            // Przekieruj do strony /Reservations/Index
            return RedirectToPage("/Reservations/Index");
        }

        private void RefreshAvailableTimes()
        {
            AvailableTimes = _reservationRepository.GetAvailableTimes(Reservation.CourtId, Reservation.Data);
            AvailableTimesEnd = AvailableTimes.Select(time => time.Add(TimeSpan.FromHours(1))).ToList();
        }
    }
}
