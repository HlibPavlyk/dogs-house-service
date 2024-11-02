namespace DogsHouseService.Application.Dtos.Other;

/*public class PagedResponse<T> (int TotalPages, IEnumerable<T> Items);*/

public class PagedResponse<T>
{
    public int TotalPages { get; }
    public IEnumerable<T> Items { get; }

    public PagedResponse(int totalPages, IEnumerable<T> items)
    {
        TotalPages = totalPages;
        Items = items;
    }
}