using MongoDB.Bson;

namespace ModerApiTest.Models.Responses
{
    public record RegisterResponseModel(string message, string id, UserModel user);
}
