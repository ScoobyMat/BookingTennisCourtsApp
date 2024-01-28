using BookingTennisCourts.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingTennisCourts.Repositories.Contracts
{
    public interface ICourtsRepository : IGenericRepository<Court>
    {
        Task<bool> CourtHasReservations(int courtId);
        Task<string> GetCourtName(int courtId);
    }

}
