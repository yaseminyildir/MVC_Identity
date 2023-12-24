using Microsoft.AspNetCore.Identity;

namespace MVC_Identity.Areas.Administrator.Models.ViewModels
{
    public class UserListViewModel
    {
       
        public string Id { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }

        public IdentityRole Roles { get; set; }
    }
}
