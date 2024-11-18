using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using BCinema.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Infrastructure.Repositories;

public class OtpRepository(ApplicationDbContext context) : IOtpRepository
{
    public async Task AddAsync(Otp otp, CancellationToken cancellationToken)
    {
        await context.Otps.AddAsync(otp, cancellationToken);
    }

    public void DeleteAsync(Otp otp)
    {
        context.Otps.Remove(otp);
        context.SaveChanges();
    }

    public async Task<Otp?> GetByCodeAsync(string code, CancellationToken cancellationToken)
    {
        return await context.Otps
            .Include(o => o.User)
            .FirstOrDefaultAsync(x => x.Code == code, cancellationToken);
    }

    public Task<Otp?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return context.Otps
            .Include(o => o.User)
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}