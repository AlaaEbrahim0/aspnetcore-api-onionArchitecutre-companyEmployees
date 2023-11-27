using System.Runtime.CompilerServices;

namespace Shared.RequestFeatures;

public class PagedList<T>: List<T>
{
    public MetaData MetaData { get; set; }

    public PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        MetaData = new MetaData()
        {
            PageSize = pageSize,
            CurrentPage = pageNumber,
            TotalCount = count,
            TotalPages = (int)Math.Ceiling(count / (double)pageSize),
        };

        this.AddRange(items);
    }
}