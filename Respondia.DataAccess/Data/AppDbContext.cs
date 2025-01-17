using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Respondia.Core.Entities;
using Respondia.DataAccess.ModelConfigs;
using Respondia.DataAccess.Utils;
using System.Linq.Expressions;

namespace Respondia.DataAccess.Data
{
    public class AppDbContext : IdentityUserContext<AppUser, int>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        /// <summary>
        /// Configures the model that was discovered by convention from the entity types
        /// exposed in <see cref="DbSet{TEntity}"/> properties on your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EntryPointDataAccess).Assembly);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                                .HasQueryFilter(CreateSoftDeleteFilter(entityType.ClrType));
                }
            }
        }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        public override int SaveChanges()
        {
            ApplyAuditableChanges();
            return base.SaveChanges();
        }

        /// <summary>
        /// Asynchronously saves all changes made in this context to the database.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditableChanges();
            return await base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Applies changes to auditable entities before saving to the database.
        /// </summary>
        private void ApplyAuditableChanges()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.DateCreated = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.DateUpdated = DateTime.Now;
                        break;
                    case EntityState.Deleted:
                        entry.Entity.IsDeleted = true;
                        entry.Entity.DateUpdated = DateTime.Now;
                        entry.State = EntityState.Modified;
                        break;
                }
            }
        }

        /// <summary>
        /// Creates a query filter expression for soft delete functionality.
        /// </summary>
        /// <param name="entityType">The type of the entity.</param>
        /// <returns>A lambda expression representing the query filter.</returns>
        private static LambdaExpression CreateSoftDeleteFilter(Type entityType)
        {
            var parameter = Expression.Parameter(entityType, "e");
            var body = Expression.Equal(
                Expression.Property(parameter, nameof(BaseEntity.IsDeleted)),
                Expression.Constant(false));

            return Expression.Lambda(body, parameter);
        }
    }
}
