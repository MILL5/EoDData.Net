using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EoDData.Net.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class TestManager
    {
        public static ILogger Logger { get; private set; }

        public static IConfiguration Configuration { get; private set; }

        public static IEoDDataDependencies Dependencies { get; private set; }

        public static IEoDDataClient TestClient { get; private set; }

        protected TestManager()
        {
        }

        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .Build();

            var services = new ServiceCollection();

            services.AddSingleton(Configuration);
            services.AddApplication(Configuration);

            var serviceProvider = services.BuildServiceProvider();


            TestClient = serviceProvider.GetService<IEoDDataClient>();
            Dependencies = serviceProvider.GetService<IEoDDataDependencies>();
        }

        public static void AssertAllPropertiesNotNull<T>(T obj, List<string> ignores = null)
        {
            foreach (var prop in obj.GetType().GetProperties())
            {
                if (ignores != null && ignores.Contains(prop.Name))
                {
                    continue;
                }

                var value = prop.GetValue(obj);

                Assert.IsNotNull(value, $"{ prop.Name } is null.");

                if (value is string)
                {
                    Assert.AreNotEqual(string.Empty, value, $"{ prop.Name } is an empty string.");
                }
            }
        }
    }
}