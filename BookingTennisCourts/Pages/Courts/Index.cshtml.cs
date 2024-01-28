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
        private readonly ICourtsRepository _repository;

        public IndexModel(ICourtsRepository repository)
        {
            _repository = repository;
        }

        public IList<Court> Court { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Court = await _repository.GetAll();
        }
    }
}
