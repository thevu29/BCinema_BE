using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using BCinema.Persistence.Context;

namespace BCinema.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<User?> GetByEmailAndProviderAsync(string email, string provider, CancellationToken cancellationToken)
        {
            return _context.Users
                .Include(u => u.Role)
                .Where(u => u.DeleteAt == null)
                .FirstOrDefaultAsync(u => u.Email == email && u.Provider == provider, cancellationToken);
        }

        public async Task<bool> AnyAsync(
            Expression<Func<User, bool>> predicate,
            CancellationToken cancellationToken)
        {
            return await _context.Users.AnyAsync(predicate, cancellationToken);
        }

        public IQueryable<User> GetUsers()
        {
            return _context.Users.AsQueryable().Include(u => u.Role);
        }

        public async Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken)
        {
            return await _context.Users
                .Include(u => u.Role)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken)
        {
            await _context.Users.AddAsync(user, cancellationToken);
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Where(u => u.DeleteAt == null)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Where(u => u.DeleteAt == null)
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public Task DeleteInActiveUsersAsync(CancellationToken cancellationToken)
        {
            var users = _context.Users.Where(u => u.IsActivated == false);
            _context.Users.RemoveRange(users);
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
