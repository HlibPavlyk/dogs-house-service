namespace DogsHouseService.Application.Dtos.Other;

public class PagedResponse<T> (int TotalPages, IEnumerable<T> Items);