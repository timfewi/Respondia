using Respondia.Core.DTOs.FeedbackDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.Core.Interfaces.Services
{
    public interface IFeedbackQueryService
    {
        /// <summary>
        /// Retrieves a list of all feedback forms for a campaign.
        /// </summary>
        /// <param name="campaignId">The ID of the campaign.</param>
        /// <returns>A list of feedback form overviews.</returns>
        Task<IEnumerable<FeedbackFormOverviewDTO>> GetFeedbackFormsAsync(int campaignId);

        /// <summary>
        /// Retrieves the detailed information of a feedback form, including questions and summary.
        /// </summary>
        /// <param name="formId">The ID of the feedback form.</param>
        /// <returns>The details of the feedback form, or null if not found.</returns>
        Task<FeedbackFormDetailsDTO?> GetFeedbackFormDetailsAsync(int formId);

        /// <summary>
        /// Retrieves the summary/analysis for a feedback form.
        /// </summary>
        /// <param name="formId">The ID of the feedback form.</param>
        /// <returns>The analysis of the feedback form, or null if no summary is available.</returns>
        Task<FeedbackAnalysisDTO?> GetFeedbackAnalysisAsync(int formId);
    
    }

}
