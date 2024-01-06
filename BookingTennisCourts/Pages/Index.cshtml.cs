using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using BookingTennisCourts.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BookingTennisCourts.Pages
{
    public class IndexModel : PageModel
    {
        private readonly BookingTennisCourtsAppDbContext _context;

        [BindProperty(SupportsGet = true)]
        public int SelectedCourt { get; set; }

        [BindProperty]
        public DateTime ReservationData { get; set; } = DateTime.Now;

        [BindProperty]
        public TimeSpan StartTime { get; set; }

        [BindProperty]
        public string SelectedHour { get; set; }

        [BindProperty]
        public TimeSpan EndTime { get; set; }

        [BindProperty]
        public string SelectedEndHour { get; set; }

        public List<Court> Courts { get; set; }

        public List<Reservation> Reservations { get; set; }

        public IndexModel(BookingTennisCourtsAppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            LoadCourts();
            LoadReservations();
            return Page();
        }

        public IActionResult OnPostSelectCourt()
        {
            LoadCourts();
            LoadReservations();
            return Page();
        }

        public IActionResult OnPostSelectHour()
        {
            LoadCourts();
            LoadReservations();
            StartTime = TimeSpan.Parse(SelectedHour);
            return Page();
        }

        public IActionResult OnPostSelectEndHour()
        {
            LoadCourts();
            LoadReservations();
            StartTime = TimeSpan.Parse(SelectedHour);
            EndTime = TimeSpan.Parse(SelectedEndHour);
            return Page();
        }

        public IActionResult OnPostCreateReservation()
        {
            if (!ModelState.IsValid)
            {
                LoadCourts();
                LoadReservations();
                return Page();
            }

            var startTime = TimeSpan.Parse(SelectedHour);
            var endTime = TimeSpan.Parse(SelectedEndHour);

            // Pobierz aktualnie zalogowanego użytkownika
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var reservation = new Reservation
            {
                CourtId = SelectedCourt,
                Data = ReservationData,
                StartTime = startTime,
                EndTime = endTime,
                UserId = userId,
            };

            _context.Reservations.Add(reservation);
            _context.SaveChanges();

            return RedirectToPage("Reservations/Index");
        }


        private void LoadCourts()
        {
            Courts = _context.Courts.ToList();
        }

        private void LoadReservations()
        {
            Reservations = _context.Reservations
                .Where(r => r.CourtId == SelectedCourt && r.Data == ReservationData.Date)
                .ToList();
        }


    }
}
