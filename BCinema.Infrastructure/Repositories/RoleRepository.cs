using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using BCinema.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BCinema.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Role> GetRoles()
        {
            return _context.Roles.AsQueryable();
        }

        public async Task<IEnumerable<Role>> GetRolesAsync(CancellationToken cancellationToken)
        {
            return await _context.Roles.ToListAsync(cancellationToken);
        }

        public async Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<bool> AnyAsync(
            Expression<Func<Role, bool>> predicate,
            CancellationToken cancellationToken)
        {
            return await _context.Roles.AnyAsync(predicate, cancellationToken);
        }

        public async Task AddAsync(Role role, CancellationToken cancellationToken)
        {
            await _context.Roles.AddAsync(role, cancellationToken);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
