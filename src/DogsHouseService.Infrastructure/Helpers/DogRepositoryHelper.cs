using DogsHouseService.Application.Dtos.Other;
using DogsHouseService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DogsHouseService.Infrastructure.Helpers;

public static class DogRepositoryHelper
{
    public static async Task<PagedResponse<T>?> GetPagedEntitiesAsync<T>(IQueryable<T> query,
        int page, int size) where T : class
    {
        var totalItems = await query.CountAsync();
        if (totalItems == 0)
            return null;

        var items = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();

        var totalPages = (int)Math.Ceiling(totalItems / (double)size);

        return new PagedResponse<T>(totalPages, items);
    }
    
    public static IQueryable<Dog> SortDogsByAttributeAsync(IQueryable<Dog> query,
        string attribute, string order)
    {
        var isAscending = order switch
        {
            "asc" => true,
            "desc" => false,
            _ => throw new ArgumentException("Invalid order.")
        };
        
        query = attribute switch
        {
            "name" => isAscending
                ? query.OrderBy(x => x.Name)
                : query.OrderByDescending(x => x.Name),
            "color" => isAscending
                ? query.OrderBy(x => x.Color)
                : query.OrderByDescending(x => x.Color),
            "tail_length" => isAscending
                ? query.OrderBy(x => x.TailLength)
                : query.OrderByDescending(x => x.TailLength),
            "weight" => isAscending
                ? query.OrderBy(x => x.Weight)
                : query.OrderByDescending(x => x.Weight),
            _ => throw new ArgumentException("Invalid attribute.")
        };
        
        return query;
    }
}