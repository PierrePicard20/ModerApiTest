using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ModerApiTest.DAL.Collections;
using MongoDB.Driver;

namespace ModerApiTest.DAL.Services
{
    /// <summary>
    /// Class ArticleCollectionService allows access to the Article collection of the database
    /// </summary>
    public class ArticleCollectionService : ICollectionService<ArticleDocument>
    {
        private IMongoCollection<ArticleDocument> _articleCollection;

        public ArticleCollectionService(IDatabaseContext dbContext)
        {
            _articleCollection = dbContext.ArticlesCollection;
        }

        /// <summary>
        /// Add an ArticleDocument
        /// </summary>
        /// <param name="article">the document to be added</param>
        public void Add(ArticleDocument article)
        {
            _articleCollection.InsertOne(article);
        }

        /// <summary>
        /// FirstOrDefault finds the unique (or first) document satisfaying a criteria
        /// </summary>
        /// <param name="filter">the criteria to be applied</param>
        /// <returns>the ArticleDocument found or null when not found</returns>
        public ArticleDocument FirstOrDefault(Expression<Func<ArticleDocument, bool>> filter)
        {
            var find = _articleCollection.Find(filter);
            find = find.Limit(1);
            return find.FirstOrDefault();
        }

        /// <summary>
        /// FindAll fetches all the documents satisfaying the criteria passed as parameter
        /// </summary>
        /// <param name="filter">the criteria to be applied</param>
        /// <returns>an enumeration of ArticleDocument instances</returns>
        public IEnumerable<ArticleDocument> FindAll(Expression<Func<ArticleDocument, bool>> filter)
        {
            var find = _articleCollection.Find(filter);
            return find.ToEnumerable<ArticleDocument>();
        }

        /// <summary>
        /// Finds a single document and deletes it atomically
        /// </summary>
        /// <param name="filter">The filter</param>
        /// <returns>The deleted document if one was deleted</returns>
        public ArticleDocument Remove(Expression<Func<ArticleDocument, bool>> filter)
        {
            return _articleCollection.FindOneAndDelete(filter);
        }

        /// <summary>
        /// Update performs the update of the document passed as parameter
        /// </summary>
        /// <param name="article">The article to update</param>
        /// <returns>true when success, false otherwise</returns>
        public bool Update(ArticleDocument article)
        {
            var update = Builders<ArticleDocument>.Update
                .Set(m => m.Title, article.Title)
                .Set(m => m.Description, article.Description)
                .Set(m => m.UserId, article.UserId);
            return _articleCollection.FindOneAndUpdate(x => x.UserId == article.UserId, update) != null;
        }
    }
}
