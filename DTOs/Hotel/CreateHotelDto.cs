using System.ComponentModel.DataAnnotations;

namespace HoteListing.Api.DTOs.Hotel;

public class CreateHotelDto
{
    [Required]
    public string? Name { get; set; }

    [Required]
    [MaxLength(250)]
    public string Address { get; set; }

    [Required]
    [Range(1, 5)]
    public string Rating { get; set; }

    [Required]
    public int CountryId { get; set; }
}

