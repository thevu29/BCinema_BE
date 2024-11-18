using BCinema.Domain.Entities;
using System.Linq.Expressions;

namespace BCinema.Domain.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        IQueryable<User> GetUsers();
        Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken);
        Task AddAsync(User user, CancellationToken cancellationToken);
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<User?> GetByEmailAndProviderAsync(string email, string provider, CancellationToken cancellationToken);
        Task<bool> AnyAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken);
        Task SaveChangesAsync();
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
