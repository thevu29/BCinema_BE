using Microsoft.EntityFrameworkCore.Storage;
using BCinema.Domain.Entities;

namespace BCinema.Domain.Interfaces.IRepositories;

public interface IPaymentRepository
{
    IQueryable<Payment> GetPayments();
    Task<IEnumerable<Payment>> GetPaymentsAsync(CancellationToken cancellationToken);
    Task<IEnumerable<Payment>> GetUserPaymentsAsync(Guid userId, CancellationToken cancellationToken);
    Task<Payment?> GetPaymentByIdAsync(Guid paymentId, CancellationToken cancellationToken);
    Task AddPaymentAsync(Payment payment, CancellationToken cancellationToken);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
    Task SaveChangesAsync();
    Task SaveChangesAsync(CancellationToken cancellationToken);
}