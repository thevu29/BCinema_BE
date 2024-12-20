﻿using BCinema.Domain.Entities;
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

    public async Task DeleteByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var tokens = await _context.Tokens
            .Where(t => t.UserId == userId)
            .ToListAsync(cancellationToken);
        _context.Tokens.RemoveRange(tokens);
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task DeleteRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var token = await _context.Tokens
            .FirstOrDefaultAsync(t => t.RefreshToken == refreshToken, cancellationToken);
        
        if (token is not null)
        {
            _context.Tokens.Remove(token);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}