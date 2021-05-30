using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ModerApiTest.Models;
using ModerApiTest.Utils;
using ModerApiTest.Managers;

namespace ModerApiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private IArticleManager _articleManager;

        /// <summary>
        /// Constructor called only by the framework
        /// </summary>
        /// <param name="articleManager">built by dependency injection</param>
        public ArticleController(IArticleManager articleManager)
        {
            _articleManager = articleManager;
        }

        /// <summary>
        /// GetArticleInfo http handler for endpoint GET 'api/article/{id}'
        /// </summary>
        /// <param name="id">the article id</param>
        /// <returns>the response</returns>
        [HttpGet("{id}")]
        public IActionResult GetArticleInfo(string id)
        {
            var articleDocument = _articleManager.FindArticle(id);
            if (articleDocument == null)
            {
                return NotFound(new { message = "Article not found" });
            }
            else
            {
                return Ok(_articleManager.GetArticleResponse("Article found", articleDocument));
            }
        }

        /// <summary>
        /// InsertArticle http handler for endpoint POST 'api/article'
        /// </summary>
        /// <param name="articleModel">the article informations</param>
        /// <returns>the response</returns>
        [Authorize]
        [HttpPost]
        public IActionResult InsertArticle([FromBody] ArticleModel articleModel)
        {
            var currentUserId = HttpContext.GetCurrentUserId();
            var articleDocument = _articleManager.FindArticleByUserAndTitle(currentUserId, articleModel);
            if (articleDocument == null)
            {
                articleDocument = _articleManager.InsertArticle(currentUserId, articleModel);
                return Ok(_articleManager.GetArticleResponse("Article added", articleDocument));
            }
            else
            {
                return BadRequest(new { message = "The current user already owns that article" } );
            }
        }

        /// <summary>
        /// UpdateArticle http handler for endpoint PUT 'api/article/{id}'
        /// </summary>
        /// <param name="id">the article id</param>
        /// <param name="articleModel">the article informations</param>
        /// <returns>the response</returns>
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdateArticle(string id, [FromBody] ArticleModel articleModel)
        {
            var articleDocument = _articleManager.UpdateArticle(HttpContext.GetCurrentUserId(), id, articleModel);
            if (articleDocument == null)
            {
                return NotFound(new { message = "Article not found" });
            }
            else
            {
                return Ok(_articleManager.GetArticleResponse("Article updated", articleDocument));
            }
        }

        /// <summary>
        /// DeleteArticle http handler for endpoint DELETE 'api/article/{id}'
        /// </summary>
        /// <param name="id">the article id</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteArticle(string id)
        {
            var userId = HttpContext.GetCurrentUserId();
            var articleDocument = _articleManager.DeleteArticle(id, userId);
            if (articleDocument == null)
            {
                return NotFound(new { message = "Article not found" });
            }
            else
            {
                return Ok(_articleManager.GetArticleResponse("Article deleted", articleDocument));
            }
        }
    }
}
