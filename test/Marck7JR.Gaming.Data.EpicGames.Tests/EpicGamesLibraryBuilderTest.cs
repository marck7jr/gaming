using Marck7JR.Core.Extensions.Hosting;
using Marck7JR.Gaming.Data.Contracts;
using Marck7JR.Gaming.Data.Extensions;
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

            HostBinder.GetHostBuilder()
                .ConfigureAppConfiguration((hostBuilderContext, configuration) =>
                {
                    configuration.AddJsonFile($"_{nameof(OAuthTokenResponse)}.json");
                })
                .ConfigureServices((hostBuilderContext, services) =>
                {
                    services.AddOptions();
                    services.AddGameLibraryService<EpicGamesLibraryService>();
                    services.Configure<OAuthTokenResponse>(hostBuilderContext.Configuration);
                });
        }

        [ClassInitialize]
        public static void InitializeTestClass(TestContext? _)
        {
            Library = HostBinder.GetHost().Services.GetRequiredService<IGameLibraryFactory>().GetGameLibrary<EpicGamesLibrary>();
            Service = HostBinder.GetHost().Services.GetRequiredService<IGameLibraryServiceFactory>().GetGameLibraryService<EpicGamesLibraryService>();
        }
    }
}
