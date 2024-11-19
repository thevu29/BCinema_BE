using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using BCinema.Persistence.Context;

namespace BCinema.Infrastructure.Repositories
{
    public class SeatTypeRepository(ApplicationDbContext context) : ISeatTypeRepository
    {
        public async Task<bool> AnyAsync(Expression<Func<SeatType, bool>> predicate, CancellationToken cancellationToken)
        {
            return await context.SeatTypes.AnyAsync(predicate, cancellationToken);
        }

        public IQueryable<SeatType> GetSeatTypes()
        {
            return context.SeatTypes;
        }

        public async Task<IEnumerable<SeatType>> GetSeatTypesAsync(CancellationToken cancellation)
        {
            return await context.SeatTypes
                .Where(st => st.DeleteAt == null)
                .ToListAsync(cancellation);
        }

        public async Task<SeatType?> GetByIdAsync(Guid id, CancellationToken cancellation)
        {
            return await context.SeatTypes
                .FirstOrDefaultAsync(st => st.Id == id && st.DeleteAt == null, cancellation);
        }

        public async Task<SeatType?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await context.SeatTypes
                .FirstOrDefaultAsync(st => st.Name == name && st.DeleteAt == null, cancellationToken);
        }

        public async Task AddAsync(SeatType seatType, CancellationToken cancellation)
        {
            await context.SeatTypes.AddAsync(seatType, cancellation);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync(CancellationToken cancellation)
        {
            await context.SaveChangesAsync(cancellation);
        }
    }
}
