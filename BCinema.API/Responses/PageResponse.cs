namespace BCinema.API.Responses;

public class PageResponse<T>(bool success, string message, T data, int page, int size, int totalPage, int totalElements)
    : ApiResponse<T>(success, message, data)
{
    public int Page { get; set; } = page;
    public int Size { get; set; } = size;
    public int Take { get; set; } = (data as IEnumerable<object>)?.Count() ?? 0;
    public int TotalPages { get; set; } = totalPage;
    public int TotalElements { get; set; } = totalElements;
}