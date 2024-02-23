using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookingTennisCourts.Contracts;

namespace BookingTennisCourts.Pages
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IReservationsRepository _reservationRepository;
        private readonly ICourtsRepository _courtsRepository;
        private readonly UserManager<User> _userManager;

        public IndexModel(ILogger<IndexModel> logger, IReservationsRepository reservationRepository, ICourtsRepository courtsRepository, UserManager<User> userManager)
        {
            _logger = logger;
            _reservationRepository = reservationRepository;
            _courtsRepository = courtsRepository;
            _userManager = userManager;
        }

        public List<Court> Courts { get; set; } = new List<Court>();

        [BindProperty]
        public int CourtId { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public async Task OnGetAsync()
        {
            Date = DateTime.Today;
            Courts = await _courtsRepository.GetAll();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            return RedirectToPage("./Reservations/Create", new { courtId = CourtId, date = Date.ToString("yyyy-MM-dd") });
        }
    }
}
