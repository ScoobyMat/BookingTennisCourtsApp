using BookingTennisCourts.Data;
using BookingTennisCourts.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingTennisCourts.Repositories.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseDomainEntity
    {

        private readonly BookingTennisCourtsAppDbContext _context;
        private readonly DbSet<TEntity> _db;

        public GenericRepository(BookingTennisCourtsAppDbContext context)
        {
            this._context = context;
            _db = _context.Set<TEntity>();
        }

        public async Task Delete(int id)
        {
            var entity = await _db.FindAsync(id);

            if (entity != null)
            {
                _db.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<bool> Exists(int id)
        {
            return await _db.AnyAsync(e => e.Id == id);
        }

        public async Task<TEntity> Get(int id)
        {
            return await _db.FindAsync(id);
        }

        public async Task<List<TEntity>> GetAll()
        {
            return await _db.ToListAsync();
        }

        public async Task Insert(TEntity entity)
        {
            await _db.AddAsync(entity);
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

        public async Task Update(TEntity entity)
        {
            _db.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await SaveChanges();
        }

        public async Task<bool> HasReservations(int courtId)
        {
            return await _context.Reservations.AnyAsync(r => r.CourtId == courtId);
        }
    }
}
