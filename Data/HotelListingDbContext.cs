using HoteListing.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Api.Data;

public class HotelListingDbContext : DbContext // Represents the database context for the Hotel Listing application. It inherits from the DbContext class provided by Entity Framework Core, which is an Object-Relational Mapping (ORM) framework for .NET applications. The DbContext class provides methods and properties to interact with the database, such as querying and saving data.
{
    public HotelListingDbContext(DbContextOptions<HotelListingDbContext> options) : base(options) // Constructor that takes DbContextOptions and passes it to the base DbContext class. This allows configuration of the database context, such as specifying the database provider and connection string.
    {
        
    }

    public DbSet<Country> Countries { get; set; } // DbSet for the Country entity. DbSet represents a collection of entities of a specific type that can be queried from the database. In this case, it represents the collection of Country entities in the database.
    public DbSet<Hotel> Hotels { get; set; }
}
