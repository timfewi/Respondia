using Respondia.Core.DTOs.FeedbackDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.Core.Interfaces.Services
{
    public interface IFeedbackCommandService
    {
        /// <summary>
        /// Creates a new feedback form.
        /// </summary>
        /// <param name="dto">DTO containing the information for the form.</param>
        /// <param name="userId">ID of the user creating the form.</param>
        /// <returns>The ID of the newly created form.</returns>
        Task<int> CreateFeedbackFormAsync(FeedbackFormCreateDTO dto, int userId);

        /// <summary>
        /// Adds a new question to an existing form.
        /// </summary>
        /// <param name="formId">ID of the form.</param>
        /// <param name="questionDto">DTO containing question information.</param>
        /// <param name="userId">ID of the user adding the question.</param>
        Task AddQuestionAsync(int formId, FeedbackQuestionCreateDTO questionDto, int userId);

        /// <summary>
        /// Generates a link to fill out the form and saves it in the database.
        /// </summary>
        /// <param name="formId">ID of the form.</param>
        /// <param name="expiryDate">Expiration date of the link.</param>
        /// <param name="userId">ID of the user creating the link.</param>
        /// <returns>A DTO containing the token, expiration date, etc.</returns>
        Task<FeedbackLinkDTO> GenerateFeedbackLinkAsync(int formId, DateTime expiryDate, int userId);
    }

}
