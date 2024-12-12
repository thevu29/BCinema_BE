using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using BCinema.Persistence.Context;

namespace BCinema.Infrastructure.Repositories
{
    public class UserRepository(ApplicationDbContext context) : IUserRepository
    {
        public async Task<int> CountAsync(int year, int month, CancellationToken cancellationToken)
        {
            return await context.Users
                .Include(u => u.Role)
                .Where(u => u.CreateAt.Year == year && u.CreateAt.Month == month && u.Role.Name == "User" && u.DeleteAt == null)
                .CountAsync(cancellationToken);
        }
        
        public Task<User?> GetByEmailAndProviderAsync(string email, string provider, CancellationToken cancellationToken)
        {
            return context.Users
                .Include(u => u.Role)
                .Where(u => u.DeleteAt == null)
                .FirstOrDefaultAsync(u => u.Email == email && u.Provider == provider, cancellationToken);
        }

        public async Task<bool> AnyAsync(
            Expression<Func<User, bool>> predicate,
            CancellationToken cancellationToken)
        {
            return await context.Users.AnyAsync(predicate, cancellationToken);
        }

        public IQueryable<User> GetUsers()
        {
            return context.Users.AsQueryable().Include(u => u.Role);
        }

        public async Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken)
        {
            return await context.Users
                .Include(u => u.Role)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken)
        {
            await context.Users.AddAsync(user, cancellationToken);
        }

        public void Delete(User user)
        {
            context.Users.Remove(user);
            context.SaveChanges();
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await context.Users
                .Include(u => u.Role)
                .Where(u => u.DeleteAt == null)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await context.Users
                .Include(u => u.Role)
                .Where(u => u.DeleteAt == null)
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await context.SaveChangesAsync(cancellationToken);
        }

        public Task DeleteInActiveUsersAsync(CancellationToken cancellationToken)
        {
            var users = context.Users.Where(u => u.IsActivated == false);
            context.Users.RemoveRange(users);
            return context.SaveChangesAsync(cancellationToken);
        }
    }
}
