using System.ComponentModel.DataAnnotations;
using ModerApiTest.DAL.Collections;
using MongoDB.Bson;

namespace ModerApiTest.Models
{
    public record ArticleModel( [Required] string title
                              , [Required] string description
                              , string user_id)
    {
        public ArticleDocument ToDocument()
        {
            var doc = new ArticleDocument()
            {
                Title = title,
                Description = description
            };
            ObjectId id;
            if (ObjectId.TryParse(user_id, out id))
            {
                doc.UserId = id;
            }
            else
            {
                doc.UserId = default(ObjectId);
            }
            return doc;
        }

        public ArticleModel WithUserId(string userId)
        {
            return new ArticleModel(title, description, userId);
        }
    }
}
