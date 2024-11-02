using DogsHouseService.Application.Abstractions.Repositories;
using DogsHouseService.Application.Dtos.Other;
using DogsHouseService.Domain.Entities;
using DogsHouseService.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DogsHouseService.Infrastructure.Repositories;

public class DogRepository : IDogRepository
{
    private readonly ApplicationDbContext _context;

    protected DogRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task AddAsync(Dog dog)
    {
        await _context.Dogs
            .AddAsync(dog);
    }

    public async Task<object?> GetAllAsync(SortByItemQueryDto? sortByItemQueryDto, PageQueryDto? pageQueryDto)
    {
        var query = _context.Dogs.AsNoTracking();

        if (sortByItemQueryDto != null)
            query = DogRepositoryHelper.SortDogsByAttributeAsync(query, sortByItemQueryDto.Attribute, sortByItemQueryDto.Order);

        if (pageQueryDto != null)
            return await DogRepositoryHelper.GetPagedEntitiesAsync(query, pageQueryDto.PageNumber, pageQueryDto.PageSize);
    
        return await query.ToListAsync();
    }
 
}
