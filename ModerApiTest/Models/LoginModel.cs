using System.ComponentModel.DataAnnotations;

namespace ModerApiTest.Models
{
    public record LoginModel( [Required][EmailAddress] string email
                            , [Required][StringLength(30, MinimumLength = 8)] string password);
}
