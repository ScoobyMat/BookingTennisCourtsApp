using BookingTennisCourts.Data.Data;
using BookingTennisCourts.Data.Entities;
using BookingTennisCourts.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingTennisCourts.Repositories.Repositories
{
    public class ReservationsRepository : GenericRepository<Reservation>, IReservationsRepository
    {
        private readonly BookingTennisCourtsAppDbContext _context;

        public ReservationsRepository(BookingTennisCourtsAppDbContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<List<Reservation>> GetReservationsByUserId(string userId)
        {
            return await _context.Reservations
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public List<TimeSpan> GetAvailableTimes(int? courtId, DateTime date)
        {
            var reservationsForCourtAndDate = _context.Reservations
                .Where(r => r.CourtId == courtId && r.Data.Date == date.Date)
                .ToList();

            var bookedTimes = reservationsForCourtAndDate
                .SelectMany(r => Enumerable.Range(r.StartTime.Hours, r.EndTime.Hours - r.StartTime.Hours)
                    .Select(hour => new TimeSpan(hour, 0, 0))
                    .ToList())
                .ToList();

            var availableTimes = Enumerable.Range(10, 8)
                .Select(hour => new TimeSpan(hour, 0, 0))
                .Where(time => !bookedTimes.Contains(time))
                .ToList();

            return availableTimes;
        }



    }
}
