using Marck7JR.Core.Extensions;
using Marck7JR.Core.Extensions.Hosting;
using Marck7JR.Gaming.Web.IGDB.Http;
using Marck7JR.Gaming.Web.IGDB.Http.Messages;
using Marck7JR.Gaming.Web.Twitch.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Marck7JR.Gaming.Web.IGDB.Tests.Http
{
    [TestClass]
    [DeploymentItem(JsonFilePath)]
    public class IGDBHttpClientTest
    {
        public const string JsonFilePath = "Directory.Build.json";

        private static IGDBHttpClient? _httpClient;

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext? _)
        {
            HostBinder.GetHostBuilder()
                .ConfigureAppConfiguration((hostBuilderContext, configuration) =>
                {
                    configuration.AddJsonFile(JsonFilePath);
                })
                .ConfigureServices((hostBuilderContext, services) =>
                {
                    services.AddOptions();
                    services.AddHttpClient<IGDBHttpClient>();
                    services.AddHttpClient<TwitchHttpClient>();
                    services.Configure<IGDBHttpClientOptions>(hostBuilderContext.Configuration.GetSection(nameof(IGDBHttpClientOptions)));
                    services.Configure<TwitchHttpClientOptions>(hostBuilderContext.Configuration.GetSection(nameof(TwitchHttpClientOptions)));
                });
        }

        public TestContext? TestContext { get; set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext? _)
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
