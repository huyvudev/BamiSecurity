using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace CR.TestBase
{
    public abstract class UnitTestBase<TDbContext>
        where TDbContext : DbContext, new()
    {
        protected UnitTestBase() { }

        protected DbContextOptions<TDbContext> CreateDbContextOptions(string databaseName)
        {
            return new DbContextOptionsBuilder<TDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
        }

        protected ILogger<TService> CreateLogger<TService>()
            where TService : class
        {
            return new Mock<ILogger<TService>>().Object;
        }

        /// <summary>
        /// Refresh lại dữ liệu
        /// </summary>
        protected abstract void Refresh();
    }
}
