using ModerApiTest.DAL.Collections;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace ModerApiTest.DAL
{
    /// <summary>
    /// Class DatabseContext handles mongodb database access.
    /// </summary>
    public class DatabaseContext : IDatabaseContext
    {
        private MongoClient _client;

        public IMongoCollection<UserDocument> _userCollection;
        public IMongoCollection<ArticleDocument> _articlesCollection;

        /// <summary>
        /// Constructor connects to the database and perform initializations
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="dbName"></param>
        public DatabaseContext(string connectionString, string dbName)
        {
            _client = new MongoClient(connectionString);
            var database = _client.GetDatabase(dbName);

            _userCollection = database.GetCollection<UserDocument>("Users");
            var emailIndex = Builders<UserDocument>.IndexKeys.Ascending(_ => _.Email);
            _userCollection.Indexes.CreateOneAsync(new CreateIndexModel<UserDocument>(emailIndex)).GetAwaiter().GetResult();

            _articlesCollection = database.GetCollection<ArticleDocument>("Articles");
            var userIdIndex = Builders<ArticleDocument>.IndexKeys.Ascending(_ => _.UserId);
            _articlesCollection.Indexes.CreateOneAsync(new CreateIndexModel<ArticleDocument>(userIdIndex)).GetAwaiter().GetResult();
        }

        /*
         * Properties for collections access
         */

        public IMongoCollection<UserDocument> UserCollection => _userCollection;
        public IMongoCollection<ArticleDocument> ArticlesCollection => _articlesCollection;

        /// <summary>
        /// RunTransaction allows to run ACID transactions
        /// </summary>
        /// <param name="action">contains the code that performs updates</param>
        /// <returns>true when transaction was commited, false when it was rollbacked</returns>
        public async Task<bool> RunTransaction(Func<IClientSessionHandle,bool> action)
        {
            using (var session = await _client.StartSessionAsync())
            {
                session.StartTransaction();
                try
                {
                    if (action(session))
                    {
                        await session.CommitTransactionAsync();
                        return true;
                    }
                    else
                    {
                        await session.AbortTransactionAsync();
                        return false;
                    }
                }
                catch
                {
                    await session.AbortTransactionAsync();
                    return false;
                }
            }
        }
    }
}
