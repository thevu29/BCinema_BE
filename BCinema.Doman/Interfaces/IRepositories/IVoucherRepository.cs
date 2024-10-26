using BCinema.Domain.Entities;
using System.Linq.Expressions;

namespace BCinema.Domain.Interfaces.IRepositories
{
    public interface IVoucherRepository
    {
        IQueryable<Voucher> GetVouchers();
        Task<IEnumerable<Voucher>> GetVouchersAsync(CancellationToken cancellationToken);
        Task<Voucher?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Voucher?> GetByCoedAsync(string code, CancellationToken cancellationToken);
        Task AddVoucherAsync(Voucher voucher, CancellationToken cancellationToken);
        Task<bool> AnyAsync(Expression<Func<Voucher, bool>> predicate, CancellationToken cancellationToken);
        Task SaveChangesAsync();
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
