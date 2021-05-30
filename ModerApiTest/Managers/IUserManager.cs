using ModerApiTest.DAL.Collections;
using ModerApiTest.Models;
using ModerApiTest.Models.Responses;
using System.Collections.Generic;

namespace ModerApiTest.Managers
{
    public interface IUserManager
    {
        UserDocument GetUserInfo(string userId);

        IEnumerable<ArticleDocument> GetUserArticles(string userId);

        UserDocument UpdateUser(string userId, UserModel user);

        UserDocument FindUser(string userId);

        bool DeleteUser(UserDocument user);

        UserResponseModel GetUserResponse(string message, UserDocument user);
    }
}
