using ModerApiTest.DAL.Collections;

namespace ModerApiTest.Models
{
    public static class ModelExtensions
    {
        public static UserModel ToModel(this UserDocument userDocument)
        {
            return new UserModel(userDocument.Email, string.Empty, userDocument.FirstName, userDocument.LastName);
        }

        public static ArticleModel ToModel(this ArticleDocument articleDocument)
        {
            return new ArticleModel(articleDocument.Title, articleDocument.Description, articleDocument.UserId.ToString());
        }
    }
}
