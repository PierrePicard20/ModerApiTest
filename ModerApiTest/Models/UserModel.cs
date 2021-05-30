using ModerApiTest.DAL.Collections;
using System.ComponentModel.DataAnnotations;

namespace ModerApiTest.Models
{
    public record UserModel( [Required][EmailAddress] string email
                           , [Required][StringLength(30, MinimumLength = 8)] string password
                           , [Required] string first_name
                           , [Required] string last_name)
    {
        public UserDocument ToDocument()
        {
            return new UserDocument()
            {
                Email = email,
                Password = BCrypt.Net.BCrypt.HashPassword(password),    // to do not store passwords in plain text
                FirstName = first_name,
                LastName = last_name
            };
        }
    }
}
