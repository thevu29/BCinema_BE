using BCinema.Domain.Entities;

namespace BCinema.Domain.Interfaces.IRepositories
{
    public interface IOtpRepository
    {
        Task AddAsync(Otp otp, CancellationToken cancellationToken);
        void Delete(Otp otp);
        Task<Otp?> GetByCodeAsync(string code, CancellationToken cancellationToken);
        Task<Otp?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
