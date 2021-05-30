using MongoDB.Bson;

namespace ModerApiTest.Models.Responses
{
    public record ArticleResponseModel(string message, string id, ArticleModel article);
}
