namespace BCinema.Application.Helpers;

public abstract class PaginationQuery
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    public string SortBy { get; set; } = "Id";
    public string SortOrder { get; set; } = "ASC";

    public PaginationQuery()
    {
        
    }
    
    public PaginationQuery(int page, int size, string sortBy = "Id", string sortOrder = "ASC")
    {
        Page = page;
        Size = size;
        SortBy = sortBy;
        SortOrder = sortOrder;
    }
}