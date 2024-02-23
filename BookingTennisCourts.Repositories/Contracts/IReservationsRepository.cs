namespace BookingTennisCourts.Contracts
{
    public interface IReservationsRepository
    {
        Task<List<Reservation>> GetAll();
        Task<Reservation> Get(int id);
        Task<bool> Exists(int id);
        Task Insert(Reservation entity);
        Task Delete(int id);
        Task Update(Reservation entity);
        Task<int> SaveChanges();
        Task<List<Reservation>> GetReservationsByUserId(string userId);
        List<TimeSpan> GetAvailableTimes(int courtId, DateTime date);
        List<(TimeSpan Hour, string Status)> GetAvailabilityAndOccupancy(int courtId, DateTime date);
        bool CheckReservation(Reservation reservation);
        Task<List<Reservation>> GetReservationsByUserLastName(string lastName);
        Task<List<Reservation>> GetReservationsByDate(DateTime date);
    }
}
