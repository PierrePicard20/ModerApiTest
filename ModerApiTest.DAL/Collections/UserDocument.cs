using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ModerApiTest.DAL.Collections
{
    /// <summary>
    /// Class UserDocument defines the users documents structure.
    /// </summary>
    public class UserDocument
    {
        /*
         * Public elements
         */

        [BsonId]
        public ObjectId UserId { get; set; }

        [BsonElement("email"), BsonRequired]
        public string Email { get; set; }

        [BsonElement("password"), BsonRequired]
        public string Password { get; set; }

        [BsonElement("first_name"), BsonRequired]
        public string FirstName { get; set; }

        [BsonElement("last_name"), BsonRequired]
        public string LastName { get; set; }
    }
}
