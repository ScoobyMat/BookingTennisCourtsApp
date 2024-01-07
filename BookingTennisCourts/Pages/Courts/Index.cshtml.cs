using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BookingTennisCourts.Data;
using BookingTennisCourts.Repositories.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace BookingTennisCourts.Pages.Courts
{
    [Authorize(Roles = "Admin")]

    public class IndexModel : PageModel
    {
        private readonly IGenericRepository<Court> _repository;

        public IndexModel(IGenericRepository<Court> repository)
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
