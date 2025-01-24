using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.Core.DTOs.FeedbackDTOs
{
   
    /// <summary>
    /// DTO for representing the summarized feedback results
    /// of a single feedback form.
    /// </summary>
    public class FeedbackSummeryDTO
    {
        // Basic Metrics

        public decimal AverageRating { get; set; }
        public int TotalResponses { get; set; }
        public int PositiveFeedbackCount { get; set; }
        public int NegativeFeedbackCount { get; set; }

        // Trends

        public string AnswerRatio { get; set; } = string.Empty;
        public string RatingDistribution { get; set; } = string.Empty;
        public decimal ResponseRate { get; set; }

        // Time 
        public DateTime? PeakResponseTime { get; set; }
        public TimeSpan? ResponseDurationAverage { get; set; }

        // Intelligent Analysis
        public string InteligentSummery { get; set; } = string.Empty;
        public decimal SentimentScore { get; set; }
        public string TopKeywords { get; set; } = string.Empty;
        public string CommonIssues { get; set; } = string.Empty;
        public string ImprovementSuggestions { get; set; } = string.Empty;

        // Benchmarks
        public decimal BenchmarkComparison { get; set; }
        public decimal PreviousAverageRating { get; set; }
    }
}
