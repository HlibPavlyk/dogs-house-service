using DogsHouseService.Application.Abstractions.Services;
using DogsHouseService.Application.Dtos.Dog;
using DogsHouseService.Application.Dtos.Other;
using DogsHouseService.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DogsHouseService.Api.Controllers;

[ApiController]
public class DogController : Controller
{
    private readonly IDogService _dogService;

    public DogController(IDogService dogService)
    {
        _dogService = dogService;
    }
    
    [HttpGet]
    [Route("ping")]
    public string PingDogService()
    {
        return _dogService.PingDogService();
    }
    
    [HttpGet]
    [Route("dogs")]
    public async Task<object> GetAllDogsAsync([FromQuery] int? pageNumber, [FromQuery] int? pageSize, 
        [FromQuery] string? attribute, [FromQuery] string? order)
    {
        try
        {
            var pageQueryDto = (pageNumber == null || pageSize == null) ? null : new PageQueryDto(pageNumber.Value, pageSize.Value);
            var sortByItemQueryDto = (attribute == null || order == null) ? null : new SortByItemQueryDto(attribute, order);
            
            var dogs = await _dogService.GetAllDogsAsync(sortByItemQueryDto, pageQueryDto);
            return Ok(dogs);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost]
    [Route("dog")]
    public Task AddAsync([FromBody] DogCreateDto dogDto)
    {
        return _dogService.AddAsync(dogDto);
    }
}