using Respondia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.Core.Interfaces.Repositories
{
    /// <summary>
    /// Provides methods for accessing and manipulating campaign data in the database.
    /// </summary>
    public interface ICampaignRepository : IRepository<Campaign>
    {
        /// <summary>
        /// Retrieves a campaign with its associated statistics and feedback summaries by campaign ID.
        /// </summary>
        /// <param name="campaignId">The unique identifier of the campaign.</param>
        /// <returns>The campaign with its statistics and feedback summaries.</returns>
        /// <exception cref="DataAccessException">Thrown when the campaign is not found or an error occurs during retrieval.</exception>
        Task<Campaign> GetCampaignWithStatisticsAsync(int campaignId);

        /// <summary>
        /// Adds new campaign statistics to the database.
        /// </summary>
        /// <param name="entity">The campaign statistics entity to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the statistics entity is null.</exception>
        Task AddCampaignStatisticsAsync(CampaignStatistics entity);

        /// <summary>
        /// Updates existing campaign statistics in the database.
        /// </summary>
        /// <param name="statistics">The campaign statistics entity to update.</param>
        /// <exception cref="ArgumentNullException">Thrown when the statistics entity is null.</exception>
        void UpdateCampaignStatistics(CampaignStatistics entity);
    }
}
