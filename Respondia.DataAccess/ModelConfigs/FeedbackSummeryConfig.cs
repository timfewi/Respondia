using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Respondia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.DataAccess.ModelConfigs
{
    public class FeedbackSummeryConfig : BaseEntityConfig<FeedbackSummery>
    {
        public override void Configure(EntityTypeBuilder<FeedbackSummery> builder)
        {
            base.Configure(builder);

            builder.ToTable("FeedbackSummaries");

            builder.HasOne(fs => fs.FeedbackForm) 
                   .WithOne(ff => ff.FeedbackSummery)
                   .HasForeignKey<FeedbackSummery>(fs => fs.FeedbackFormId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Basic Metrics
            builder.Property(fs => fs.AverageRating)
                   .HasPrecision(5, 2) //  123.45
                   .IsRequired(); 

            builder.Property(fs => fs.TotalResponses)
                   .IsRequired(); 

            builder.Property(fs => fs.PositiveFeedbackCount)
                   .IsRequired();

            builder.Property(fs => fs.NegativeFeedbackCount)
                   .IsRequired(); 

            // Trends
            builder.Property(fs => fs.AnswerRatio)
                   .HasMaxLength(1000) 
                   .IsRequired(false); 

            builder.Property(fs => fs.RatingDistribution)
                   .HasColumnType("nvarchar(max)") 
                   .IsRequired(false); 

            builder.Property(fs => fs.ResponseRate)
                   .HasPrecision(5, 2) // 100.00%
                   .IsRequired(); 

            // Time
            builder.Property(fs => fs.PeakResponseTime)
                   .IsRequired(false); 

            builder.Property(fs => fs.ResponseDurationAverage)
                   .IsRequired(false); 

            // Inteligent Analyse
            builder.Property(fs => fs.InteligentSummery)
                   .HasMaxLength(4000) 
                   .IsRequired(false); 

            builder.Property(fs => fs.SentimentScore)
                   .HasPrecision(4, 2) // -1.00 to 1.00
                   .IsRequired(); 

            builder.Property(fs => fs.TopKeywords)
                   .HasMaxLength(1000)
                   .IsRequired(false); 

            builder.Property(fs => fs.CommonIssues)
                   .HasMaxLength(1000) 
                   .IsRequired(false); 

            builder.Property(fs => fs.ImprovementSuggestions)
                   .HasMaxLength(1000) 
                   .IsRequired(false); 

            // Benchmarks
            builder.Property(fs => fs.BenchmarkComparison)
                   .HasPrecision(5, 2) // 99.99
                   .IsRequired(); 

            builder.Property(fs => fs.PreviousAverageRating)
                   .HasPrecision(5, 2) // 4.56
                   .IsRequired(); 
        }
    }
}
