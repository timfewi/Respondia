using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Respondia.Core.Attributes;
using Respondia.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Respondia.DataAccess.Data;

namespace Respondia.DataAccess.Repositories
{
    /// <inheritdoc />
    [Injectable(ServiceLifetime.Scoped)]
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;
        private readonly ILogger<UnitOfWork> _logger;

        // DI's
        public ICampaignRepository Campaigns { get; }
        public IFeedbackRepository Feedbacks { get; }
        public UnitOfWork(
            AppDbContext context,
            ILogger<UnitOfWork> logger,
            // Repositories
            ICampaignRepository campaigns,
            IFeedbackRepository feedbacks
            )
        {
            _context = context;
            _logger = logger;
            // Repos
            Campaigns = campaigns;
            Feedbacks = feedbacks;
        }


        /// <inheritdoc />
        public async Task BeginTransactionAsync()
        {
            if (_transaction == null)
            {
                _logger.LogInformation("Beginning a new transaction.");
                _transaction = await _context.Database.BeginTransactionAsync();
            }
        }

        /// <inheritdoc />
        public async Task CommitTransactionAsync()
        {
            await EnsureTransactionAsync();

            try
            {
                await _context.SaveChangesAsync();
                await _transaction?.CommitAsync()!;
                _logger.LogInformation("Transaction committed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while committing the transaction.");
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null!;
                }
            }
        }

        private async Task EnsureTransactionAsync()
        {
            if (_transaction == null)
            {
                _logger.LogInformation("No active transaction. Starting a new transaction.");
                _transaction = await _context.Database.BeginTransactionAsync();
            }
        }

        /// <inheritdoc />
        public async Task<int> CompleteAsync()
        {
            _logger.LogInformation("Saving changes to the database.");
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            _transaction?.Dispose();
        }

        /// <inheritdoc />
        public async Task ExecuteSqlCommandAsync(string sql, params object[] parameters)
        {
            _logger.LogInformation("Executing raw SQL command.");
            await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        /// <inheritdoc />
        public async Task RollbackTransactionAsync()
        {
            if (_transaction == null)
            {
                _logger.LogWarning("No active transaction to rollback.");
                return;
            }

            try
            {
                await _transaction.RollbackAsync();
                _logger.LogInformation("Transaction rolled back.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while rolling back the transaction.");
                throw;
            }
            finally
            {
                Dispose();
            }
        }


        /// <inheritdoc />
        public IQueryable<T> Query<T>() where T : class
        {
            _logger.LogInformation($"Querying {typeof(T).Name} without tracking.");
            return _context.Set<T>().AsNoTracking();
        }


        /// <inheritdoc />
        public void Attach<T>(T entity) where T : class
        {
            _context.Set<T>().Attach(entity);
        }

        /// <inheritdoc />
        public void Detach<T>(T entity) where T : class
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        /// <inheritdoc />
        public void Reset()
        {
            foreach (var entry in _context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }

    }
}
