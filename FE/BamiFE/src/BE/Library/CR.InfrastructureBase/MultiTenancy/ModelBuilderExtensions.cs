using System.Linq.Expressions;
using CR.EntitiesBase.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CR.InfrastructureBase.MultiTenancy
{
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Lọc các bảng theo tenantId
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void AddTenantQueryFilter(this ModelBuilder modelBuilder)
        {
            foreach (
                var entityType in modelBuilder
                    .Model.GetEntityTypes()
                    .Where(entityType => typeof(IMultiTenancy).IsAssignableFrom(entityType.ClrType))
            )
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var filter = Expression.Lambda(
                    Expression.Equal(
                        Expression.Property(parameter, nameof(IMultiTenancy.TenantId)),
                        Expression.Constant(0)
                    ),
                    parameter
                );
                entityType.SetQueryFilter(filter);
            }
        }
    }
}
