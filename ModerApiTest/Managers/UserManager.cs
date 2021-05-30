using ModerApiTest.DAL;
using ModerApiTest.DAL.Collections;
using ModerApiTest.DAL.Services;
using ModerApiTest.Models;
using ModerApiTest.Models.Responses;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Linq;

namespace ModerApiTest.Managers
{
    public class UserManager : IUserManager
    {
        private ICollectionService<UserDocument> _userService;
        private ICollectionService<ArticleDocument> _articleService;
        private IDatabaseContext _dbContext;

        /// <summary>
        /// Constructor called only by the framework
        /// </summary>
        /// <param name="userService">built by dependency injection</param>
        /// <param name="articleService">built by dependency injection</param>
        /// <param name="dbContext">built by dependency injection</param>
        public UserManager( ICollectionService<UserDocument> userService
                          , ICollectionService<ArticleDocument> articleService
                          , IDatabaseContext dbContext)
        {
            _userService = userService;
            _articleService = articleService;
            _dbContext = dbContext;
        }

        /// <summary>
        /// GetUserInfo returns all information of a specific user
        /// </summary>
        /// <param name="userId">the user id</param>
        /// <returns>the user document</returns>
        public UserDocument GetUserInfo(string userId)
        {
            ObjectId id;
            UserDocument userDocument;
            if (ObjectId.TryParse(userId, out id) &&
                (userDocument = _userService.FirstOrDefault(x => x.UserId == id)) != null)
            {
                return userDocument;
            }
            return null;
        }

        /// <summary>
        /// GetUserArticles returns all articles owned by a specific user
        /// </summary>
        /// <param name="userId">the user id</param>
        /// <returns>the article documents</returns>
        public IEnumerable<ArticleDocument> GetUserArticles(string userId)
        {
            ObjectId id;
            if (ObjectId.TryParse(userId, out id))
            {
                return GetUserArticles(id);
            }
            return Enumerable.Empty<ArticleDocument>();
        }

        /// <summary>
        /// GetUserArticles returns all articles owned by a specific user
        /// </summary>
        /// <param name="userId">the user id</param>
        /// <returns>the article documents</returns>
        public IEnumerable<ArticleDocument> GetUserArticles(ObjectId userId)
        {
            return _articleService.FindAll(x => x.UserId == userId);
        }

        /// <summary>
        /// UpdateUser updates a user document
        /// </summary>
        /// <param name="userId">the user id</param>
        /// <param name="user">the user informations</param>
        /// <returns>the new user document</returns>
        public UserDocument UpdateUser(string userId, UserModel user)
        {
            ObjectId id;
            if (ObjectId.TryParse(userId, out id))
            {
                var userDocument = user.ToDocument();
                userDocument.UserId = id;
                if (_userService.Update(userDocument))
                {
                    return userDocument;
                }
            }
            return null;
        }

        /// <summary>
        /// FindUser finds a specific user
        /// </summary>
        /// <param name="userId">the user id</param>
        /// <returns>the user document</returns>
        public UserDocument FindUser(string userId)
        {
            ObjectId id;
            UserDocument userDocument;
            if (ObjectId.TryParse(userId, out id) &&
               (userDocument = _userService.FirstOrDefault(x => x.UserId == id)) != null)
            {
                return userDocument;
            }
            return null;
        }

        /// <summary>
        /// DeleteUser deletes a user document
        /// </summary>
        /// <param name="user">the document to be deleted</param>
        /// <returns>true when the operation succeeds, false otherwise</returns>
        public bool DeleteUser(UserDocument user)
        {
            /* Stand alone install do not support transaction
            return _dbContext.RunTransaction(_ => {
                                                // delete of the user and all of its articles
                                                return (_userService.Remove(x => x.UserId == user.UserId) != null) &&
                                                       (_articleService.Remove(x => x.UserId == user.UserId) != null);
                                            }).GetAwaiter().GetResult();
            */

            // then we do it wihtout transaction:
            return (_userService.Remove(x => x.UserId == user.UserId) != null) &&
                   (_articleService.Remove(x => x.UserId == user.UserId) != null);
        }

        /// <summary>
        /// GetUserResponse builds the response about an operation on the user collection
        /// </summary>
        /// <param name="message">the message field content</param>
        /// <param name="user">the user document</param>
        /// <returns>the response</returns>
        public UserResponseModel GetUserResponse(string message, UserDocument user)
        {
            var articles = GetUserArticles(user.UserId);
            return new UserResponseModel( message: message
                                        , id: user.UserId.ToString()
                                        , user: user.ToModel()
                                        , articles: articles.Select(x => {
                                            return new
                                            {
                                                id = x.ArticleId.ToString(),
                                                title = x.Title,
                                                description = x.Description,
                                                userId = x.UserId.ToString()
                                            };}).ToArray());
        }
    }
}
