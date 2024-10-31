using BCinema.Domain.Entities;
using System.Linq.Expressions;

namespace BCinema.Domain.Interfaces.IRepositories
{
    public interface ISeatTypeRepository
    {
        IQueryable<SeatType> GetSeatTypes();
        Task<IEnumerable<SeatType>> GetSeatTypesAsync(CancellationToken cancellation);
        Task<SeatType?> GetByIdAsync(Guid id, CancellationToken cancellation);
        Task<SeatType?> GetByNameAsync(string name, CancellationToken cancellation);
        Task AddAsync(SeatType seatType, CancellationToken cancellation);
        Task<bool> AnyAsync(Expression<Func<SeatType, bool>> predicate, CancellationToken cancellationToken);
        Task SaveChangesAsync();
        Task SaveChangesAsync(CancellationToken cancellation);
    }
}
