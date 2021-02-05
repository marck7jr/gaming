using Marck7JR.Core.Extensions;
using Marck7JR.Core.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Marck7JR.Gaming.Web.EpicGames.Http
{
    [TestClass]
    [DeploymentItem(JsonFilePath)]
    public class EpicGamesHttpClientTest
    {
        public const string JsonFilePath = "Directory.Build.json";

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext? _)
        {
            HostBinder.GetHostBuilder()
                .ConfigureAppConfiguration((context, configuration) =>
                {
                    configuration.AddJsonFile(JsonFilePath, true);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddOptions();
                    services.AddHttpClient<EpicGamesHttpClient>().ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { CookieContainer = new(), UseCookies = true });
                    services.Configure<EpicGamesHttpClientOptions>(context.Configuration.GetSection(nameof(EpicGamesHttpClientOptions)));
                });
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext _)
        {
            _httpClient = HostBinder.GetHost().Services.GetRequiredService<EpicGamesHttpClient>();
        }

        private static EpicGamesHttpClient? _httpClient;
        public TestContext? TestContext { get; set; }

        [TestMethod]
        public async Task GetOAuthTokenResponseAsync_WithSetSidResponse_IsNotNull()
        {
            var oAuthTokenResponse = await _httpClient!.GetOAuthTokenResponseAsync();
            var json = await oAuthTokenResponse!.ToJsonAsync();

            Assert.IsNotNull(oAuthTokenResponse);
            Assert.IsTrue(json.IsNotNullOrEmpty());

            TestContext?.WriteLine(json);
        }

        [TestMethod]
        public async Task GetOAuthTokenResponseAsync_WithOAuthTokenResponse_IsNotNull()
        {
            var oAuthTokenResponse = await _httpClient!.GetOAuthTokenResponseAsync();
            var json = await oAuthTokenResponse!.ToJsonAsync();

            Assert.IsNotNull(oAuthTokenResponse);
            Assert.IsTrue(json.IsNotNullOrEmpty());

            TestContext?.WriteLine(json);
        }


        [TestMethod]
        public async Task GetCatalogResponsesAsync_IsNotNull()
        {
            var assetsResponseItems = await _httpClient!.GetAssetsResponsesAsync().ToListAsync();

            Assert.IsNotNull(assetsResponseItems);

            foreach (var catalogResponse in await _httpClient!.GetCatalogResponsesAsync(assetsResponseItems).ToListAsync())
            {
                TestContext?.Write(catalogResponse!.ToJson());
            }
        }
    }
}
