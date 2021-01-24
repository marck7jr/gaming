using Marck7JR.Core.Extensions.Hosting;
using Marck7JR.Gaming.Web.EpicGames.Http;
using Marck7JR.Gaming.Web.EpicGames.Http.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Marck7JR.Gaming.Data.EpicGames
{
    [TestClass]
    [DeploymentItem("Http/_EpicGamesHttpClientOptions.json")]
    [DeploymentItem("Http/_OAuthTokenResponse.json")]
    public class EpicGamesLibraryBuilderTest : GameLibraryServiceTest<EpicGamesLibraryService, EpicGamesLibrary>
    {
        [AssemblyInitialize]
        public static void InitializeAssembly(TestContext? testContext)
        {
            EpicGamesHttpClientTest.InitializeAssembly(testContext);

            Host.GetHostBuilder()
                .ConfigureAppConfiguration((hostBuilderContext, configuration) =>
                {
                    configuration.AddJsonFile($"_{nameof(OAuthTokenResponse)}.json");
                })
                .ConfigureServices((hostBuilderContext, services) =>
                {
                    services.AddOptions();
                    services.AddScoped<EpicGamesLibrary>();
                    services.AddSingleton<EpicGamesLibraryService>();
                    services.Configure<OAuthTokenResponse>(hostBuilderContext.Configuration);
                });
        }

        [ClassInitialize]
        public static void InitializeTestClass(TestContext? _)
        {
            Library = Host.GetHost().Services.GetRequiredService<EpicGamesLibrary>();
            Service = Host.GetHost().Services.GetRequiredService<EpicGamesLibraryService>();
        }
    }
}
