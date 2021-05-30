using ModerApiTest.DAL.Services;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace ModerApiTest.DAL.Collections
{
    /// <summary>
    /// Class ArticleDocument defines the articles documents structure.
    /// </summary>
    public class ArticleDocument
    {
        /*
         * Public elements
         */

        [BsonId]
        public ObjectId ArticleId { get; set; }

        [BsonElement("title"), BsonRequired]
        public string Title { get; set; }

        [BsonElement("description"), BsonRequired]
        public string Description { get; set; }

        [BsonElement("user_id"), BsonRequired]
        public ObjectId UserId
        {
            get
            {
                return _userId;
            }
            set
            {
                if (_userId != value)
                {
                    _userId = value;
                    _user = null;
                }
            }
        }

        public UserDocument GetUser(ICollectionService<UserDocument> userService)
        {
            if (_user == null)
            {
                _user = userService.FirstOrDefault(x => x.UserId == UserId);
            }
            return _user;
        }

        public void SetUser(UserDocument user)
        {
            _user = user;
            UserId = user != null?user.UserId:ObjectId.Empty;
        }

        /*
         * Private elements
         */

        [BsonIgnore]
        private ObjectId _userId;

        [BsonIgnore]
        private UserDocument _user;
    }
}
