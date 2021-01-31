using Marck7JR.Core.Extensions.Hosting;
using Marck7JR.Gaming.Data.Contracts;
using Marck7JR.Gaming.Data.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Marck7JR.Gaming.Data.Ubisoft
{
    [TestClass]
    public class UbisoftLibraryServiceTest : GameLibraryServiceTest<UbisoftLibraryService, UbisoftLibrary>
    {
        static UbisoftLibraryServiceTest()
        {
            HostBinder.GetHostBuilder().ConfigureServices((host, services) =>
            {
                services.AddGameLibraryService<UbisoftLibraryService>();
            });
        }

        [ClassInitialize]
        public static void InitializeTestClass(TestContext _)
        {
            Library = HostBinder.GetHost().Services.GetRequiredService<IGameLibraryFactory>().GetGameLibrary<UbisoftLibrary>();
            Service = HostBinder.GetHost().Services.GetRequiredService<IGameLibraryServiceFactory>().GetGameLibraryService<UbisoftLibraryService>();
        }
    }
}
