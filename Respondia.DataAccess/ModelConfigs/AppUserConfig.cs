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
    public class AppUserConfig : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AppUsers");

            builder.Property(u => u.DisplayName)
                   .HasMaxLength(100);

            builder.Property(u => u.ExternalProvider)
                   .HasMaxLength(50);

            builder.Property(u => u.ProviderKey)
                   .HasMaxLength(250);

            // Optional: Beziehungen definieren
            builder.HasMany(u => u.Campaigns)
                   .WithOne(c => c.AppUser)
                   .HasForeignKey(c => c.AppUserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
