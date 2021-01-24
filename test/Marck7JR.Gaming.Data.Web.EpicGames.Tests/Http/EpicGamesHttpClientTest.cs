using Marck7JR.Core.Extensions;
using Marck7JR.Core.Extensions.Hosting;
using Marck7JR.Gaming.Web.EpicGames.Http.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Marck7JR.Gaming.Web.EpicGames.Http
{
    [TestClass]
    [DeploymentItem("Http/_EpicGamesHttpClientOptions.json")]
    [DeploymentItem("Http/_OAuthTokenResponse.json")]
    [DeploymentItem("Http/_SetSidResponse.json")]
    public class EpicGamesHttpClientTest
    {
        [AssemblyInitialize]
        public static void InitializeAssembly(TestContext? _)
        {
            Host.GetHostBuilder()
                .ConfigureAppConfiguration((hostBuilderContext, configuration) =>
                {
                    configuration.AddJsonFile($"_{nameof(EpicGamesHttpClientOptions)}.json");
                })
                .ConfigureServices((hostBuilderContext, services) =>
                {
                    services.AddOptions();
                    services.AddHttpClient<EpicGamesHttpClient>().ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { CookieContainer = new(), UseCookies = true });
                    services.Configure<EpicGamesHttpClientOptions>(hostBuilderContext.Configuration);
                });
        }

        [ClassInitialize]
        public static void InitializeTestClass(TestContext _)
        {
            _httpClient = Host.GetHost().Services.GetRequiredService<EpicGamesHttpClient>();
        }

        private static EpicGamesHttpClient? _httpClient;
        public TestContext? TestContext { get; set; }

        [TestMethod]
        public async Task GetOAuthTokenResponseAsync_WithSetSidResponse_IsNotNull()
        {
            var setSidResponse = File.ReadAllText($"_{nameof(SetSidResponse)}.json").FromJson<SetSidResponse>();

            Assert.IsNotNull(setSidResponse);

            var oAuthTokenResponse = await _httpClient!.GetOAuthTokenResponseAsync(setSidResponse);

            Assert.IsNotNull(oAuthTokenResponse);

            TestContext?.Write(oAuthTokenResponse!.ToJson());
        }

        [TestMethod]
        public async Task GetOAuthTokenResponseAsync_WithOAuthTokenResponse_IsNotNull()
        {
            var oAuthTokenResponse = File.ReadAllText($"_{nameof(OAuthTokenResponse)}.json").FromJson<OAuthTokenResponse>();

            Assert.IsNotNull(oAuthTokenResponse);

            oAuthTokenResponse = await _httpClient!.GetOAuthTokenResponseAsync(oAuthTokenResponse);

            Assert.IsNotNull(oAuthTokenResponse);

            TestContext?.WriteLine(oAuthTokenResponse!.ToJson());
        }


        [TestMethod]
        public async Task GetCatalogResponsesAsync_IsNotNull()
        {
            var oAuthTokenResponse = File.ReadAllText($"_{nameof(OAuthTokenResponse)}.json").FromJson<OAuthTokenResponse>();

            var assetsResponseItems = await _httpClient!.GetAssetsResponsesAsync(oAuthTokenResponse).ToListAsync();

            Assert.IsNotNull(assetsResponseItems);

            foreach (var catalogResponse in await _httpClient!.GetCatalogResponsesAsync(assetsResponseItems).ToListAsync())
            {
                TestContext?.Write(catalogResponse!.ToJson());
            }
        }
    }
}
