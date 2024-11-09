using Microsoft.EntityFrameworkCore;

namespace BCinema.Application.Helpers;

public class PaginatedList<T>(int page, int size, int count, IEnumerable<T> data)
{
    public int Page { get; set; } = page;
    public int Size { get; set; } = size;
    public int TotalPages { get; set; } = (int) Math.Ceiling(count / (double) size);
    public int TotalElements { get; set; } = count;
    public IEnumerable<T> Data { get; set; } = data;

    public static async Task<PaginatedList<T>> ToPageList(IQueryable<T> source, int page, int size)
    {
        var property = typeof(T).GetProperty("DeleteAt");
        
        if (property != null)
        {
            source = source.Where(x => x != null && EF.Property<DateTime?>(x, "DeleteAt") == null);
        }
        
        var count = await source.CountAsync();
        var data = await source.Skip((page - 1) * size).Take(size).ToListAsync();
        return new PaginatedList<T>(page, size, count, data);
    }
}