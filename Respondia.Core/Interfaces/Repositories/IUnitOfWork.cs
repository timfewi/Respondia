using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.Core.Interfaces.Repositories
{
    /// <summary>
    /// Represents a unit of work that encapsulates a set of operations to be executed as a single transaction.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        // DI's
        ICampaignRepository Campaigns { get; }
        IFeedbackRepository Feedbacks { get; }
       
        /// <summary>
        /// Begins a new transaction asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task BeginTransactionAsync();

        /// <summary>
        /// Commits the current transaction asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task CommitTransactionAsync();

        /// <summary>
        /// Rolls back the current transaction asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RollbackTransactionAsync();

        /// <summary>
        /// Saves all changes made in the context to the database asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of state entries written to the database.</returns>
        Task<int> CompleteAsync();

        /// <summary>
        /// Executes the given SQL command asynchronously.
        /// </summary>
        /// <param name="sql">The SQL command string.</param>
        /// <param name="parameters">The parameters to apply to the SQL command string.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ExecuteSqlCommandAsync(string sql, params object[] parameters);

        /// <summary>
        /// Returns an IQueryable for the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the query.</typeparam>
        /// <returns>An IQueryable for the specified type.</returns>
        IQueryable<T> Query<T>() where T : class;

        /// <summary>
        /// Attaches the specified entity to the context.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity to attach.</param>
        void Attach<T>(T entity) where T : class;

        /// <summary>
        /// Detaches the specified entity from the context.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity to detach.</param>
        void Detach<T>(T entity) where T : class;

        /// <summary>
        /// Resets the unit of work, clearing any tracked changes.
        /// </summary>
        void Reset();
    }
}
