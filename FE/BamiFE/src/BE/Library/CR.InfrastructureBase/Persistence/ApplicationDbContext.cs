using System.Security.Claims;
using CR.EntitiesBase.Entities;
using CR.EntitiesBase.Interfaces;
using CR.InfrastructureBase.HttpContexts;
using CR.InfrastructureBase.MultiTenancy;
using CR.Utils.DataUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CR.InfrastructureBase.Persistence
{
    public class ApplicationDbContext<TUser> : DbContext, IMultiTenancyOption
        where TUser : class, IUser
    {
        protected readonly IHttpContextAccessor _httpContextAccessor = null!;
        protected readonly int? UserId = null;
        //private readonly TenantCommandInterceptor? _tenantCommandInterceptor;

        public virtual DbSet<TUser> Users { get; set; }
        public int? TenantId { get; set; }

        public ApplicationDbContext() { }

        public ApplicationDbContext(
            DbContextOptions options,
            IHttpContextAccessor httpContextAccessor
        )
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            UserId = _httpContextAccessor.GetCurrentUserIdInContext();
            //_tenantCommandInterceptor = _httpContextAccessor
            //    .HttpContext?.RequestServices
            //    .GetRequiredService<TenantCommandInterceptor>();
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (_tenantCommandInterceptor != null)
        //    {
        //        optionsBuilder.AddInterceptors(_tenantCommandInterceptor);
        //    }
        //    base.OnConfiguring(optionsBuilder);
        //}

        private void CheckAudit()
        {
            ChangeTracker.DetectChanges();
            var added = ChangeTracker
                .Entries()
                .Where(t => t.State == EntityState.Added)
                .Select(t => t.Entity)
                .AsParallel();

            added.ForAll(entity =>
            {
                if (entity is ICreatedBy createdEntity && createdEntity.CreatedBy == null)
                {
                    createdEntity.CreatedDate = DateTimeUtils.GetDate();
                    createdEntity.CreatedBy = UserId;
                }

                if (entity is IMultiTenancy multiTenant)
                {
                    multiTenant.TenantId = TenantId ?? 0;
                }

                if (entity is IMultiTenancyOption multiTenancyOption && TenantId != null)
                {
                    multiTenancyOption.TenantId = TenantId;
                }
            });

            var modified = ChangeTracker
                .Entries()
                .Where(t => t.State == EntityState.Modified)
                .Select(t => t.Entity)
                .AsParallel();
            modified.ForAll(entity =>
            {
                if (entity is IModifiedBy modifiedEntity && modifiedEntity.ModifiedBy == null)
                {
                    modifiedEntity.ModifiedDate = DateTimeUtils.GetDate();
                    modifiedEntity.ModifiedBy = UserId;
                }
                if (
                    entity is ISoftDeleted softDeletedEntity
                    && softDeletedEntity.Deleted
                    && softDeletedEntity.DeletedBy == null
                )
                {
                    softDeletedEntity.DeletedDate = DateTimeUtils.GetDate();
                    softDeletedEntity.DeletedBy = UserId;
                }
            });
        }

        public override int SaveChanges()
        {
            CheckAudit();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            CheckAudit();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            CheckAudit();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default
        )
        {
            CheckAudit();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddTenantQueryFilter();
            base.OnModelCreating(modelBuilder);
        }
    }
}
