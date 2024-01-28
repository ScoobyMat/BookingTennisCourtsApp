using BookingTennisCourts.Data.Data;
using BookingTennisCourts.Data.Entities;
using BookingTennisCourts.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingTennisCourts.Repositories.Repositories
{
    public class CourtsRepository : GenericRepository<Court>, ICourtsRepository
    {
        private readonly BookingTennisCourtsAppDbContext _context;

        public CourtsRepository(BookingTennisCourtsAppDbContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<bool> CourtHasReservations(int courtId)
        {
            return await _context.Reservations.AnyAsync(r => r.CourtId == courtId);
        }

        public async Task<string> GetCourtName(int courtId)
        {
            var court = await _context.Courts.FindAsync(courtId);
            return court?.Name;
        }

    }
}