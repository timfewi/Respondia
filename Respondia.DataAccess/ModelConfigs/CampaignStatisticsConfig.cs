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
    public class CampaignStatisticsConfig : BaseEntityConfig<CampaignStatistics>
    {
        public override void Configure(EntityTypeBuilder<CampaignStatistics> builder)
        {
            base.Configure(builder);

            builder.ToTable("CampaignStatistics");

            builder.Property(cs => cs.TotalResponses)
                   .IsRequired();

            builder.Property(cs => cs.AverageRating)
                   .HasColumnType("decimal(5, 2)"); // Beispiel für eine Bewertungsskala

            builder.HasOne(cs => cs.Campaign)
                   .WithOne(c => c.Statistics)
                   .HasForeignKey<CampaignStatistics>(cs => cs.CampaignId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
