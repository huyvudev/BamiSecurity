using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CR.Core.Infrastructure.Persistence
{
    [Obsolete]
    public class CoreDbContextTransient : CoreDbContext
    {
        public CoreDbContextTransient(
            DbContextOptions<CoreDbContext> options,
            IHttpContextAccessor httpContextAccessor
        )
            : base(options, httpContextAccessor) { }
    }
}
