using Marck7JR.Core.Extensions;
using Marck7JR.Core.Extensions.Hosting;
using Marck7JR.Gaming.Web.IGDB.Http;
using Marck7JR.Gaming.Web.IGDB.Http.Messages;
using Marck7JR.Gaming.Web.Twitch.Http;
using Marck7JR.Gaming.Web.Twitch.Http.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Marck7JR.Gaming.Web.IGDB.Tests.Http
{
    [TestClass]
    [DeploymentItem("Http/_TwitchHttpClientOptions.json")]
    [DeploymentItem("Http/_TwitchOAuthResponse.json")]
    public class IGDBHttpClientTest
    {
        private static IGDBHttpClient? _httpClient;

        static IGDBHttpClientTest()
        {
            HostBinder.GetHostBuilder()
                .ConfigureAppConfiguration((hostBuilderContext, configuration) =>
                {
                    configuration.AddJsonFile($"_{nameof(TwitchHttpClientOptions)}.json");
                    configuration.AddJsonFile($"_{nameof(TwitchOAuthResponse)}.json");
                })
                .ConfigureServices((hostBuilderContext, services) =>
                {
                    services.AddOptions();
                    services.AddHttpClient<IGDBHttpClient>();
                    services.Configure<TwitchHttpClientOptions>(hostBuilderContext.Configuration);
                    services.Configure<TwitchOAuthResponse>(hostBuilderContext.Configuration);
                });
        }

        public TestContext? TestContext { get; set; }

        [ClassInitialize]
        public static void InitializeTestClass(TestContext? _)
        {
            _httpClient = HostBinder.GetHost().Services.GetRequiredService<IGDBHttpClient>();
        }

        [TestMethod]
        [DataRow("Assassins Creed Valhalla")]
        [DataRow("Hades")]
        [DataRow("Cyberpunk 2077")]
        public async Task GetGames_Search_IsNotNull(object args)
        {
            var gameResult = await _httpClient!
                .Fields("*")
                .Search(args)
                .Where("platforms = (6)", "version_parent = null")
                .QueryAsync<GameResult>(IGDBHttpClientEndpoint.Games);

            Assert.IsNotNull(gameResult);

            TestContext?.Write(gameResult!.ToJson());
        }
    }
}
