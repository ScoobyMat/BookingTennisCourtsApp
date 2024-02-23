using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookingTennisCourts.Contracts;
using BookingTennisCourts.Repositories;

namespace BookingTennisCourts.Pages.Reservations
{
    public class EditModel : PageModel
    {
        private readonly IReservationsRepository _reservationsRepository;
        private readonly ICourtsRepository _courtsRepository;

        public EditModel(IReservationsRepository reservationsRepository, ICourtsRepository courtsRepository)
        {
            _reservationsRepository = reservationsRepository;
            _courtsRepository = courtsRepository;
        }

        [BindProperty]
        public Reservation Reservation { get; set; }

        public SelectList Courts { get; set; }

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

        public async Task<IActionResult> OnPostAsync()
        {
            // Sprawdź dostępność nowego terminu rezerwacji
            var isReservationAvailable = _reservationsRepository.CheckReservation(Reservation);
            if (!isReservationAvailable)
            {
                ModelState.AddModelError(string.Empty, "Wybrany termin jest już zarezerwowany.");
                Courts = new SelectList(await _courtsRepository.GetAll(), "Id", "Name");
                return Page();
            }

            var courtId = Reservation.CourtId;
            var court = await _courtsRepository.Get(courtId);
            double hours = (Reservation.EndTime - Reservation.StartTime).TotalHours;
            float fullPrice = (float)(hours * court.PricePerHour);

            Reservation.FullPrice = fullPrice;
            var Edit = await _reservationsRepository.Get(Reservation.Id);

            Edit.CourtId = Reservation.CourtId;
            Edit.Date = Reservation.Date;
            Edit.StartTime = Reservation.StartTime;
            Edit.EndTime = Reservation.EndTime;
            Edit.UserId = Reservation.UserId;
            Edit.FullPrice = fullPrice;

            await _reservationsRepository.Update(Edit);
            await _reservationsRepository.SaveChanges();

            return RedirectToPage("./Index");
        }
    }
}