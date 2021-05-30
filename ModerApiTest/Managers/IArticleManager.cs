using ModerApiTest.DAL.Collections;
using ModerApiTest.Models;
using ModerApiTest.Models.Responses;

namespace ModerApiTest.Managers
{
    public interface IArticleManager
    {
        ArticleDocument FindArticle(string articleId);

        ArticleDocument FindArticleByUserAndTitle(string userId, ArticleModel article);

        ArticleDocument InsertArticle(string userId, ArticleModel article);

        ArticleDocument UpdateArticle(string userId, string articleId, ArticleModel article);

        ArticleDocument DeleteArticle(string articleId, string userId);

        ArticleResponseModel GetArticleResponse(string message, ArticleDocument article);

    }
}
