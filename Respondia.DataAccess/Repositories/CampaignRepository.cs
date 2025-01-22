using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Respondia.Core.Attributes;
using Respondia.Core.Entities;
using Respondia.Core.Exceptions;
using Respondia.Core.Interfaces.Repositories;
using Respondia.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.DataAccess.Repositories
{
    [Injectable(ServiceLifetime.Scoped)]
    /// <inheritdoc/>
    public class CampaignRepository : ICampaignRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Campaign> _campaigns;
        private readonly DbSet<CampaignStatistics> _campaignStatistics;
        private readonly ILogger<CampaignRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignRepository"/> class with the specified database context and logger.
        /// </summary>
        /// <param name="context">The database context used to access the database.</param>
        /// <param name="logger">The logger used for logging operations.</param>
        public CampaignRepository(AppDbContext context, ILogger<CampaignRepository> logger)
        {
            _context = context;
            _campaigns = _context.Set<Campaign>();
            _campaignStatistics = _context.Set<CampaignStatistics>();
            _logger = logger;
        }

        #region GetAll / GetById / Find

        /// <inheritdoc/>
        public async Task<IEnumerable<Campaign>> GetAllAsync()
        {
            try
            {
                var campaigns = await _campaigns
                    .AsNoTracking()
                    .Include(c => c.Statistics)
                    .Include(c => c.FeedbackForms)
                    .Where(c => !c.IsDeleted)
                    .ToListAsync();

                return campaigns ?? new List<Campaign>();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An invalid operation occurred while fetching campaigns. Method: {Method}", nameof(GetAllAsync));
                throw new DataAccessException("An error occurred while retrieving campaigns from the database.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in {Method}: {Message}", nameof(GetAllAsync), ex.Message);
                throw new DataAccessException($"An unexpected error occurred in {nameof(GetAllAsync)}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<Campaign?> GetByIdAsync(int id)
        {
            try
            {
                var campaign = await _campaigns
                    .AsNoTracking()
                    .Include(c => c.Statistics)
                    .Include(c => c.FeedbackForms)
                    .FirstOrDefaultAsync(c => !c.IsDeleted && c.Id == id);

                if (campaign == null)
                {
                    _logger.LogWarning("Campaign with Id {Id} was not found in the database.", id);
                }
                else
                {
                    _logger.LogInformation("Successfully fetched campaign with Id {Id}.", id);
                }

                return campaign;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An invalid operation occurred while fetching campaign with Id {Id}. Method: {Method}", id, nameof(GetByIdAsync));
                throw new DataAccessException($"An error occurred while retrieving campaign with Id {id} from the database.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in {Method}: {Message}", nameof(GetByIdAsync), ex.Message);
                throw new DataAccessException($"An unexpected error occurred in {nameof(GetByIdAsync)}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Campaign>> FindAsync(Expression<Func<Campaign, bool>> predicate)
        {
            try
            {
                var campaigns = await _campaigns
                    .AsNoTracking()
                    .Include(c => c.Statistics)
                    .Include(c => c.FeedbackForms)
                    .Where(predicate)
                    .ToListAsync();

                return campaigns;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An invalid operation occurred while executing a query with a predicate. Method: {Method}", nameof(FindAsync));
                throw new DataAccessException("An error occurred while retrieving campaigns based on the provided criteria.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in {Method}: {Message}", nameof(FindAsync), ex.Message);
                throw new DataAccessException($"An unexpected error occurred in {nameof(FindAsync)}.", ex);
            }
        }

        #endregion

        #region Create / Update / Remove

        /// <inheritdoc/>
        public async Task AddAsync(Campaign entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                await _campaigns.AddAsync(entity);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error while adding a new {Entity}. Method: {Method}", nameof(Campaign), nameof(AddAsync));
                throw new DataAccessException($"Error inserting the {nameof(Campaign)} into the database.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in {Method}: {Message}", nameof(AddAsync), ex.Message);
                throw new DataAccessException($"An unexpected error occurred in {nameof(AddAsync)}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task AddCampaignStatisticsAsync(CampaignStatistics entity)
        {

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                await _campaignStatistics.AddAsync(entity);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error while adding a new {Entity}. Method: {Method}", nameof(CampaignStatistics), nameof(AddCampaignStatisticsAsync));
                throw new DataAccessException($"Error inserting the {nameof(CampaignStatistics)} into the database.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in {Method}: {Message}", nameof(AddCampaignStatisticsAsync), ex.Message);
                throw new DataAccessException($"An unexpected error occurred in {nameof(AddCampaignStatisticsAsync)}.", ex);
            }
        }

        /// <inheritdoc/>
        public void Remove(Campaign entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                // Soft Delete
                entity.IsDeleted = true;
                _campaigns.Update(entity);

                _logger.LogInformation("Campaign with Id {Id} marked as deleted.", entity.Id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error while removing {Entity}. Method: {Method}",
                    nameof(Campaign), nameof(Remove));
                throw new DataAccessException($"Error removing the {nameof(Campaign)} from the database.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in {Method}: {Message}", nameof(Remove), ex.Message);
                throw new DataAccessException($"An unexpected error occurred in {nameof(Remove)}.", ex);
            }
        }

        /// <inheritdoc/>
        public void Update(Campaign entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                _campaigns.Update(entity);

                _logger.LogInformation("Campaign with Id {Id} was updated.", entity.Id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error while updating {Entity}. Method: {Method}",
                    nameof(Campaign), nameof(Update));
                throw new DataAccessException($"Error updating the {nameof(Campaign)} in the database.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in {Method}: {Message}", nameof(Update), ex.Message);
                throw new DataAccessException($"An unexpected error occurred in {nameof(Update)}.", ex);
            }
        }

        /// <inheritdoc/>
        public void UpdateCampaignStatistics(CampaignStatistics statistics)
        {
            if (statistics == null)
                throw new ArgumentNullException(nameof(statistics));

            try
            {
                statistics.LastUpdated = DateTime.Now;
                _campaignStatistics.Update(statistics);

                _logger.LogInformation("CampaignStatistics with Id {Id} was updated.", statistics.Id);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex,
                    "Error while updating {Entity}. Method: {Method}",
                    nameof(CampaignStatistics), nameof(UpdateCampaignStatistics));
                throw new DataAccessException($"Error updating the {nameof(CampaignStatistics)} in the database.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unexpected error in {Method}: {Message}",
                    nameof(UpdateCampaignStatistics), ex.Message);
                throw new DataAccessException($"An unexpected error occurred in {nameof(UpdateCampaignStatistics)}.", ex);
            }
        }

        #endregion

        #region Specific Methods

        /// <inheritdoc/>
        public async Task<Campaign> GetCampaignWithStatisticsAsync(int campaignId)
        {
            try
            {
                var campaign = await _campaigns
                    .AsNoTracking()
                    .Include(c => c.Statistics)
                    .Include(c => c.FeedbackForms)
                        .ThenInclude(f => f.FeedbackSummery)
                    .FirstOrDefaultAsync(c => c.Id == campaignId && !c.IsDeleted);

                if (campaign == null)
                {
                    _logger.LogWarning("No campaign with Id {Id} found for statistics loading.", campaignId);
                    throw new DataAccessException($"Campaign with Id {campaignId} not found.");
                }

                _logger.LogInformation("Successfully fetched campaign with statistics for Id {Id}.", campaignId);
                return campaign;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex,
                    "Invalid operation while fetching campaign with statistics. Id: {Id}, Method: {Method}",
                    campaignId, nameof(GetCampaignWithStatisticsAsync));
                throw new DataAccessException($"Error while retrieving campaign with Id {campaignId} and its statistics.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unexpected error in {Method}: {Message}",
                    nameof(GetCampaignWithStatisticsAsync), ex.Message);
                throw new DataAccessException($"An unexpected error occurred in {nameof(GetCampaignWithStatisticsAsync)}.", ex);
            }
        }
        #endregion
    }
}


