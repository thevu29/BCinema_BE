using BCinema.Domain.Entities;

namespace BCinema.Domain.Interfaces.IRepositories;

public interface IFoodRepository
{
    IQueryable<Food> GetFoods();
    Task<IEnumerable<Food>> GetFoodsAsync(CancellationToken cancellationToken);
    Task<Food?> GetFoodByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(Food food, CancellationToken cancellationToken);
    Task SaveChangesAsync();
    Task SaveChangesAsync(CancellationToken cancellationToken);
}