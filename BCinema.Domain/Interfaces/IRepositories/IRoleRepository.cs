using BCinema.Domain.Entities;
using System.Linq.Expressions;

namespace BCinema.Domain.Interfaces.IRepositories
{
    public interface IRoleRepository
    {
        IQueryable<Role> GetRoles();
        Task<IEnumerable<Role>> GetRolesAsync(CancellationToken cancellationToken);
        Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task AddAsync(Role role, CancellationToken cancellationToken);
        Task<bool> AnyAsync(Expression<Func<Role, bool>> predicate, CancellationToken cancellationToken);
        Task SaveChangesAsync();
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
