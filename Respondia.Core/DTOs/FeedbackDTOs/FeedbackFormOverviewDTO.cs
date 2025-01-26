using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.Core.DTOs.FeedbackDTOs
{
    public class FeedbackFormOverviewDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TotalQuestions { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
