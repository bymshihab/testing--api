using System.ComponentModel.DataAnnotations;

public class CreateCountryDto
{
    [Required]
    [MaxLength(100)]
    public string? Name {get; set;}
    [Required]
    [MaxLength(5)]
    public string? ShortName {get; set;}
}
