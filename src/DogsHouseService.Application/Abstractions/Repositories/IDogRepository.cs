using DogsHouseService.Application.Dtos.Other;
using DogsHouseService.Domain.Entities;

namespace DogsHouseService.Application.Abstractions.Repositories;

public interface IDogRepository
{
    Task AddAsync(Dog dog);
    Task<object?> GetAllAsync(SortByItemQueryDto? sortByItemQueryDto, PageQueryDto? pageQueryDto);
}