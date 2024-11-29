using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using BCinema.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BCinema.Infrastructure.Repositories;

public class PaymentRepository(ApplicationDbContext context) : IPaymentRepository
{
    public IQueryable<Payment> GetPayments()
    {
        return context.Payments
            .AsQueryable()
            .Include(p => p.User)
            .Include(p => p.Schedule)
            .Include(p => p.Voucher)
            .Include(p => p.PaymentDetails);
    }
    
    public async Task<IEnumerable<Payment>> GetPaymentsAsync(CancellationToken cancellationToken)
    {
        return await context.Payments
            .Include(p => p.User)
            .Include(p => p.Schedule)
            .Include(p => p.Voucher)
            .Include(p => p.PaymentDetails)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<IEnumerable<Payment>> GetUserPaymentsAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await context.Payments
            .Where(p => p.UserId == userId)
            .Include(p => p.User)
            .Include(p => p.Schedule)
            .Include(p => p.Voucher)
            .Include(p => p.PaymentDetails)
            .ToListAsync(cancellationToken);
    }
    
    public async Task<Payment?> GetPaymentByIdAsync(Guid paymentId, CancellationToken cancellationToken)
    {
        return await context.Payments
            .Include(p => p.User)
            .Include(p => p.Schedule)
            .Include(p => p.Voucher)
            .Include(p => p.PaymentDetails)
            .FirstOrDefaultAsync(p => p.Id == paymentId, cancellationToken);
    }
    
    public async Task AddPaymentAsync(Payment payment, CancellationToken cancellationToken)
    {
        await context.Payments.AddAsync(payment, cancellationToken);
    }
    
    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return await context.Database.BeginTransactionAsync(cancellationToken);
    }
    
    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task DeletePaymentAsync(Payment payment, CancellationToken cancellationToken)
    {
        context.Payments.Remove(payment);
        return Task.CompletedTask;
    }
}