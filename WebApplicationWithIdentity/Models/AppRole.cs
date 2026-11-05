using Microsoft.AspNetCore.Identity;

namespace WebApplicationWithIdentity.Models
{
    public class AppRole: IdentityRole
    {

        
        public string Description { get; set; } = string.Empty;

        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } 

    }
}
