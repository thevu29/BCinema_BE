using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using BCinema.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BCinema.Infrastructure.Repositories;

public class FoodRepository : IFoodRepository
{
    private readonly ApplicationDbContext _context;

    public FoodRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IQueryable<Food> GetFoods()
    {
        return _context.Foods.AsQueryable();
    }

    public async Task<IEnumerable<Food>> GetFoodsAsync(CancellationToken cancellationToken)
    {
        return await _context.Foods.ToListAsync(cancellationToken);
    }

    public async Task<Food?> GetFoodByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Foods.FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    public async Task AddAsync(Food food, CancellationToken cancellationToken)
    {
        await _context.Foods.AddAsync(food, cancellationToken);
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