using Marck7JR.Core.Extensions;
using Marck7JR.Core.Extensions.Hosting;
using Marck7JR.Gaming.Web.Steam.Http.ISteamUser;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace Marck7JR.Gaming.Web.Steam.Http
{
    [TestClass]
    [DeploymentItem(JsonFilePath)]
    public class SteamHttpClientTest
    {
        public const string JsonFilePath = "Directory.Build.json";

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext? _)
        {
            HostBinder.GetHostBuilder()
                .ConfigureAppConfiguration((hostBuilderContext, configuration) =>
                {
                    configuration.AddJsonFile(JsonFilePath, true);
                })
                .ConfigureServices((hostBuilderContext, services) =>
                {
                    services.AddOptions();
                    services.AddHttpClient<SteamHttpClient>();
                    services.Configure<SteamHttpClientOptions>(hostBuilderContext.Configuration.GetSection(nameof(SteamHttpClientOptions)));
                });
        }

        private static SteamHttpClient? _httpClient;

        [ClassInitialize]
        public static void ClassInitialize(TestContext? _)
        {
            _httpClient = HostBinder.GetHost().Services.GetRequiredService<SteamHttpClient>();
        }

        public TestContext? TestContext { get; set; }

        [TestMethod]
        [DataRow("76561198252552650")]
        public async Task GetOwnedGamesAsync_IsNotNull(string steamId)
        {
            var getOwnedGames = await _httpClient!
                .FromIPlayerService()
                .GetOwnedGamesAsync(queries: new[] { "include_played_free_games=true", "include_appinfo=true", $"steamid={steamId}" });

            Assert.IsNotNull(getOwnedGames);

            foreach (var game in getOwnedGames!.response.games)
            {
                TestContext!.WriteLine($"[{game.appid}] {game.name}");
            }
        }

        [TestMethod]
        [DataRow("76561198252552650")]
        public async Task GetPlayerSummaries_IsNotNull(params string[] steamids)
        {
            var getPlayerSummaries = await _httpClient!
                .FromISteamUser()
                .GetGetPlayerSummariesAsync(queries: new[] { $"steamids={string.Join(',', steamids)}" });

            var json = await getPlayerSummaries!.ToJsonAsync();

            Assert.IsNotNull(getPlayerSummaries);
            Assert.IsTrue(getPlayerSummaries?.response.players.player.FirstOrDefault() is GetPlayerSummaries.Player);
            Assert.IsTrue(json.IsNotNullOrEmpty());

            TestContext?.WriteLine(getPlayerSummaries!.ToJson());
        }
    }
}
