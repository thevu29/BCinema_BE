using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using BCinema.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Infrastructure.Repositories;

public class TokenRepository : ITokenRepository
{
    private readonly ApplicationDbContext _context;

    public TokenRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task AddTokenAsync(Token token, CancellationToken cancellationToken)
    {
        await _context.Tokens.AddAsync(token, cancellationToken);
    }

    public async Task<Token?> GetTokenByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        return await _context.Tokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}