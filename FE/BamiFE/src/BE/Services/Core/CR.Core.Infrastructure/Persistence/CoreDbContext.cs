using CR.Constants.Authorization.Role;
using CR.Constants.Common.Database;
using CR.Constants.Core.Users;
using CR.Core.Domain.AuthToken;
using CR.Core.Domain.Batch;
using CR.Core.Domain.Brand;
using CR.Core.Domain.FilePrint;
using CR.Core.Domain.Order;
using CR.Core.Domain.Otps;
using CR.Core.Domain.Partner;
using CR.Core.Domain.Sku;
using CR.Core.Domain.SysVar;
using CR.Core.Domain.Users;
using CR.InfrastructureBase.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CR.Core.Infrastructure.Persistence
{
    public partial class CoreDbContext : ApplicationDbContext<User>
    {
        public override DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<SysVar> SysVars { get; set; }
        public DbSet<NotificationToken> NotificationTokens { get; set; }
        public DbSet<AuthOtp> AuthOtps { get; set; }
        public DbSet<SendOtp> SendOtps { get; set; }

        public DbSet<CorePartner> CorePartners { get; set; }
        public DbSet<CorePartnerType> CorePartnerTypes { get; set; }
        public DbSet<CoreSku> CoreSkus { get; set; }
        public DbSet<CoreSkuBase> CoreSkuBases { get; set; }
        public DbSet<CoreSkuSize> CoreSkuSizes { get; set; }
        public DbSet<CoreSkuSizePkgMockup> CoreSkuSizePkgMockups { get; set; }
        public DbSet<CoreMaterial> CoreMaterials { get; set; }
        public DbSet<CoreProductionMethod> CoreProductionMethods { get; set; }
        public DbSet<CoreFilePrint> CoreFilePrints { get; set; }
        public DbSet<CoreOrder> CoreOrders { get; set; }
        public DbSet<CoreOrderDetail> CoreOrderDetails { get; set; }
        public DbSet<CoreOrderItem> CoreOrderItems { get; set; }
        public DbSet<CoreBrand> CoreBrands { get; set; }
        public DbSet<CoreStore> CoreStores { get; set; }
        public DbSet<CoreBatch> CoreBatches { get; set; }

        public CoreDbContext()
            : base() { }

        public CoreDbContext(
            DbContextOptions<CoreDbContext> options,
            IHttpContextAccessor httpContextAccessor
        )
            : base(options, httpContextAccessor) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(DbSchemas.CRCore);
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.IsTempPin).HasColumnName("IsTempPin").HasDefaultValue(false);
                entity
                    .Property(e => e.Status)
                    .HasColumnName("Status")
                    .HasDefaultValue(UserStatus.ACTIVE);
                entity
                    .Property(e => e.UserType)
                    .HasColumnName("UserType")
                    .HasDefaultValue(UserTypeEnum.ADMIN);
                entity
                    .Property(e => e.LoginFailCount)
                    .HasColumnName("LoginFailCount")
                    .HasDefaultValue(0);
                entity
                    .Property(e => e.DateTimeLoginFailCount)
                    .HasColumnName("DateTimeLoginFailCount");
            });
            modelBuilder
                .Entity<NotificationToken>()
                .HasOne(e => e.User)
                .WithMany(x => x.NotificationTokens)
                .HasForeignKey(e => e.UserId);
            modelBuilder
                .Entity<AuthOtp>()
                .HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId);

            modelBuilder
                .Entity<RolePermission>()
                .HasOne(e => e.Role)
                .WithMany(x => x.RolePermissions)
                .HasForeignKey(e => e.RoleId);

            modelBuilder.Entity<UserRole>().HasOne<Role>().WithMany().HasForeignKey(e => e.RoleId);

            modelBuilder.Entity<UserRole>().HasOne<User>().WithMany().HasForeignKey(e => e.UserId);

            modelBuilder.Entity<SysVar>();
            modelBuilder.Entity<SendOtp>();

            modelBuilder.Entity<Role>(entity =>
            {
                entity
                    .Property(e => e.Status)
                    .HasColumnName("Status")
                    .HasDefaultValue(RoleStatus.ACTIVE);
                entity
                    .Property(e => e.PermissionInWeb)
                    .HasColumnName("PermissionInWeb")
                    .HasDefaultValue(PermissionInWebs.Core);
            });

            modelBuilder
                .Entity<User>()
                .HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId);

            modelBuilder
                .Entity<Role>()
                .HasMany(e => e.UserRoles)
                .WithOne(e => e.Role)
                .HasForeignKey(e => e.RoleId);

            #region Partner

            modelBuilder
                .Entity<CorePartner>()
                .HasOne(x => x.PartnerType)
                .WithMany(x => x.Partners)
                .HasForeignKey(x => x.PartnerTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region Sku
            modelBuilder
                .Entity<CoreSkuSize>()
                .HasOne(x => x.Sku)
                .WithMany(x => x.SkuSizes)
                .HasForeignKey(x => x.SkuId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<CoreSkuSizePkgMockup>()
                .HasOne(x => x.SkuSize)
                .WithMany(x => x.PkgMockups)
                .HasForeignKey(x => x.SkuSizeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<CoreSku>()
                .HasOne(x => x.SkuBase)
                .WithMany(x => x.Skus)
                .HasForeignKey(x => x.SkuBaseId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder
                .Entity<CoreSku>()
                .HasOne(x => x.ProductionMethod)
                .WithMany(x => x.Skus)
                .HasForeignKey(x => x.ProductMethodId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder
                .Entity<CoreSku>()
                .HasOne(x => x.Material)
                .WithMany(x => x.Skus)
                .HasForeignKey(x => x.MaterialId)
                .OnDelete(DeleteBehavior.SetNull);

            #endregion

            #region order
            modelBuilder
                .Entity<CoreOrderDetail>()
                .HasOne(x => x.Order)
                .WithMany(x => x.OrderDetails)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<CoreOrderItem>()
                .HasOne(x => x.Order)
                .WithMany(x => x.OrderItems)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder
            //    .Entity<CoreOrderItem>()
            //    .HasOne(x => x.Sku)
            //    .WithMany(x => x.OrderItems)
            //    .HasForeignKey(x => x.SkuId)
            //    .OnDelete(DeleteBehavior.SetNull);


            modelBuilder
                .Entity<CoreOrderDetail>()
                .HasOne(x => x.Sku)
                .WithMany()
                .HasForeignKey(x => x.SkuId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder
                .Entity<CoreOrder>()
                .HasOne(x => x.Brand)
                .WithMany()
                .HasForeignKey(x => x.BrandId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder
                .Entity<CoreOrderItem>()
                .HasOne(x => x.OrderDetail)
                .WithMany()
                .HasForeignKey(x => x.OrderDetailId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<CoreFilePrint>()
                .HasOne(x => x.OrderItem)
                .WithOne(x => x.FilePrint)
                .HasForeignKey<CoreFilePrint>(x => x.OrderItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<CoreOrder>()
                .HasMany(o => o.Tags)
                .WithMany()
                .UsingEntity<CoreOrderTag>(
                    l =>
                        l.HasOne<CoreTag>()
                            .WithMany()
                            .HasForeignKey(t => t.TagId)
                            .OnDelete(DeleteBehavior.Restrict),
                    r =>
                        r.HasOne<CoreOrder>()
                            .WithMany()
                            .HasForeignKey(r => r.OrderId)
                            .OnDelete(DeleteBehavior.Restrict)
                );

            #endregion

            #region Brand

            modelBuilder
                .Entity<CoreStore>()
                .HasOne(x => x.Brand)
                .WithMany(x => x.Stores)
                .HasForeignKey(x => x.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion

            #region Batch
            modelBuilder
                .Entity<CoreBatch>()
                .HasOne(x => x.Partner)
                .WithMany(x => x.Batches)
                .HasForeignKey(x => x.PartnerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<CoreOrderItem>()
                .HasOne(x => x.Batch)
                .WithMany(x => x.OrderItems)
                .HasForeignKey(x => x.BatchId)
                .OnDelete(DeleteBehavior.SetNull);
            #endregion

            modelBuilder.SeedData();
            base.OnModelCreating(modelBuilder);
        }
    }
}
