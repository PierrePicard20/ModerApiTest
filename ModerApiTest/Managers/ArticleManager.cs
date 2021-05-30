using ModerApiTest.DAL.Collections;
using ModerApiTest.DAL.Services;
using ModerApiTest.Models;
using ModerApiTest.Models.Responses;
using MongoDB.Bson;

namespace ModerApiTest.Managers
{
    public class ArticleManager : IArticleManager
    {
        private ICollectionService<ArticleDocument> _articleService;

        /// <summary>
        /// Constructor called only by the framework
        /// </summary>
        /// <param name="articleService">built by dependency injection</param>
        public ArticleManager(ICollectionService<ArticleDocument> articleService)
        {
            _articleService = articleService;
        }

        /// <summary>
        /// FindArticle finds by article id
        /// </summary>
        /// <param name="articleId">the article id</param>
        /// <returns>the document</returns>
        public ArticleDocument FindArticle(string articleId)
        {
            ObjectId id;
            ArticleDocument articleDocument;
            if (ObjectId.TryParse(articleId, out id) &&
                ((articleDocument = _articleService.FirstOrDefault(x => x.ArticleId == id)) != null))
            {
                return articleDocument;
            }
            return null;
        }

        /// <summary>
        /// FindArticleByUserAndTitle finds by user owner and article's title
        /// </summary>
        /// <param name="userId">the user id owner</param>
        /// <param name="article">the article inforamtions</param>
        /// <returns></returns>
        public ArticleDocument FindArticleByUserAndTitle(string userId, ArticleModel article)
        {
            ObjectId id;
            ArticleDocument articleDocument;
            if (ObjectId.TryParse(userId, out id) &&
                ((articleDocument = _articleService.FirstOrDefault(x => x.UserId == id && x.Title == article.title)) != null))
            {
                return articleDocument;
            }
            return null;
        }

        /// <summary>
        /// InsertArticle 
        /// </summary>
        /// <param name="userId">the user id owner</param>
        /// <param name="article">the article informations</param>
        /// <returns>the inserted document</returns>
        public ArticleDocument InsertArticle(string userId, ArticleModel article)
        {
            article = article.WithUserId(userId);
            var articleDocument = article.ToDocument();
            _articleService.Add(articleDocument);
            return articleDocument;
        }

        /// <summary>
        /// UpdateArticle updates an article document
        /// </summary>
        /// <param name="userId">the user id owner</param>
        /// <param name="articleId">the article id</param>
        /// <param name="article">the article inforamtions</param>
        /// <returns>the new article document</returns>
        public ArticleDocument UpdateArticle(string userId, string articleId, ArticleModel article)
        {
            ObjectId id;
            if (ObjectId.TryParse(articleId, out id))
            {
                article = article.WithUserId(userId);
                var articleDocument = article.ToDocument();
                articleDocument.ArticleId = id;
                if (_articleService.Update(articleDocument))
                {
                    return articleDocument;
                }
            }
            return null;
        }

        /// <summary>
        /// DeleteArticle deleted an article document
        /// </summary>
        /// <param name="articleId">the article id</param>
        /// <returns>the deleted document</returns>
        public ArticleDocument DeleteArticle(string articleId, string userId)
        {
            ObjectId articleObjectid, userObjectId;
            if (ObjectId.TryParse(articleId, out articleObjectid) && ObjectId.TryParse(userId, out userObjectId))
            {
                return _articleService.Remove(x => x.ArticleId == articleObjectid && x.UserId == userObjectId);
            }
            return null;
        }

        /// <summary>
        /// GetArticleResponse builds the response about an operation on the article collection
        /// </summary>
        /// <param name="message">the message field content</param>
        /// <param name="article">the article document</param>
        /// <returns>the response</returns>
        public ArticleResponseModel GetArticleResponse(string message, ArticleDocument article)
        {
            return new ArticleResponseModel( message: message
                                           , id: article.ArticleId.ToString()
                                           , article: article.ToModel());
        }
    }
}
