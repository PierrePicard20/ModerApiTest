using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ModerApiTest.DAL.Services
{
    /// <summary>
    /// Interface ICollectionService<T> defines the way to access a document type 'T' collection.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICollectionService<T>
    {
        /// <summary>
        /// FirstOrDefault finds the unique (or first) document satisfaying a criteria
        /// </summary>
        /// <param name="filter">the criteria to be applied</param>
        /// <returns>the document found or null when not found</returns>
        T FirstOrDefault(Expression<Func<T, bool>> filter);

        /// <summary>
        /// FindAll fetches all the documents satisfaying the criteria passed as parameter
        /// </summary>
        /// <param name="filter">the criteria to be applied</param>
        /// <returns>an enumeration of documents of type 'T'</returns>
        IEnumerable<T> FindAll(Expression<Func<T, bool>> filter);

        /// <summary>
        /// Add a document of type 'T'
        /// </summary>
        /// <param name="document">the document to be added</param>
        void Add(T document);

        /// <summary>
        /// Update performs the update of the document passed as parameter
        /// </summary>
        /// <param name="article">The article to update</param>
        /// <returns>true when success, false otherwise</returns>
        bool Update(T document);

        /// <summary>
        /// Finds a single document and deletes it atomically
        /// </summary>
        /// <param name="filter">The filter</param>
        /// <returns>The deleted document if one was deleted</returns>
        T Remove(Expression<Func<T, bool>> filter);
    }
}
