using DogsHouseService.Application.Dtos.Dog;
using DogsHouseService.Application.Dtos.Other;

namespace DogsHouseService.Application.Abstractions.Services;

public interface IDogService
{
    string PingDogService();
    Task<object> GetAllDogsAsync(SortByItemQueryDto? sortByItemQueryDto, PageQueryDto? pageQueryDto);
    Task AddAsync(DogCreateDto dogDto);
}
