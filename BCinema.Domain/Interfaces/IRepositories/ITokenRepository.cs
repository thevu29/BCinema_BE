using BCinema.Domain.Entities;

namespace BCinema.Domain.Interfaces.IRepositories;

public interface ITokenRepository
{
    Task AddTokenAsync(Token token, CancellationToken cancellationToken);
    Task<Token?> GetTokenByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}