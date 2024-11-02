using DogsHouseService.Application.Abstractions.Repositories;
using DogsHouseService.Application.Abstractions.Services;
using DogsHouseService.Application.Dtos.Dog;
using DogsHouseService.Application.Dtos.Other;
using DogsHouseService.Domain.Entities;
using DogsHouseService.Domain.Exceptions;

namespace DogsHouseService.Application.Services;

public class DogService : IDogService
{
    private readonly IDogRepository _dogRepository;

    public DogService(IDogRepository dogRepository)
    {
        _dogRepository = dogRepository;
    }

    public string PingDogService()
    {
        return "Dogshouseservice.Version1.0.1";
    }

    public async Task<object> GetAllDogsAsync(SortByItemQueryDto? sortByItemQueryDto, PageQueryDto? pageQueryDto)
    {
        var query = _dogRepository.GetAllAsQueryable();

        if (sortByItemQueryDto != null)
            query = _dogRepository.SortDogsQueryByAttributeAsync(query, sortByItemQueryDto.Attribute, sortByItemQueryDto.Order);

        var response = await GetResponseFromQueryAsync(query, pageQueryDto);
        if (response == null)
            throw new NotFoundException("No dogs found.");

        return response;
    }
    
    private async Task<object?> GetResponseFromQueryAsync(IQueryable<Dog> query, PageQueryDto? pageQueryDto)
    {
        if (pageQueryDto != null)
            return await _dogRepository.GetPagedDogsFromQueryAsync(query, pageQueryDto.PageNumber, pageQueryDto.PageSize);

        return await _dogRepository.MaterializeDogsQueryAsync(query);
    }

    public Task AddAsync(DogCreateDto dogDto)
    {
        throw new NotImplementedException();
    }
}