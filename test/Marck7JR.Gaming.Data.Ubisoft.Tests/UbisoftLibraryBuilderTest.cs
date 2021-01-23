using Marck7JR.Core.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Marck7JR.Gaming.Data.Ubisoft
{
    [TestClass]
    public class UbisoftLibraryBuilderTest : GameLibraryServiceTest<UbisoftLibraryService, UbisoftLibrary>
    {
        static UbisoftLibraryBuilderTest()
        {
            Host.GetHostBuilder().ConfigureServices((host, services) =>
            {
                services.AddSingleton<UbisoftLibrary>();
                services.AddSingleton<UbisoftLibraryService>();
            });
        }

        [ClassInitialize]
        public static void InitializeTestClass(TestContext _)
        {
            Library = Host.GetHost().Services.GetRequiredService<UbisoftLibrary>();
            Service = Host.GetHost().Services.GetRequiredService<UbisoftLibraryService>();
        }
    }
}
