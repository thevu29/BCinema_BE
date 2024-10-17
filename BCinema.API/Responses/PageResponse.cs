namespace BCinema.API.Responses;

public class PageResponse<T> : ApiResponse<T>
{
    public int Page { get; set; }
    public int Size { get; set; }
    public int TotalPages { get; set; }
    public int TotalElements { get; set; }
    
    public PageResponse(bool success, string message, T data, int page, int size, int totalPage, int totalElements) : base(success, message, data)
    {
        Page = page;
        Size = size;
        TotalPages = totalPage;
        TotalElements = totalElements;
    }
}