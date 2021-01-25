using Marck7JR.Core.Extensions.Hosting;
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
                services.AddSingleton<UbisoftLibrary>();
                services.AddSingleton<UbisoftLibraryService>();
            });
        }

        [ClassInitialize]
        public static void InitializeTestClass(TestContext _)
        {
            Library = HostBinder.GetHost().Services.GetRequiredService<UbisoftLibrary>();
            Service = HostBinder.GetHost().Services.GetRequiredService<UbisoftLibraryService>();
        }
    }
}
