using Marck7JR.Core.Extensions;
using Marck7JR.Core.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;

namespace Marck7JR.Gaming.Web.Steam.Http
{
    [TestClass]
    public class SteamHttpClientTest
    {
        [AssemblyInitialize]
        public static void InitializeAssembly(TestContext? _)
        {
            HostBinder.GetHostBuilder()
                .ConfigureAppConfiguration((hostBuilderContext, configuration) =>
                {
                    configuration.AddUserSecrets<SteamHttpClientTest>();
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
        public static void InitializeTestClass(TestContext? _)
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

            Assert.IsNotNull(getPlayerSummaries);

            var json = await getPlayerSummaries!.ToJsonAsync();

            await File.WriteAllTextAsync("Http/_GetPlayerSummaries.json", json);

            TestContext?.WriteLine(getPlayerSummaries!.ToJson());
        }
    }
}
