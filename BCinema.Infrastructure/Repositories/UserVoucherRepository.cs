using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using BCinema.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Infrastructure.Repositories
{
    public class UserVoucherRepository(ApplicationDbContext context) : IUserVoucherRepository
    {
        public IQueryable<UserVoucher> GetUserVouchers()
        {
            return context.UserVouchers;
        }

        public async Task<IEnumerable<UserVoucher>> GetUserVouchersAsync(CancellationToken cancellationToken)
        {
            return await context.UserVouchers
                .Include(x => x.User)
                .Include(x => x.Voucher)
                .ToListAsync(cancellationToken);
        }

        public async Task<UserVoucher?> GetUserVoucherByUIdAndVIdAsync(
           Guid userId, Guid voucherId, CancellationToken cancellationToken)
        {
            return await context.UserVouchers
                .Include(x => x.User)
                .Include(x => x.Voucher)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.VoucherId == voucherId, cancellationToken);
        }
        
        public async Task<UserVoucher?> GetUserVoucherByUIdAndVCodeAsync(
            Guid userId,
            string code,
            CancellationToken cancellationToken)
        {
            return await context.UserVouchers
                .Include(x => x.User)
                .Include(x => x.Voucher)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Voucher.Code == code, cancellationToken);
        }

        public async Task<IEnumerable<UserVoucher>> GetUserVouchersByUIdAsync(
            Guid userId,
            CancellationToken cancellationToken)
        {
            return await context.UserVouchers
                .Include(x => x.User)
                .Include(x => x.Voucher)
                .Where(x => x.UserId == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task<UserVoucher> AddUserVoucherAsync(
            UserVoucher userVoucher,
            CancellationToken cancellationToken)
        {
            await context.UserVouchers.AddAsync(userVoucher, cancellationToken);
            return userVoucher;
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
