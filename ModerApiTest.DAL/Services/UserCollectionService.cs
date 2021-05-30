using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ModerApiTest.DAL.Collections;
using MongoDB.Driver;

namespace ModerApiTest.DAL.Services
{
    /// <summary>
    /// Class UserCollectionService allows access to the Article collection of the database
    /// </summary>
    public class UserCollectionService : ICollectionService<UserDocument>
    {
        IMongoCollection<UserDocument> _userCollection;

        public UserCollectionService(IDatabaseContext dbContext)
        {
            _userCollection = dbContext.UserCollection;
        }

        /// <summary>
        /// Add an UserDocument
        /// </summary>
        /// <param name="user">the document to be added</param>
        public void Add(UserDocument user)
        {
            _userCollection.InsertOne(user);
        }

        /// <summary>
        /// FirstOrDefault finds the unique (or first) document satisfaying a criteria
        /// </summary>
        /// <param name="filter">the criteria to be applied</param>
        /// <returns>the UserDocument found or null when not found</returns>
        public UserDocument FirstOrDefault(Expression<Func<UserDocument,bool>> filter)
        {
            var find = _userCollection.Find(filter);
            find = find.Limit(1);
            return find.FirstOrDefault();
        }

        /// <summary>
        /// FindAll fetches all the documents satisfaying the criteria passed as parameter
        /// </summary>
        /// <param name="filter">the criteria to be applied</param>
        /// <returns>an enumeration of UserDocument instances</returns>
        public IEnumerable<UserDocument> FindAll(Expression<Func<UserDocument, bool>> filter)
        {
            var find = _userCollection.Find(filter);
            return find.ToEnumerable<UserDocument>();
        }

        /// <summary>
        /// Finds a single document and deletes it atomically
        /// </summary>
        /// <param name="filter">The filter</param>
        /// <returns>The deleted document if one was deleted</returns>
        public UserDocument Remove(Expression<Func<UserDocument, bool>> filter)
        {
            return _userCollection.FindOneAndDelete(filter);
        }

        /// <summary>
        /// Update performs the update of the document passed as parameter
        /// </summary>
        /// <param name="user">The article to update</param>
        /// <returns>true when success, false otherwise</returns>
        public bool Update(UserDocument user)
        {
            var update = Builders<UserDocument>.Update
                .Set(m => m.Email, user.Email)
                .Set(m => m.Password, user.Password)
                .Set(m => m.FirstName, user.FirstName)
                .Set(m => m.LastName, user.LastName);
            return _userCollection.FindOneAndUpdate(x => x.UserId == user.UserId, update) != null;
        }
    }
}
