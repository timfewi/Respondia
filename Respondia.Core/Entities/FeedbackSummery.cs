using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.Core.Entities
{
    /// <summary>
    /// Represents a summary of feedback for a specific feedback form, including metrics, trends, time analysis, intelligent summaries, and benchmarks.
    /// </summary>
    public class FeedbackSummery : BaseEntity
    {
        // Basic Metrics

        /// <summary>
        /// Gets or sets the average rating calculated from all feedback responses.
        /// </summary>
        public decimal AverageRating { get; set; }

        /// <summary>
        /// Gets or sets the total number of responses received.
        /// </summary>
        public int TotalResponses { get; set; }

        /// <summary>
        /// Gets or sets the count of positive feedback responses.
        /// </summary>
        public int PositiveFeedbackCount { get; set; }

        /// <summary>
        /// Gets or sets the count of negative feedback responses.
        /// </summary>
        public int NegativeFeedbackCount { get; set; }

        // Trends

        /// <summary>
        /// Gets or sets the answer ratio, representing the proportion of different responses.
        /// </summary>
        public string AnswerRatio { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the rating distribution in JSON format.
        /// </summary>
        public string RatingDistribution { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the response rate as a percentage of total invitations.
        /// </summary>
        public decimal ResponseRate { get; set; }

        // Time 

        /// <summary>
        /// Gets or sets the time when the peak number of responses were received.
        /// </summary>
        public DateTime? PeakResponseTime { get; set; }

        /// <summary>
        /// Gets or sets the average duration it took for respondents to complete the feedback form.
        /// </summary>
        public TimeSpan? ResponseDurationAverage { get; set; }

        // Intelligent Analysis 

        /// <summary>
        /// Gets or sets an intelligent summary generated from the feedback responses.
        /// </summary>
        public string InteligentSummery { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the sentiment score ranging from -1.0 to 1.0 indicating overall sentiment.
        /// </summary>
        public decimal SentimentScore { get; set; }

        /// <summary>
        /// Gets or sets the top keywords extracted from the feedback.
        /// </summary>
        public string TopKeywords { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the common issues identified from the feedback.
        /// </summary>
        public string CommonIssues { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets suggested improvements based on the feedback analysis.
        /// </summary>
        public string ImprovementSuggestions { get; set; } = string.Empty;

        // Benchmarks

        /// <summary>
        /// Gets or sets the comparison metric against predefined benchmarks.
        /// </summary>
        public decimal BenchmarkComparison { get; set; }

        /// <summary>
        /// Gets or sets the previous average rating for comparative analysis.
        /// </summary>
        public decimal PreviousAverageRating { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated feedback form.
        /// </summary>
        public int FeedbackFormId { get; set; }

        /// <summary>
        /// Gets or sets the associated feedback form.
        /// </summary>
        public FeedbackForm FeedbackForm { get; set; } = default!;
    }

}
