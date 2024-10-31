using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using BCinema.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Infrastructure.Repositories
{
    public class UserVoucherRepository : IUserVoucherRepository
    {
        private readonly ApplicationDbContext _context;

        public UserVoucherRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<UserVoucher> GetUserVouchers()
        {
            return _context.UserVouchers;
        }

        public async Task<IEnumerable<UserVoucher>> GetUserVouchersAsync(CancellationToken cancellationToken)
        {
            return await _context.UserVouchers
                .Include(x => x.User)
                .Include(x => x.Voucher)
                .ToListAsync(cancellationToken);
        }

        public async Task<UserVoucher?> GetUserVoucherByUIdAndVIdAsync(
           Guid userId, Guid voucherId, CancellationToken cancellationToken)
        {
            return await _context.UserVouchers
                .Include(x => x.User)
                .Include(x => x.Voucher)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.VoucherId == voucherId, cancellationToken);
        }

        public async Task<IEnumerable<UserVoucher>> GetUserVouchersByUIdAsync(
            Guid userId,
            CancellationToken cancellationToken)
        {
            return await _context.UserVouchers
                .Include(x => x.User)
                .Include(x => x.Voucher)
                .Where(x => x.UserId == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task<UserVoucher> AddUserVoucherAsync(
            UserVoucher userVoucher,
            CancellationToken cancellationToken)
        {
            await _context.UserVouchers.AddAsync(userVoucher, cancellationToken);
            return userVoucher;
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
