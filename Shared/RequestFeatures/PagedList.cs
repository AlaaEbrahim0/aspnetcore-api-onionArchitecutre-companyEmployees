namespace Shared.RequestFeatures;

public class PagedList<T>: List<T>
{
    public MetaData MetaData { get; set; }
    public PagedList(IQueryable<T> items, int count, int pageSize, int pageNumber)
    {
        MetaData = new MetaData()
        {
            TotalPages = (int)Math.Ceiling(count / (double)pageSize),
            TotalCount = count,
            PageSize = pageSize,
            CurrentPage = pageNumber
        };
    }

    public static PagedList<T> ToPagedList(IQueryable<T> source, int pageNumber, 
        int pageSize)
    {
        var count = source.Count();
        var items = source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        return new PagedList<T>(items, count, pageSize, pageNumber);
    }
}