using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookingTennisCourts.Repositories.Contracts;
using Microsoft.AspNetCore.Authorization;
using BookingTennisCourts.Data.Entities;

namespace BookingTennisCourts.Pages.Courts
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ICourtsRepository _courtsRepository;

        public IndexModel(ICourtsRepository repository)
        {
            _courtsRepository = repository;
        }

        public IList<Court> Court { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Court = await _courtsRepository.GetAll();
        }
    }
}
