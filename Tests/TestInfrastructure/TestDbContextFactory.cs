using DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace Tests.TestInfrastructure
{
    internal static class TestDbContextFactory
    {
        public static MyDbContext Create(string? dbName = null)
        {
            dbName ??= Guid.NewGuid().ToString("N");

            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseLazyLoadingProxies()
                .UseInMemoryDatabase(dbName)
                .Options;

            return new MyDbContext(options);
        }
    }
}
