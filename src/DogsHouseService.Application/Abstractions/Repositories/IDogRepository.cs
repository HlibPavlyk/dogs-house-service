using DogsHouseService.Application.Dtos.Other;
using DogsHouseService.Domain.Entities;

namespace DogsHouseService.Application.Abstractions.Repositories;

public interface IDogRepository
{
    Task AddAsync(Dog dog);
    IQueryable<Dog> GetAllAsQueryable();
    IQueryable<Dog> SortDogsQueryByAttributeAsync(IQueryable<Dog> query, string attribute, string order);
    Task<PagedResponse<Dog>?> GetPagedDogsFromQueryAsync(IQueryable<Dog> query, int page, int size);
    Task<IEnumerable<Dog>?> MaterializeDogsQueryAsync(IQueryable<Dog> query);
    Task<bool> DogExistsAsync(string name);
    Task SaveChangesAsync();
}