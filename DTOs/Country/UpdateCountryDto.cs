using System.ComponentModel.DataAnnotations;

public class UpdateCountryDto : CreateCountryDto
{
    [Required]
    public int CountryId {get; set;}
}