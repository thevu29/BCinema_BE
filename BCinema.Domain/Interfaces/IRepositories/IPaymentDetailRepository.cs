using BCinema.Domain.Entities;

namespace BCinema.Domain.Interfaces.IRepositories;

public interface IPaymentDetailRepository
{
    Task<IEnumerable<PaymentDetail>> GetPaymentDetailsAsync(CancellationToken cancellationToken);
    Task<IEnumerable<PaymentDetail>> GetPaymentDetailsByPaymentIdAsync(Guid paymentId, CancellationToken cancellationToken);
    Task AddPaymentDetailsAsync(IEnumerable<PaymentDetail> paymentDetails, CancellationToken cancellationToken);
    Task SaveChangesAsync();
    Task SaveChangesAsync(CancellationToken cancellationToken);
    Task DeletePaymentDetails(IEnumerable<PaymentDetail> paymentDetails, CancellationToken cancellationToken);
}