using System.ComponentModel.DataAnnotations;

namespace HoteListing.Api.DTOs.Hotel;

public class UpdateHotelDto : CreateHotelDto
{
     [Required]
    public int Id { get; set; }
}

