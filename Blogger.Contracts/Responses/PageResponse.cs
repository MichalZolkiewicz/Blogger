namespace Blogger.Contracts.Responses;

public class PageResponse<T> : Response<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }

    public int TotalRecords { get; set; }

    public bool NextPage { get; set; }
    public bool PreviousPage { get; set; }

    public PageResponse(T data, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        Data = data;
        Succeeded = true;
    }
}
