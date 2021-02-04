using Marck7JR.Core.Extensions.Hosting;
using Marck7JR.Gaming.Data.Contracts;
using Marck7JR.Gaming.Data.Extensions;
using Marck7JR.Gaming.Web.EpicGames.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Marck7JR.Gaming.Data.EpicGames
{
    [TestClass]
    public class EpicGamesLibraryBuilderTest : GameLibraryServiceTest<EpicGamesLibraryService, EpicGamesLibrary>
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext? testContext)
        {
            EpicGamesHttpClientTest.AssemblyInitialize(testContext);

            HostBinder.GetHostBuilder()
                .ConfigureServices((hostBuilderContext, services) =>
                {
                    services.AddOptions();
                    services.AddGameLibraryService<EpicGamesLibraryService>();
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
