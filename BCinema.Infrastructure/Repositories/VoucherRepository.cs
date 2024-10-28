using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using BCinema.Persistence.Context;

namespace BCinema.Infrastructure.Repositories
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly ApplicationDbContext _context;

        public VoucherRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AnyAsync(
            Expression<Func<Voucher, bool>> predicate,
            CancellationToken cancellationToken)
        {
            return await _context.Vouchers.AnyAsync(predicate, cancellationToken);
        }

        public IQueryable<Voucher> GetVouchers()
        {
            return _context.Vouchers;
        }

        public async Task<IEnumerable<Voucher>> GetVouchersAsync(CancellationToken cancellationToken)
        {
            return await _context.Vouchers.ToListAsync(cancellationToken);
        }

        public async Task<Voucher?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Vouchers.
                FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
        }

        public async Task<Voucher?> GetByCoedAsync(string code, CancellationToken cancellationToken)
        {
            return await _context.Vouchers
                .FirstOrDefaultAsync(v => v.Code == code, cancellationToken);
        }

        public async Task AddVoucherAsync(Voucher voucher, CancellationToken cancellationToken)
        {
            await _context.Vouchers.AddAsync(voucher, cancellationToken);
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
