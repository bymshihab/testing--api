namespace HoteListing.Api.DTOs.Hotel;

public record GetHotelsDto(
    int Id, 
    string Name, 
    string Address, 
    string Rating, 
    int CountryId
);

