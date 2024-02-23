using Microsoft.AspNetCore.Mvc.RazorPages;
using BookingTennisCourts.Contracts;

namespace BookingTennisCourts.Pages.Courts
{
    public class IndexModel : PageModel
    {
        private readonly ICourtsRepository _courtsRepository;

        public IndexModel(ICourtsRepository courtsRepository)
        {
            _courtsRepository = courtsRepository;
        }

        public IList<Court> Court { get; set; }

        public async Task OnGetAsync()
        {
            Court = await _courtsRepository.GetAll();
        }
    }
}
