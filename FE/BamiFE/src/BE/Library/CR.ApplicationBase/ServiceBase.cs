using CR.ApplicationBase.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace CR.ApplicationBase
{
    public abstract class ServiceBase<TDbContext> : ServiceAbstract
        where TDbContext : DbContext
    {
        protected readonly TDbContext _dbContext;
        protected readonly IMapErrorCode? _mapErrorCode;

        protected ServiceBase(ILogger logger, IHttpContextAccessor httpContext)
            : base(logger, httpContext)
        {
            _dbContext = httpContext.HttpContext!.RequestServices.GetRequiredService<TDbContext>();
        }

        protected ServiceBase(
            ILogger logger,
            IHttpContextAccessor httpContext,
            TDbContext dbContext,
            ILocalization localization
        )
            : base(logger, httpContext, localization)
        {
            _dbContext = dbContext;
        }

        protected ServiceBase(
            ILogger logger,
            IHttpContextAccessor httpContext,
            ILocalization localization
        )
            : base(logger, httpContext, localization)
        {
            _dbContext = httpContext.HttpContext!.RequestServices.GetRequiredService<TDbContext>();
        }

        protected ServiceBase(
            ILogger logger,
            IMapErrorCode mapErrorCode,
            IHttpContextAccessor httpContext,
            TDbContext dbContext,
            ILocalization localization
        )
            : base(logger, httpContext, localization)
        {
            _dbContext = dbContext;
            _mapErrorCode = mapErrorCode;
        }

        protected IQueryable<Entity> GetEntities<Entity>(Expression<Func<Entity, bool>>? expression)
            where Entity : class
        {
            IQueryable<Entity> query = _dbContext.Set<Entity>();
            if (expression is not null)
            {
                query = query.Where(expression);
            }
            return query;
        }

        protected Entity? FindEntities<Entity>(Expression<Func<Entity, bool>> expression)
            where Entity : class
        {
            return _dbContext.Set<Entity>().FirstOrDefault(expression);
        }
    }
}
