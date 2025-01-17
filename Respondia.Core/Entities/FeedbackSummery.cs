using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.Core.Entities
{
    public class FeedbackSummery : BaseEntity
    {
        // Basic Metrics
        public decimal AverageRating { get; set; }
        public int TotalResponses { get; set; }
        public int PositiveFeedbackCount { get; set; }
        public int NegativeFeedbackCount { get; set; }

        // Trends
        public string AnswerRatio { get; set; } = string.Empty;
        public string RatingDistribution { get; set; } = string.Empty; // JSON für Verteilung
        public decimal ResponseRate { get; set; } // Prozentsatz der Antworten

        // Time 
        public DateTime? PeakResponseTime { get; set; }
        public TimeSpan? ResponseDurationAverage { get; set; }

        // Inteligent 
        public string InteligentSummery { get; set; } = string.Empty;
        public decimal SentimentScore { get; set; } // z. B. von -1.0 bis 1.0
        public string TopKeywords { get; set; } = string.Empty;
        public string CommonIssues { get; set; } = string.Empty;
        public string ImprovementSuggestions { get; set; } = string.Empty;

        // Benchmarks
        public decimal BenchmarkComparison { get; set; }
        public decimal PreviousAverageRating { get; set; }


        public int FeedbackFormId { get; set; } 
        public FeedbackForm FeedbackForm { get; set; } = default!;
    }

}
