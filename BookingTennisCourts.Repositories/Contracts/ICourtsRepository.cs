namespace BookingTennisCourts.Contracts
{
    public interface ICourtsRepository
    {
        Task<List<Court>> GetAll();
        Task<Court> Get(int id);
        Task<bool> Exists(int id);
        Task Insert(Court entity);
        Task Delete(int id);
        Task Update(Court entity);
        Task<int> SaveChanges();
        Task<bool> CourtHasReservations(int courtId);
        Task<string> GetCourtName(int courtId);
    }
}
