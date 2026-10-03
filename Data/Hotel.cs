using System.ComponentModel.DataAnnotations;

namespace HoteListing.Api.Data;

public class Hotel
{
    public int Id {get; set;}
    public string? Name {get; set;}
    public string Address {get; set;}
    public int CountryId {get; set;}
    public Country? Country {get; set;} // Navigation property to represent the relationship with countries

}