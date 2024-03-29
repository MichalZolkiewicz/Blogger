using WebAPI.Wrapper;

namespace WebAPI.Filters.Helpers;

public class PaginationHelper
{
    public static PageResponse<IEnumerable<T>> CreatePageResponse<T>(IEnumerable<T> pageData, PaginationFilter validPaginationFilter, int totalRecords)
    {
        var response = new PageResponse<IEnumerable<T>>(pageData, validPaginationFilter.PageNumber, validPaginationFilter.PageSize);
        var totalPages = ((double)totalRecords / validPaginationFilter.PageSize);
        var roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
        var currentPage = validPaginationFilter.PageNumber;

        response.TotalPages = roundedTotalPages;
        response.TotalRecords = totalRecords;
        response.PreviousPage = currentPage > 1 ? true : false;
        response.NextPage = currentPage < roundedTotalPages ? true : false;

        return response;
    }
}
