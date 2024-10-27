using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using BCinema.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BCinema.Infrastructure.Repositories
{
    public class SeatTypeRepository : ISeatTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public SeatTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AnyAsync(
            Expression<Func<SeatType, bool>> predicate,
            CancellationToken cancellationToken)
        {
            return await _context.SeatTypes.AnyAsync(predicate, cancellationToken);
        }

        public IQueryable<SeatType> GetSeatTypes()
        {
            return _context.SeatTypes;
        }

        public async Task<IEnumerable<SeatType>> GetSeatTypesAsync(CancellationToken cancellation)
        {
            return await _context.SeatTypes.ToListAsync(cancellation);
        }

        public async Task<SeatType?> GetByIdAsync(Guid id, CancellationToken cancellation)
        {
            return await _context.SeatTypes.FirstOrDefaultAsync(s => s.Id == id, cancellation);
        }

        public async Task<SeatType?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.SeatTypes.FirstOrDefaultAsync(s => s.Name == name, cancellationToken);
        }

        public async Task AddAsync(SeatType seatType, CancellationToken cancellation)
        {
            await _context.SeatTypes.AddAsync(seatType, cancellation);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync(CancellationToken cancellation)
        {
            await _context.SaveChangesAsync(cancellation);
        }
    }
}
