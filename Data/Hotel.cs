namespace HoteListing.Api.Data;

public class Hotel
{
    public int Id {get; set;}
    public string? Name {get; set;} = string.Empty;
    public string Address {get; set;} = string.Empty;
    public string Rating {get; set;} = string.Empty;

    public int CountryId {get; set;}
    public Country? Country {get; set;} // Navigation property to represent the relationship with countries

}