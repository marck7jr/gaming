using Marck7JR.Core.Extensions;
using Marck7JR.Gaming.Data.Contracts;
using Marck7JR.Gaming.Web.Steam.Http;
using Marck7JR.Gaming.Web.Steam.Http.ISteamUser;
using Microsoft.Extensions.Options;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marck7JR.Gaming.Data.Steam
{
    public class SteamLibraryService : GameLibraryService<SteamLibrary>
    {
        private readonly SteamHttpClient _httpClient;
        private readonly GetPlayerSummaries _options;

        public SteamLibraryService(SteamLibrary library, SteamHttpClient httpClient, IOptions<GetPlayerSummaries> options) : base(library)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public SteamLibraryService(IGameLibraryFactory gameLibraryFactory, SteamHttpClient httpClient, IOptions<GetPlayerSummaries> options) : base(gameLibraryFactory)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public override Func<SteamLibrary, IAsyncEnumerable<GameApplication>>? GetApplicationsOfflineAsync => GetGameApplicationOfflineAsync;
        public override Func<SteamLibrary, IAsyncEnumerable<GameApplication>>? GetApplicationsOnlineAsync => GetGameApplicationOnlineAsync;

        private async IAsyncEnumerable<GameApplication> GetGameApplicationOfflineAsync(SteamLibrary library)
        {
            if (library is IGameLibrary { IsAvailable: true } && library.RegistryKey?.OpenSubKey("Apps") is RegistryKey appsKey)
            {
                var appsSubKeyNames = appsKey.GetSubKeyNames();

                var keys = appsSubKeyNames
                    .Select(names => new
                    {
                        appId = names,
                        name = appsKey.OpenSubKey(names)!.GetValue("Name")?.ToString() ?? string.Empty,
                        installed = appsKey.OpenSubKey(names)!.GetValue("Installed") as int?,
                    })
                    .Where(x => x.name.IsNotNullOrEmpty() && x.installed.GetValueOrDefault() == 0x1)
                    .GroupBy(x => x.appId)
                    .Select(group => group.Last());

                await Task.CompletedTask;

                foreach (var key in keys)
                {
                    yield return new(library, key)
                    {
                        AppId = $"{key.appId}",
                        IsInstalled = true,
                        DisplayName = key.name ?? throw new ArgumentNullException(nameof(GameApplication.DisplayName)),
                    };
                }
            }
        }

        private async IAsyncEnumerable<GameApplication> GetGameApplicationOnlineAsync(SteamLibrary library)
        {
            var getOwnedGames = await _httpClient
                    .FromIPlayerService()
                    .GetOwnedGamesAsync(queries: new[] { "include_played_free_games=true", "include_appinfo=true", $"steamid={_options.response.players.player.First().steamid}" });

            if (getOwnedGames is not null)
            {
                var games = getOwnedGames.response.games
                    .GroupBy(game => game.appid)
                    .Select(group => group.Last());

                foreach (var game in games)
                {
                    yield return new(library, game)
                    {
                        AppId = $"{game.appid}",
                        DisplayName = game.name ?? throw new ArgumentNullException(nameof(GameApplication.DisplayName)),
                    };
                }
            }
        }
    }
}
