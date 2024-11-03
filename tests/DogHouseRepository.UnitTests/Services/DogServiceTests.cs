using DogsHouseService.Application.Abstractions.Repositories;
using DogsHouseService.Application.Abstractions.Services;
using DogsHouseService.Application.Dtos.Dog;
using DogsHouseService.Application.Dtos.Other;
using DogsHouseService.Application.Services;
using DogsHouseService.Domain.Entities;
using DogsHouseService.Domain.Exceptions;
using Moq;

namespace DogHouseRepository.UnitTests.Services;

public class DogServiceTests
{
    private readonly Mock<IDogRepository> _dogRepositoryMock;
    private readonly IDogService _dogService;

    public DogServiceTests()
    {
        _dogRepositoryMock = new Mock<IDogRepository>();
        _dogService = new DogService(_dogRepositoryMock.Object);
    }

    [Fact]
    public void PingDogService_ShouldReturnVersion()
    {
        var result = _dogService.PingDogService();

        Assert.Equal("Dogshouseservice.Version1.0.1", result);
    }

    [Fact]
    public async Task GetAllDogsAsync_NoDogsFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var emptyQuery = Enumerable.Empty<Dog>().AsQueryable();
        _dogRepositoryMock.Setup(repo => repo.GetAllAsQueryable()).Returns(emptyQuery);
        _dogRepositoryMock.Setup(repo => repo.MaterializeDogsQueryAsync(emptyQuery)).ReturnsAsync((IEnumerable<Dog>?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _dogService.GetAllDogsAsync(null, null));
    }


    [Fact]
    public async Task GetAllDogsAsync_WithPaging_ShouldReturnPagedResults()
    {
        // Arrange
        var dogsQuery = new[] { new Dog { Name = "Buddy" }, new Dog { Name = "Max" } }.AsQueryable();
        var pagedDogs = new PagedResponse<Dog>(10, dogsQuery.ToList());

        _dogRepositoryMock.Setup(repo => repo.GetAllAsQueryable()).Returns(dogsQuery);
        _dogRepositoryMock.Setup(repo => repo.GetPagedDogsFromQueryAsync(dogsQuery, 1, 10)).ReturnsAsync(pagedDogs);

        var pageQueryDto = new PageQueryDto( 1, 10 );

        // Act
        var result = await _dogService.GetAllDogsAsync(null, pageQueryDto);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<PagedResponse<Dog>>(result);

        var resultPagedResponse = result as PagedResponse<Dog>;
        Assert.Equal(2, resultPagedResponse?.Items.Count());
        Assert.Equal("Buddy", resultPagedResponse?.Items.First() .Name);
        Assert.Equal("Max", resultPagedResponse?.Items.Last().Name);
    }


    [Fact]
    public async Task GetAllDogsAsync_WithSorting_ShouldSortResults()
    {
        // Arrange
        var dogsQuery = new[]
        {
            new Dog { Name = "Max" },
            new Dog { Name = "Buddy" }
        }.AsQueryable();

        var sortedDogs = dogsQuery.OrderBy(d => d.Name).ToList();

        _dogRepositoryMock.Setup(repo => repo.GetAllAsQueryable()).Returns(dogsQuery);
        _dogRepositoryMock.Setup(repo => repo.SortDogsQueryByAttributeAsync(dogsQuery, "name", "asc")).Returns(sortedDogs.AsQueryable());
        _dogRepositoryMock.Setup(repo => repo.MaterializeDogsQueryAsync(It.IsAny<IQueryable<Dog>>())).ReturnsAsync(sortedDogs);

        var sortByItemQueryDto = new SortByItemQueryDto("name", "asc");

        // Act
        var result = await _dogService.GetAllDogsAsync(sortByItemQueryDto, null);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<Dog>>(result);
    
        var resultList = result as List<Dog>;
        Assert.Equal(2, resultList?.Count);
        Assert.Equal("Buddy", resultList?[0].Name);
        Assert.Equal("Max", resultList?[1].Name);
    }


    [Fact]
    public async Task AddAsync_ValidDog_ShouldAddDogSuccessfully()
    {
        // Arrange
        var dogDto = new DogCreateDto { Name = "Buddy", Color = "Brown", TailLength = 10, Weight = 20 };
        _dogRepositoryMock.Setup(repo => repo.DogExistsAsync(dogDto.Name)).ReturnsAsync(false);

        // Act
        await _dogService.AddAsync(dogDto);

        // Assert
        _dogRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Dog>(d => d.Name == dogDto.Name && d.Color == dogDto.Color)), Times.Once);
        _dogRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }
}
