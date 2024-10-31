using BCinema.Domain.Entities;

namespace BCinema.Domain.Interfaces.IRepositories
{
    public interface IUserVoucherRepository
    {
        IQueryable<UserVoucher> GetUserVouchers();
        Task<IEnumerable<UserVoucher>> GetUserVouchersAsync(CancellationToken cancellationToken);
        Task<UserVoucher?> GetUserVoucherByUIdAndVIdAsync(
            Guid userId, Guid voucherId, CancellationToken cancellationToken);
        Task<IEnumerable<UserVoucher>> GetUserVouchersByUIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<UserVoucher> AddUserVoucherAsync(UserVoucher userVoucher, CancellationToken cancellationToken);
        Task SaveChangesAsync();
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
