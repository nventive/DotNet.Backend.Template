using System;
using System.Threading.Tasks;
using MartinCostello.Logging.XUnit;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace NV.Templates.Backend.Core.Tests
{
    /// <summary>
    /// Helper for <see cref="CoreDbContext"/> testing.
    /// </summary>
    internal static class CoreDbContextTester
    {
        /// <summary>
        /// Encapsulate the execution of a unit-test in a pristine <see cref="CoreDbContext"/>.
        /// </summary>
        /// <param name="testOutputHelper">The <see cref="ITestOutputHelper"/> for test output support.</param>
        /// <param name="execution">The test to execute.</param>
        /// <example>
        /// public class MyTestClass
        /// {
        ///     private readonly ITestOutputHelper _testOutputHelper;
        ///
        ///     public MyTestClass(ITestOutputHelper testOutputHelper)
        ///     {
        ///         _testOutputHelper = testOutputHelper;
        ///     }
        ///
        ///     public async Task ItShould()
        ///     {
        ///         await CoreDbContextHelper.Execute(_testOutputHelper, async createContext =>
        ///         {
        ///             var dbContext = createContext();
        ///             // ....
        ///
        ///             // You can then create a NEW DbContext with the same dataset.
        ///             // This is useful to validate that the data is actually committed properly.
        ///             dbContext = createContext();
        ///         });
        ///     }
        /// }.
        /// </example>
        public static async Task Execute(ITestOutputHelper testOutputHelper, Func<Func<CoreDbContext>, Task> execution)
        {
            var connection = new SqliteConnection("DataSource=:memory:");

            connection.Open();

            var dbContextOptions = new DbContextOptionsBuilder<CoreDbContext>()
                .UseSqlite(connection)
                .UseLoggerFactory(
                new LoggerFactory(
                    new[]
                    {
                        new XUnitLoggerProvider(
                            testOutputHelper,
                            new XUnitLoggerOptions
                            {
                                Filter = (category, logLevel) => logLevel >= LogLevel.Information,
                            }),
                    }))
                .EnableDetailedErrors(true)
                .EnableSensitiveDataLogging(true)
                .Options;

            CoreDbContext DbContextCreator()
            {
                var context = new CoreDbContext(dbContextOptions);

                context.Database.EnsureCreated();

                return context;
            }

            await execution(DbContextCreator);

            connection.Close();
        }
    }
}
