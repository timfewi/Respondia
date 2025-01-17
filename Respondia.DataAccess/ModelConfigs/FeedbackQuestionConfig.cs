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
    public class FeedbackQuestionConfig : BaseEntityConfig<FeedbackQuestion>
    {
        public override void Configure(EntityTypeBuilder<FeedbackQuestion> builder)
        {
            base.Configure(builder);

            builder.ToTable("FeedbackQuestions");

            builder.Property(q => q.QuestionText)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(q => q.QuestionType)
                   .IsRequired();

            // JSON-Data in MSSQL
            builder.Property(q => q.MultipleChoiceOptions)
                   .HasColumnType("nvarchar(max)")
                   .HasConversion(
                       v => v, 
                       v => v 
                   );

            builder.HasOne(q => q.FeedbackForm)
                   .WithMany(f => f.FeedbackQuestions)
                   .HasForeignKey(q => q.FeedbackFormId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
