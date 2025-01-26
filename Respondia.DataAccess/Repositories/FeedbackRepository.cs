using Microsoft.AspNetCore.Components;
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
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<FeedbackForm> _forms;
        private readonly DbSet<FeedbackQuestion> _questions;
        private readonly DbSet<FeedbackAnswer> _answers;
        private readonly DbSet<FeedbackSummery> _summaries;
        private readonly DbSet<FeedbackLink> _links;
        private readonly ILogger<FeedbackRepository> _logger;

        public FeedbackRepository(AppDbContext context, ILogger<FeedbackRepository> logger)
        {
            _context = context;
            _forms = _context.Set<FeedbackForm>();
            _questions = _context.Set<FeedbackQuestion>();
            _answers = _context.Set<FeedbackAnswer>();
            _summaries = _context.Set<FeedbackSummery>();
            _links = _context.Set<FeedbackLink>();
            _logger = logger;
        }

        #region FeedbackForm CRUD

        /// <inheritdoc/>
        public async Task<IEnumerable<FeedbackForm>> GetAllAsync()
        {
            try
            {
                var forms = await _forms
                    .AsNoTracking()
                    .Include(f => f.Campaign)
                    .Include(f => f.FeedbackSummery)
                    .Include(f => f.FeedbackQuestions)
                    .Where(f => !f.IsDeleted)
                    .ToListAsync();

                return forms ?? new List<FeedbackForm>();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation in {Method}.", nameof(GetAllAsync));
                throw new DataAccessException("An error occurred while retrieving feedback forms.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in {Method}: {Message}", nameof(GetAllAsync), ex.Message);
                throw new DataAccessException($"Unexpected error in {nameof(GetAllAsync)}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<FeedbackForm?> GetByIdAsync(int id)
        {
            try
            {
                var form = await _forms
                    .AsNoTracking()
                    .Include(f => f.Campaign)
                    .Include(f => f.FeedbackSummery)
                    .Include(f => f.FeedbackQuestions)
                    .FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted);

                if (form == null)
                {
                    _logger.LogWarning("FeedbackForm with Id {Id} not found.", id);
                }
                else
                {
                    _logger.LogInformation("Found FeedbackForm with Id {Id}.", id);
                }

                return form;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation while retrieving form with Id {Id}. Method: {Method}", id, nameof(GetByIdAsync));
                throw new DataAccessException($"Error retrieving FeedbackForm with Id {id}.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in {Method}: {Message}", nameof(GetByIdAsync), ex.Message);
                throw new DataAccessException($"Unexpected error in {nameof(GetByIdAsync)}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<FeedbackForm>> FindAsync(Expression<Func<FeedbackForm, bool>> predicate)
        {
            try
            {
                var forms = await _forms
                    .AsNoTracking()
                    .Include(f => f.Campaign)
                    .Include(f => f.FeedbackSummery)
                    .Include(f => f.FeedbackQuestions)
                    .Where(predicate)
                    .Where(f => !f.IsDeleted)
                    .ToListAsync();

                return forms;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation in {Method}.", nameof(FindAsync));
                throw new DataAccessException("Error while retrieving feedback forms by predicate.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in {Method}: {Message}", nameof(FindAsync), ex.Message);
                throw new DataAccessException($"Unexpected error in {nameof(FindAsync)}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task AddAsync(FeedbackForm entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                await _forms.AddAsync(entity);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error while adding {Entity}. Method: {Method}", nameof(FeedbackForm), nameof(AddAsync));
                throw new DataAccessException($"Error inserting {nameof(FeedbackForm)}.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in {Method}: {Message}", nameof(AddAsync), ex.Message);
                throw new DataAccessException($"Unexpected error in {nameof(AddAsync)}.", ex);
            }
        }

        /// <inheritdoc/>
        public void Remove(FeedbackForm entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                // in DbContext -> SoftDelete
                _forms.Remove(entity);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error while removing {Entity}. Method: {Method}", nameof(FeedbackForm), nameof(Remove));
                throw new DataAccessException($"Error removing {nameof(FeedbackForm)}.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in {Method}: {Message}", nameof(Remove), ex.Message);
                throw new DataAccessException($"Unexpected error in {nameof(Remove)}.", ex);
            }
        }

        /// <inheritdoc/>
        public void Update(FeedbackForm entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                // EF-Core: State Modified
                _forms.Update(entity);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error while updating {Entity}. Method: {Method}", nameof(FeedbackForm), nameof(Update));
                throw new DataAccessException($"Error updating {nameof(FeedbackForm)}.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in {Method}: {Message}", nameof(Update), ex.Message);
                throw new DataAccessException($"Unexpected error in {nameof(Update)}.", ex);
            }
        }
        #endregion

        #region Specific Methods

        /// <inheritdoc/>
        public async Task<FeedbackForm?> GetFormWithDetailsAsync(int formId)
        {
            try
            {
                // Deep load
                var form = await _forms
                    .AsNoTracking()
                    .Include(f => f.Campaign)
                    .Include(f => f.FeedbackSummery)
                    .Include(f => f.FeedbackQuestions)
                        .ThenInclude(q => q.FeedbackAnswers)
                    .FirstOrDefaultAsync(f => f.Id == formId && !f.IsDeleted);

                if (form == null)
                {
                    _logger.LogWarning("FeedbackForm with Id {Id} not found for full details load.", formId);
                }
                else
                {
                    _logger.LogInformation("Loaded FeedbackForm with Id {Id} including details.", formId);
                }

                return form;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation in {Method}. FormId={Id}", nameof(GetFormWithDetailsAsync), formId);
                throw new DataAccessException($"Error retrieving details for FeedbackForm with Id {formId}.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in {Method}: {Message}", nameof(GetFormWithDetailsAsync), ex.Message);
                throw new DataAccessException($"Unexpected error in {nameof(GetFormWithDetailsAsync)}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<FeedbackForm>> GetFeedbackFormsByCampaignAsync(int campaignId)
        {
            try
            {
                var forms = await _forms
                    .AsNoTracking()
                    .Include(f => f.Campaign)
                    .Include(f => f.FeedbackSummery)
                    .Include(f => f.FeedbackQuestions)
                    .Where(f => f.CampaignId == campaignId && !f.IsDeleted)
                    .ToListAsync();

                _logger.LogInformation("Retrieved {Count} FeedbackForms for CampaignId {CampaignId}.", forms.Count, campaignId);
                return forms;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation in {Method}. CampaignId={CampaignId}", nameof(GetFeedbackFormsByCampaignAsync), campaignId);
                throw new DataAccessException($"Error retrieving FeedbackForms for CampaignId {campaignId}.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in {Method}: {Message}", nameof(GetFeedbackFormsByCampaignAsync), ex.Message);
                throw new DataAccessException($"Unexpected error in {nameof(GetFeedbackFormsByCampaignAsync)}.", ex);
            }
        }

        #region FeedbackQuestions

        /// <inheritdoc/>
        public async Task AddQuestionAsync(FeedbackQuestion question)
        {
            if (question == null)
                throw new ArgumentNullException(nameof(question));

            try
            {
                await _questions.AddAsync(question);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error adding {Entity}. Method: {Method}",
                    nameof(FeedbackQuestion), nameof(AddQuestionAsync));
                throw new DataAccessException($"Error inserting {nameof(FeedbackQuestion)}.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in {Method}: {Message}",
                    nameof(AddQuestionAsync), ex.Message);
                throw new DataAccessException($"Unexpected error in {nameof(AddQuestionAsync)}.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<FeedbackQuestion?> GetQuestionWithAnswersAsync(int questionId)
        {
            try
            {
                var question = await _questions
                    .AsNoTracking()
                    .Include(q => q.FeedbackAnswers)
                    .FirstOrDefaultAsync(q => q.Id == questionId && !q.IsDeleted);

                if (question == null)
                {
                    _logger.LogWarning("FeedbackQuestion with Id {Id} not found.", questionId);
                }
                else
                {
                    _logger.LogInformation("FeedbackQuestion with Id {Id} loaded (with answers).", questionId);
                }

                return question;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation for question Id {Id}. Method: {Method}", questionId, nameof(GetQuestionWithAnswersAsync));
                throw new DataAccessException($"Error retrieving question with Id {questionId}.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in {Method}: {Message}", nameof(GetQuestionWithAnswersAsync), ex.Message);
                throw new DataAccessException($"Unexpected error in {nameof(GetQuestionWithAnswersAsync)}.", ex);
            }
        }

        #endregion

        #region FeedbackAnswer

        /// <inheritdoc/>
        public async Task AddAnswerAsync(FeedbackAnswer answer)
        {
            if (answer == null)
                throw new ArgumentNullException(nameof(answer));

            try
            {
                await _answers.AddAsync(answer);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error adding {Entity}. Method: {Method}",
                    nameof(FeedbackAnswer), nameof(AddAnswerAsync));
                throw new DataAccessException($"Error inserting {nameof(FeedbackAnswer)}.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in {Method}: {Message}",
                    nameof(AddAnswerAsync), ex.Message);
                throw new DataAccessException($"Unexpected error in {nameof(AddAnswerAsync)}.", ex);
            }
        }

        #endregion

        #region FeedbackSummery

        /// <inheritdoc/>
        public async Task AddOrUpdateSummeryAsync(FeedbackSummery summery)
        {
            if (summery == null)
                throw new ArgumentNullException(nameof(summery));

            try
            {
                if (summery.Id == 0)
                {
                    await _summaries.AddAsync(summery);
                }
                else
                {
                    _summaries.Update(summery);
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error adding/updating {Entity}. Method: {Method}",
                    nameof(FeedbackSummery), nameof(AddOrUpdateSummeryAsync));
                throw new DataAccessException($"Error inserting or updating {nameof(FeedbackSummery)}.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in {Method}: {Message}",
                    nameof(AddOrUpdateSummeryAsync), ex.Message);
                throw new DataAccessException($"Unexpected error in {nameof(AddOrUpdateSummeryAsync)}.", ex);
            }
        }

        #endregion

        #region FeedbackLink

        /// <inheritdoc/>
        public async Task AddFeedbackLinkAsync(FeedbackLink feedbackLink)
        {
            if (feedbackLink == null)
                throw new ArgumentNullException(nameof(feedbackLink));

            try
            {
                await _links.AddAsync(feedbackLink);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error adding {Entity}. Method: {Method}", nameof(FeedbackLink), nameof(AddFeedbackLinkAsync));
                throw new DataAccessException($"Error inserting {nameof(FeedbackLink)}.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in {Method}: {Message}", nameof(AddFeedbackLinkAsync), ex.Message);
                throw new DataAccessException($"Unexpected error in {nameof(AddFeedbackLinkAsync)}.", ex);
            }
        }

        #endregion

        #endregion
    }
}
