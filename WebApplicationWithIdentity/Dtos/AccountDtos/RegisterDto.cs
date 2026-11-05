using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace WebApplicationWithIdentity.Dtos.AccountDtos
{
    public class RegisterDto
    {
        public string Username { get; set; }=string.Empty;

        public string firstName { get; set; }=string.Empty;


        public string lastName { get; set; } = string.Empty;


        public string Email { get; set; } = string.Empty;


        public string Password { get; set; } = string.Empty;
    }
}
