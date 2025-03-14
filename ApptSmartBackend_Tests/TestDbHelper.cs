using ApptSmartBackend.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public class TestDbHelper
{
    private SqliteConnection _connection;
    public DbContextOptions<AppDbContext> ContextOptions { get; private set; }
    [SetUp]
    public virtual void SetUp()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        ContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .UseLazyLoadingProxies()
            .Options;

        using (var context = CreateContext())
        {
            context.Database.EnsureCreated();
            SeedDatabase(context);
        }

    }

    [TearDown]
    public void TearDown()
    {
        _connection.Close();
    }

    protected AppDbContext CreateContext()
    {
        return new AppDbContext(ContextOptions);
    }

    protected virtual void SeedDatabase(AppDbContext context) { }
}