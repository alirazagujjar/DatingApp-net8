using System.Text.Json;
using API.Helpers;

namespace API.Extensions;
public static class HttpExtensions
{
    public static void AddPaginationHeader<T>(this HttpResponse response, PageList<T> data)
    {
        var paginationsHeader = new PaginationHeader(data.CurrentPage, data.PageSize,
         data.TotalCount, data.TotalePage);
        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        response.Headers.Append("Pagination", JsonSerializer.Serialize(paginationsHeader, jsonOptions));
        response.Headers.Append("Access-Control-Expose-Headers", "Pagination");
    }

}