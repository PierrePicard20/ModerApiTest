using ModerApiTest.DAL.Collections;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace ModerApiTest.DAL
{
    /// <summary>
    /// Interface IDatabaseContext defines the accessible collections of the database.
    /// </summary>
    public interface IDatabaseContext
    {
        IMongoCollection<UserDocument> UserCollection { get; }
        IMongoCollection<ArticleDocument> ArticlesCollection { get; }

        Task<bool> RunTransaction(Func<IClientSessionHandle, bool> action);
    }
}
