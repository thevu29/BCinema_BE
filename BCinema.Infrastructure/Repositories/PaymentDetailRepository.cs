using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using BCinema.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Infrastructure.Repositories;

public class PaymentDetailRepository : IPaymentDetailRepository
{
    private readonly ApplicationDbContext _context;
    
    public PaymentDetailRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<PaymentDetail>> GetPaymentDetailsAsync(CancellationToken cancellationToken)
    {
        return await _context.PaymentDetails
            .Include(pd => pd.Payment)
            .Include(pd => pd.SeatSchedule)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<PaymentDetail>> GetPaymentDetailsByPaymentIdAsync(Guid paymentId, CancellationToken cancellationToken)
    {
        return await _context.PaymentDetails
            .Where(pd => pd.PaymentId == paymentId)
            .Include(pd => pd.Payment)
            .Include(pd => pd.SeatSchedule)
            .ToListAsync(cancellationToken);
    }
    
    public async Task AddPaymentDetailsAsync(IEnumerable<PaymentDetail> paymentDetails, CancellationToken cancellationToken)
    {
        await _context.PaymentDetails.AddRangeAsync(paymentDetails, cancellationToken);
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