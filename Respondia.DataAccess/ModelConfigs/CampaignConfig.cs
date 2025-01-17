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
    public class CampaignConfig : BaseEntityConfig<Campaign>
    {
        public override void Configure(EntityTypeBuilder<Campaign> builder)
        {
            base.Configure(builder);

            builder.ToTable("Campaigns");

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(c => c.Description)
                   .HasMaxLength(1000);

            builder.HasOne(c => c.AppUser)
                   .WithMany(u => u.Campaigns)
                   .HasForeignKey(c => c.AppUserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.Statistics)
                   .WithOne(cs => cs.Campaign)
                   .HasForeignKey<CampaignStatistics>(cs => cs.CampaignId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
