using Marck7JR.Core.Extensions.Hosting;
using Marck7JR.Gaming.Data.Contracts;
using Marck7JR.Gaming.Data.Extensions;
using Marck7JR.Gaming.Web.Steam.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Marck7JR.Gaming.Data.Steam
{
    [TestClass]
    public class SteamLibraryServiceTest : GameLibraryServiceTest<SteamLibraryService, SteamLibrary>
    {
        [AssemblyInitialize]
        public static void InitializeAssembly(TestContext? testContext)
        {
            SteamHttpClientTest.InitializeAssembly(testContext);

            HostBinder.GetHostBuilder()
                .ConfigureServices((hostBuilderContext, services) =>
                {
                    services.AddOptions();
                    services.AddGameLibraryService<SteamLibraryService>();
                });
        }

        [ClassInitialize]
        public static void InitializeTestClass(TestContext? _)
        {
            Library = HostBinder.GetHost().Services.GetRequiredService<IGameLibraryFactory>().GetGameLibrary<SteamLibrary>();
            Service = HostBinder.GetHost().Services.GetRequiredService<IGameLibraryServiceFactory>().GetGameLibraryService<SteamLibraryService>();
        }
    }
}
