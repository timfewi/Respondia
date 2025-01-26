using FluentValidation;
using Newtonsoft.Json;
using Respondia.Core.DTOs.FeedbackDTOs;
using Respondia.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.BL.Validators.FeedbackValidators
{
    public class FeedbackQuestionCreateDTOValidator : AbstractValidator<FeedbackQuestionCreateDTO>
    {
        public FeedbackQuestionCreateDTOValidator()
        {
            RuleFor(x => x.QuestionText)
              .NotEmpty().WithMessage("FeedbackQuestion QuestionText must not be empty.")
              .MaximumLength(500).WithMessage("FeedbackQuestion QuestionText must be less than 500 characters");

            RuleFor(x => x.QuestionType)
                .IsInEnum().WithMessage("FeedbackQuestion QuestionType mus be a valid enum value.");

            RuleFor(x => x.MultipleChoiceOptions)
                .Must((dto, multipleChoiceOptions) =>
                {
                    if (dto.QuestionType == Core.Enums.QuestionType.MultipleChoice)
                    {
                        return !string.IsNullOrEmpty(multipleChoiceOptions);
                    }
                    return true;
                })
                .WithMessage("MultipleChoiceOptions must not be empty for a MultipleChoice question.");

            RuleFor(x => x.MultipleChoiceOptions)
              .Must(IsValidJsonArray)
              .When(x => x.QuestionType == QuestionType.MultipleChoice)
              .WithMessage("MultipleChoiceOptions must be a valid JSON array.");

            RuleFor(x => x.MultipleChoiceOptions)
                .Must(options =>
                {
                    if (!string.IsNullOrEmpty(options) && IsValidJsonArray(options))
                    {
                        var deserializedOptions = JsonConvert.DeserializeObject<List<string>>(options);
                        return deserializedOptions != null && deserializedOptions.Count > 0 && deserializedOptions.TrueForAll(opt => !string.IsNullOrWhiteSpace(opt));
                    }
                    return true;
                })
                .When(x => x.QuestionType == QuestionType.MultipleChoice)
                .WithMessage("MultipleChoiceOptions must be a non-empty JSON array with valid entries.");
        }

        private bool IsValidJsonArray(string json)
        {
            try
            {
                var deserialized = JsonConvert.DeserializeObject<List<string>>(json);
                return deserialized != null;
            }
            catch
            {
                return false;
            }
        }
    }
}
