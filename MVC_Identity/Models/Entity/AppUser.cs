using Microsoft.AspNetCore.Identity;

namespace MVC_Identity.Models.Entity
{
    public class AppUser:IdentityUser
    {
        public string? Address { get; set; }
        public DateTime? Birthdate { get; set; }

    }
}
