using Microsoft.AspNetCore.Identity;

namespace HotelListing.Api.Data;

public class ApplicationUser : IdentityUser // Represents a user in the application. It inherits from the IdentityUser class provided by ASP.NET Core Identity, which is a membership system that adds login functionality to applications. The IdentityUser class provides properties for user information such as username, email, and password hash.
{
    public string FirstName { get; set; } = string.Empty; // Property to store the user's first name.
    public string LastName { get; set; } = string.Empty; // Property to store the user's last name.
}

 