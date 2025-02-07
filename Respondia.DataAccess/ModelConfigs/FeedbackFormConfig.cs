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
    public class FeedbackFormConfig : BaseEntityConfig<FeedbackForm>
    {
        public override void Configure(EntityTypeBuilder<FeedbackForm> builder)
        {
            base.Configure(builder);

            builder.ToTable("FeedbackForms");

            builder.Property(f => f.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(f => f.Description)
                   .HasMaxLength(1000);

            builder.HasOne(f => f.Campaign)
                   .WithMany(c => c.FeedbackForms)
                   .HasForeignKey(f => f.CampaignId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.FeedbackSummery)
                   .WithOne(fs => fs.FeedbackForm)
                   .HasForeignKey<FeedbackSummery>(fs => fs.FeedbackFormId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.FeedbackLink)
                   .WithOne(fl => fl.FeedbackForm)
                   .HasForeignKey<FeedbackLink>(fl => fl.FeedbackFormId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
