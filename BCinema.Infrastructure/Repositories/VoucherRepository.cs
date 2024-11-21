using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using BCinema.Persistence.Context;

namespace BCinema.Infrastructure.Repositories
{
    public class VoucherRepository(ApplicationDbContext context) : IVoucherRepository
    {
        public async Task<bool> AnyAsync(Expression<Func<Voucher, bool>> predicate, CancellationToken cancellationToken)
        {
            return await context.Vouchers.AnyAsync(predicate, cancellationToken);
        }

        public IQueryable<Voucher> GetVouchers()
        {
            return context.Vouchers;
        }

        public async Task<IEnumerable<Voucher>> GetVouchersAsync(CancellationToken cancellationToken)
        {
            return await context.Vouchers.ToListAsync(cancellationToken);
        }

        public async Task<Voucher?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await context.Vouchers.
                FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
        }

        public async Task<Voucher?> GetByCoedAsync(string code, CancellationToken cancellationToken)
        {
            return await context.Vouchers
                .FirstOrDefaultAsync(v => v.Code == code, cancellationToken);
        }

        public async Task AddVoucherAsync(Voucher voucher, CancellationToken cancellationToken)
        {
            await context.Vouchers.AddAsync(voucher, cancellationToken);
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
