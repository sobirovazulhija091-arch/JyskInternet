
using Infrastructure.Data;
namespace Test.HeplerDb;
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