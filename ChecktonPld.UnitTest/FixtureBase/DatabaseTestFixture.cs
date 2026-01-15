using Microsoft.EntityFrameworkCore;
using ChecktonPld.Funcionalidad;
using ChecktonPld.DOM.ApplicationDbContext;
using ChecktonPld.RestAPI;

namespace ChecktonPld.UnitTest.FixtureBase
{
    [Collection("FunctionalCollection")]
    public class DatabaseTestFixture : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        protected readonly CustomWebApplicationFactory<Program> Factory;

        private readonly string _connectionString = EmServiceCollectionExtensions.GetConnectionString();

        public DatabaseTestFixture()
        {
            SetEnvironmentalVariables();
            var factory = new CustomWebApplicationFactory<Program>();
            Factory = factory;
         
            using var context = CreateContext();
            context.Database.EnsureDeleted();
            context.Database.Migrate();
        }

        protected async Task SetupDataAsync(Func<ServiceDbContext, Task> setupDataAction)
        {
            await using var context = CreateContext();
            await setupDataAction(context);
        }

        protected internal ServiceDbContext CreateContext()
            => new(
                new DbContextOptionsBuilder<ServiceDbContext>()
                    .UseSqlServer(_connectionString,
                        optionsBuilder => optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
                    .Options);
        
        private void SetEnvironmentalVariables()
        {
            Environment.SetEnvironmentVariable(
                "dbConnectionString",
                _connectionString);
            Environment.SetEnvironmentVariable(
                "api-key-checkton",
                "ak_9b4a4e4ba8ea5aa18bfca2f983c7ce147f744bfb6e5ee8202f0c9567f7a0ed0c");
            Environment.SetEnvironmentVariable(
                "checkton-pld-base-uri",
                "https://api-pld.checkton.com.mx");
        }
    }
}