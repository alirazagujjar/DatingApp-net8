using Microsoft.EntityFrameworkCore;

namespace API.Helpers;

public class PageList<T> : List<T>
{
    public PageList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
    {
        CurrentPage = pageNumber;
        PageSize = pageSize;
        TotalePage = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
        AddRange(items);
    }
    public int CurrentPage { get; set; }
    public int TotalePage { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public static async Task<PageList<T>> CreateAsynce(IQueryable<T> source, int pageNumbr, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageNumbr - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PageList<T>(items, count, pageNumbr, pageSize);
    }
}