namespace ModerApiTest.Models.Responses
{
    public record UserResponseModel(string message, string id, UserModel user, object[] articles);
}
