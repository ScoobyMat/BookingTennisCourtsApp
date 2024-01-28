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

        public async Task<List<TimeSpan>> GetAvailableTimeSlots(int? courtId, DateTime date)
        {
            var takenTimeSlots = await _context.Reservations
                .Where(r => r.CourtId == courtId && r.Data == date)
                .Select(r => new { r.StartTime, r.EndTime })
                .ToListAsync();

            var allTimeSlots = GenerateAllPossibleTimeSlots();

            var availableTimeSlots = allTimeSlots.Except(
                takenTimeSlots.SelectMany(r => GenerateTimeSlotsBetween(r.StartTime, r.EndTime))
            ).ToList();

            return availableTimeSlots;
        }

        private List<TimeSpan> GenerateAllPossibleTimeSlots()
        {
            // Assuming your courts are open from 10 AM to 6 PM
            var startTime = new TimeSpan(10, 0, 0);
            var endTime = new TimeSpan(18, 0, 0);

            var timeSlots = new List<TimeSpan>();

            while (startTime < endTime)
            {
                timeSlots.Add(startTime);
                startTime = startTime.Add(new TimeSpan(0, 30, 0)); // Assuming 30-minute time slots
            }

            return timeSlots;
        }

        private List<TimeSpan> GenerateTimeSlotsBetween(TimeSpan start, TimeSpan end)
        {
            var timeSlots = new List<TimeSpan>();

            while (start < end)
            {
                timeSlots.Add(start);
                start = start.Add(new TimeSpan(0, 30, 0)); // Assuming 30-minute time slots
            }

            return timeSlots;
        }

    }
}