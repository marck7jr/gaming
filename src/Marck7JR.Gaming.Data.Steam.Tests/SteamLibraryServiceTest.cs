using Marck7JR.Core.Extensions.Hosting;
using Marck7JR.Gaming.Web.Steam.Http;
using Marck7JR.Gaming.Web.Steam.Http.ISteamUser;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Marck7JR.Gaming.Data.Steam
{
    [TestClass]
    [DeploymentItem("_GetPlayerSummaries.json")]
    [DeploymentItem("Http/_SteamHttpClientOptions.json")]
    public class SteamLibraryServiceTest : GameLibraryServiceTest<SteamLibraryService, SteamLibrary>
    {
        [AssemblyInitialize]
        public static void InitializeAssembly(TestContext? testContext)
        {
            SteamHttpClientTest.InitializeAssembly(testContext);

            HostBinder.GetHostBuilder()
                .ConfigureAppConfiguration((hostBuilderContext, configuration) =>
                {
                    configuration.AddJsonFile($"_{nameof(GetPlayerSummaries)}.json");
                })
                .ConfigureServices((hostBuilderContext, services) =>
                {
                    services.AddOptions();
                    services.AddScoped<SteamLibrary>();
                    services.AddSingleton<SteamLibraryService>();
                    services.Configure<GetPlayerSummaries>(hostBuilderContext.Configuration);
                });
        }

        [ClassInitialize]
        public static void InitializeTestClass(TestContext? _)
        {
            Library = HostBinder.GetHost().Services.GetRequiredService<SteamLibrary>();
            Service = HostBinder.GetHost().Services.GetRequiredService<SteamLibraryService>();
        }
    }
}
