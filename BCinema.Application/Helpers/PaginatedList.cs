using Microsoft.EntityFrameworkCore;

namespace BCinema.Application.Helpers;

public class PaginatedList<T>
{
    public int Page { get; set; }
    public int Size { get; set; }
    public int TotalPages { get; set; }
    public int TotalElements { get; set; }
    public IEnumerable<T> Data { get; set; }
    
    public PaginatedList(int page, int size, int count, IEnumerable<T> data)
    {
        Page = page;
        Size = size;
        TotalPages = (int) Math.Ceiling(count / (double) size);
        TotalElements = count;
        Data = data;
    }
    
    public static async Task<PaginatedList<T>> ToPageList(IQueryable<T> source, int page, int size)
    {
        var count = await source.CountAsync();
        var data = await source.Skip((page - 1) * size).Take(size).ToListAsync();
        return new PaginatedList<T>(page, size, count, data);
    }
}