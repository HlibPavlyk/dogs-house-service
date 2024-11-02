using DogsHouseService.Application.Abstractions.Repositories;
using DogsHouseService.Application.Dtos.Other;
using DogsHouseService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DogsHouseService.Infrastructure.Repositories;

public class DogRepository : IDogRepository
{
    private readonly ApplicationDbContext _context;

    public DogRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task AddAsync(Dog dog)
    {
        await _context.Dogs
            .AddAsync(dog);
    }

    public IQueryable<Dog> GetAllAsQueryable()
    {
        return _context.Dogs.
            AsNoTracking();
    }

    public IQueryable<Dog> SortDogsQueryByAttributeAsync(IQueryable<Dog> query,
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
    
    public async Task<PagedResponse<Dog>?> GetPagedDogsFromQueryAsync(IQueryable<Dog> query,
        int page, int size)
    {
        var totalItems = await query.CountAsync();
        if (totalItems == 0)
            return null;

        var items = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();
        
        if (items.Count == 0)
            return null;

        var totalPages = (int)Math.Ceiling(totalItems / (double)size);

        return new PagedResponse<Dog>(totalPages, items);
    }

    public async Task<IEnumerable<Dog>?> MaterializeDogsQueryAsync(IQueryable<Dog> query)
    {
        return await query.
            ToListAsync();
    }
    
    public async Task<bool> DogExistsAsync(string name)
    {
        return await _context.Dogs
            .AnyAsync(d => d.Name == name);
    }
    
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
