using System.ComponentModel.DataAnnotations;

namespace DogsHouseService.Application.Dtos.Dog;

public class DogCreateDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Color { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Tail length must be a non-negative integer.")]
    public int TailLength { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Weight must be a non-negative integer.")]
    public int Weight { get; set; }
}