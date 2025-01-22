using Respondia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.Core.Interfaces.Repositories
{
    /// <summary>
    /// Represents a generic repository for managing entities of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the entity managed by the repository.</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Retrieves all entities from the database that are not soft deleted.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of all entities.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Finds entities that satisfy the specified condition.
        /// </summary>
        /// <param name="predicate">An expression to test each entity for a condition.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of entities that satisfy the condition.</returns>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Retrieves an entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains the entity if found; otherwise, null.
        /// </returns>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new entity to the database.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the entity is null.</exception>
        Task AddAsync(T entity);

        /// <summary>
        /// Marks an entity as deleted in the database (soft delete).
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        /// <exception cref="ArgumentNullException">Thrown when the entity is null.</exception>
        void Remove(T entity);

        /// <summary>
        /// Updates an existing entity in the database.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <exception cref="ArgumentNullException">Thrown when the entity is null.</exception>
        void Update(T entity);
    }
}
