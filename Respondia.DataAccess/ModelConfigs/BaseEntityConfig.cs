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
    public class BaseEntityConfig<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            // PrimaryKey
            builder.HasKey(e => e.Id);

            // IsDeleted
            builder.Property(e => e.IsDeleted)
                   .HasDefaultValue(false)
                   .IsRequired();

            // DateCreated
            builder.Property(e => e.DateCreated)
                   .IsRequired()
                   .HasDefaultValueSql("GETDATE()");

            // DateUpdated
            builder.Property(e => e.DateUpdated)
                   .IsRequired(false);

            builder.Property(e => e.RowVersion)
                   .IsRowVersion()
                   .IsConcurrencyToken();
        }
    }
}
