using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.Core.DTOs.FeedbackDTOs
{
    public class FeedbackAnswerCreateDTO
    {
        public int QuestionId { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Response { get; set; } = string.Empty;
    }
}
