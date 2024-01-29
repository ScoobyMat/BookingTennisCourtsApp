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
    public class CourtsRepository : ICourtsRepository
    {
        private readonly BookingTennisCourtsAppDbContext _context;

        public CourtsRepository(BookingTennisCourtsAppDbContext context)
        {
            this._context = context;
        }

        public async Task<List<Court>> GetAll()
        {
            return await _context.Set<Court>().ToListAsync();
        }

        public async Task<Court> Get(int id)
        {
            return await _context.Set<Court>().FindAsync(id);
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.Set<Court>().AnyAsync(e => e.Id == id);
        }

        public async Task Insert(Court court)
        {
            await _context.Set<Court>().AddAsync(court);
            await SaveChanges();
        }

        public async Task Delete(int id)
        {
            var entity = await _context.Set<Court>().FindAsync(id);

            if (entity != null)
            {
                _context.Set<Court>().Remove(entity);
                await SaveChanges();
            }
        }

        public async Task Update(Court court)
        {
            _context.Set<Court>().Attach(court);
            _context.Entry(court).State = EntityState.Modified;
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