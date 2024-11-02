using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using BCinema.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Infrastructure.Repositories;

public class FoodRepository(ApplicationDbContext context) : IFoodRepository
{
    public IQueryable<Food> GetFoods()
    {
        return context.Foods.AsQueryable();
    }

    public async Task<IEnumerable<Food>> GetFoodsAsync(CancellationToken cancellationToken)
    {
        return await context.Foods
            .Where(x => x.DeleteAt == null)
            .ToListAsync(cancellationToken);
    }

    public async Task<Food?> GetFoodByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Foods
            .FirstOrDefaultAsync(f => f.Id == id && f.DeleteAt == null, cancellationToken);
    }

    public async Task AddAsync(Food food, CancellationToken cancellationToken)
    {
        await context.Foods.AddAsync(food, cancellationToken);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}