using Microsoft.AspNetCore.Identity;

namespace WebApplicationWithIdentity.Models
{
    public class AppUser:IdentityUser
    {
        public string FirstName { get; set; }=string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string displayName { get; set; } = string.Empty;


    }
}
