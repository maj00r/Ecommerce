using System.ComponentModel.DataAnnotations;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

[CollectionName("Users")]
public class ApplicationUser : MongoIdentityUser<Guid>
{
}

[CollectionName("Roles")]
public class ApplicationRole : MongoIdentityRole<Guid>
{

}

public class User
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }

    public class LoginPayload
    {

        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }
