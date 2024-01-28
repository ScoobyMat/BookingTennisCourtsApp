using BookingTennisCourts.Data.Entities;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingTennisCourts.Repositories.Contracts
{
    public interface IReservationsRepository : IGenericRepository<Reservation>
    {
        Task<List<Reservation>> GetReservationsByUserId(string userId);
        List<TimeSpan> GetAvailableTimes(int? courtId, DateTime data);
        List<(TimeSpan Hour, string Status)> GetAvailabilityAndOccupancy(int? courtId, DateTime date);
        bool CheckReservation(Reservation reservation);
    }
}
