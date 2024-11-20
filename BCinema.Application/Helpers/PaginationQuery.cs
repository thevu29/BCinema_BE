namespace BCinema.Application.Helpers;

public abstract class PaginationQuery
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
    public string SortBy { get; set; } = "CreateAt";
    public string SortOrder { get; set; } = "DESC";

    protected PaginationQuery() {}

    protected PaginationQuery(int page, int size, string sortBy = "CreateAt", string sortOrder = "DESC")
    {
        Page = page;
        Size = size;
        SortBy = sortBy;
        SortOrder = sortOrder;
    }
}