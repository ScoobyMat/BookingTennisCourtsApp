﻿using BookingTennisCourts.Contracts;
using BookingTennisCourts.Data.Data;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace BookingTennisCourts.Repositories
{
    public class ReservationsRepository : IReservationsRepository
    {
        private readonly BookingTennisCourtsAppDbContext _context;

        public ReservationsRepository(BookingTennisCourtsAppDbContext context)
        {
            this._context = context;
        }

        public async Task<List<Reservation>> GetAll()
        {
            return await _context.Set<Reservation>()
                                 .OrderByDescending(r => r.Date)
                                 .ToListAsync();
        }


        public async Task<Reservation> Get(int id)
        {
            return await _context.Set<Reservation>().FindAsync(id);
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.Set<Reservation>().AnyAsync(e => e.Id == id);
        }

        public async Task Insert(Reservation reservation)
        {
            await _context.Set<Reservation>().AddAsync(reservation);
            await SaveChanges();
        }

        public async Task Delete(int id)
        {
            var entity = await _context.Set<Reservation>().FindAsync(id);

            if (entity != null)
            {
                _context.Set<Reservation>().Remove(entity);
                await SaveChanges();
            }
        }

        public async Task Update(Reservation entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await SaveChanges();
        }

        public async Task<int> SaveChanges()
        {
            foreach (var entity in _context.ChangeTracker.Entries<BaseDomainEntity>().Where(q => q.State == EntityState.Added))
            {
                entity.Entity.DateCreated = DateTime.Now;
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<List<Reservation>> GetReservationsByUserId(string userId)
        {
            return await _context.Reservations
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.Date)
                .ThenByDescending(r => r.StartTime)
                .ToListAsync();
        }

        public async Task<List<Reservation>> GetReservationsByUserLastName(string lastName)
        {
            // Pobierz identyfikatory użytkowników na podstawie podanego nazwiska
            var userIds = await _context.Users
                .Where(u => u.LastName == lastName)
                .Select(u => u.Id)
                .ToListAsync();

            // Pobierz rezerwacje dla znalezionych użytkowników
            var reservations = await _context.Reservations
                .Where(r => userIds.Contains(r.UserId))
                .ToListAsync();

            return reservations;
        }

        public async Task<List<Reservation>> GetReservationsByDate(DateTime date)
        {
            // Pobierz rezerwacje dla określonej daty
            var reservations = await _context.Reservations
                .Where(r => r.Date == date)
                .ToListAsync();

            return reservations;
        }


        public List<TimeSpan> GetAvailableTimes(int courtId, DateTime date)
        {
            var reservationsForCourtAndDate = GetReservationsForCourtAndDate(courtId, date);
            var bookedTimes = GetBookedTimes(reservationsForCourtAndDate);

            // Wybierz dostępne godziny spośród dostępnych w godzinach 10-18, które nie są zarezerwowane
            var availableTimes = Enumerable.Range(10, 8)
                .Select(hour => new TimeSpan(hour, 0, 0))
                .Where(time => !bookedTimes.Contains(time))
                .ToList();

            return availableTimes;
        }

        public bool CheckReservation(Reservation reservation)
        {
            // Pobierz istniejące rezerwacje dla danego kortu i daty
            var existingReservations = GetExistingReservations(reservation.CourtId, reservation.Date);

            // Sprawdź, czy nowa rezerwacja koliduje z istniejącymi rezerwacjami
            return existingReservations.All(existingReservation =>
                !(reservation.StartTime < existingReservation.EndTime && reservation.EndTime > existingReservation.StartTime) &&
                !(reservation.EndTime > existingReservation.StartTime && reservation.StartTime < existingReservation.EndTime)
            );
        }

        public List<(TimeSpan Hour, string Status)> GetAvailabilityAndOccupancy(int courtId, DateTime date)
        {
            var reservationsForCourtAndDate = GetReservationsForCourtAndDate(courtId, date);

            // Utwórz listę dostępnych i zajętych godzin
            var availableAndOccupiedHours = Enumerable.Range(10, 8)
                .Select(hour =>
                {
                    var currentHour = new TimeSpan(hour, 0, 0);
                    var isBooked = reservationsForCourtAndDate.Any(r => currentHour >= r.StartTime && currentHour < r.EndTime);
                    var status = isBooked ? "Zarezerwowane" : "Dostępne";
                    return (currentHour, status);
                })
                .ToList();
            return availableAndOccupiedHours;
        }

        // Pobierz rezerwacje dla danego kortu i daty
        private List<Reservation> GetReservationsForCourtAndDate(int courtId, DateTime date)
        {
            return _context.Reservations
                .Where(r => r.CourtId == courtId && r.Date.Date == date.Date)
                .ToList();
        }

        // Pobierz zarezerwowane godziny z listy rezerwacji
        private List<TimeSpan> GetBookedTimes(List<Reservation> reservations)
        {
            return reservations
                .SelectMany(r => Enumerable.Range(r.StartTime.Hours, r.EndTime.Hours - r.StartTime.Hours)
                    .Select(hour => new TimeSpan(hour, 0, 0))
                    .ToList())
                .ToList();
        }

        // Pobierz istniejące rezerwacje dla danego kortu i daty
        private List<Reservation> GetExistingReservations(int courtId, DateTime date)
        {
            return _context.Reservations
                .Where(r => r.CourtId == courtId && r.Date == date)
                .ToList();
        }

    }
}
