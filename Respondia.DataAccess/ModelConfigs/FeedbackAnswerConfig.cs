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
    public class FeedbackAnswerConfig : BaseEntityConfig<FeedbackAnswer>
    {
        public override void Configure(EntityTypeBuilder<FeedbackAnswer> builder)
        {
            base.Configure(builder);

            builder.ToTable("FeedbackAnswers");

            builder.Property(a => a.Response)
                   .IsRequired(false);

            builder.Property(a => a.UserIdentifier)
                   .HasMaxLength(250);

            builder.HasOne(a => a.FeedbackQuestion)
                   .WithMany(q => q.FeedbackAnswers)
                   .HasForeignKey(a => a.FeedbackQuestionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
