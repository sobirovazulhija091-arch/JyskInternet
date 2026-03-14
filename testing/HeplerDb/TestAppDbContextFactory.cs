using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Test.HelperDb;
internal static class TestAppDbContextFactory
{
    public static ApplicationDbContext CreateContext(string databaseName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;

        return new ApplicationDbContext(options);
}
}