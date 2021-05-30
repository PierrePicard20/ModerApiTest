namespace ModerApiTest.Models.Responses
{
    public record LoginResponseModel(string message, string token, System.DateTime? expirationUtcDate);
}
