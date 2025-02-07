using Respondia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.Core.Interfaces.Repositories
{
    /// <summary>
    /// Provides methods for accessing and manipulating different feedback data in the database.
    /// </summary>
    public interface IFeedbackRepository : IRepository<FeedbackForm>
    {
        /// <summary>
        /// Retrieves a feedback form along with its associated details asynchronously.
        /// </summary>
        /// <param name="formId">The unique identifier of the feedback form.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the feedback form with its details if found; otherwise, null.</returns>
        Task<FeedbackForm?> GetFormWithDetailsAsync(int formId);

        /// <summary>
        /// Retrieves a collection of feedback forms associated with a specific campaign asynchronously.
        /// </summary>
        /// <param name="campaignId">The unique identifier of the campaign.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of feedback forms associated with the specified campaign.</returns>
        Task<IEnumerable<FeedbackForm>> GetFeedbackFormsByCampaignAsync(int campaignId);

        /// <summary>
        /// Adds a new feedback question to the database asynchronously.
        /// </summary>
        /// <param name="question">The feedback question to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddQuestionAsync(FeedbackQuestion question);

        /// <summary>
        /// Retrieves a feedback question along with its associated answers asynchronously.
        /// </summary>
        /// <param name="questionId">The unique identifier of the feedback question.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the feedback question with its answers if found; otherwise, null.</returns>
        Task<FeedbackQuestion?> GetQuestionWithAnswersAsync(int questionId);

        /// <summary>
        /// Retrieves a collection of feedback questions associated with a specific feedback form asynchronously.
        /// </summary>
        /// <param name="feedbackFormId">The unique identifier of the feedback form.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of feedback questions associated with the specified feedback form.</returns>
        Task<IEnumerable<FeedbackQuestion>> GetQuestionsByFeedbackFormIdAsync(int feedbackFormId);

        /// <summary>
        /// Adds a new feedback answer to the database asynchronously.
        /// </summary>
        /// <param name="answer">The feedback answer to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddAnswerAsync(FeedbackAnswer answer);

        /// <summary>
        /// Adds or updates a feedback summary in the database asynchronously.
        /// </summary>
        /// <param name="summery">The feedback summary to add or update.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddOrUpdateSummeryAsync(FeedbackSummery summery);

        /// <summary>
        /// Adds a new feedback link to the database asynchronously.
        /// </summary>
        /// <param name="feedbackLink">The feedback link to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddFeedbackLinkAsync(FeedbackLink feedbackLink);

        /// <summary>
        /// Retrieves a feedback link by its unique token asynchronously.
        /// </summary>
        /// <param name="token">The unique token of the feedback link.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the feedback link if found; otherwise, null.</returns>
        Task<FeedbackLink?> GetFeedbackLinkByTokenAsync(string token);
    }
}
