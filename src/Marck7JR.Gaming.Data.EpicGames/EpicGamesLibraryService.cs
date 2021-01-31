using Marck7JR.Gaming.Data.Contracts;
using Marck7JR.Gaming.Data.EpicGames.Extensions;
using Marck7JR.Gaming.Data.EpicGames.Infrastructure;
using Marck7JR.Gaming.Web.EpicGames.Http;
using Marck7JR.Gaming.Web.EpicGames.Http.Infrastructure;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marck7JR.Gaming.Data.EpicGames
{
    public class EpicGamesLibraryService : GameLibraryService<EpicGamesLibrary>
    {
        private readonly EpicGamesHttpClient _httpClient;
        private readonly OAuthTokenResponse _options;

        public EpicGamesLibraryService(EpicGamesLibrary? library, EpicGamesHttpClient httpClient, IOptions<OAuthTokenResponse> options) : base(library)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public EpicGamesLibraryService(IGameLibraryFactory gameLibraryFactory, EpicGamesHttpClient httpClient, IOptions<OAuthTokenResponse> options) : base(gameLibraryFactory)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public override Func<EpicGamesLibrary, IAsyncEnumerable<GameApplication>>? GetApplicationsOfflineAsync => GetGameApplicationsOfflineAsync;
        public override Func<EpicGamesLibrary, IAsyncEnumerable<GameApplication>>? GetApplicationsOnlineAsync => GetGameApplicationsOnlineAsync;

        private async IAsyncEnumerable<GameApplication> GetGameApplicationsOfflineAsync(EpicGamesLibrary library)
        {
            if (library is IGameLibrary { IsAvailable: true } && library.GetManifests() is IEnumerable<EpicGamesApplicationManifest> applicationManifests)
            {
                var items = applicationManifests
                    .Where(manifest => !string.IsNullOrEmpty(manifest.LaunchExecutable))
                    .GroupBy(manifest => manifest.AppName)
                    .Select(group => group.Last());

                await Task.CompletedTask;

                foreach (var manifest in items)
                {
                    yield return new(library, manifest)
                    {
                        IsInstalled = true,
                        AppId = manifest.AppName,
                        DisplayName = manifest.DisplayName ?? throw new ArgumentNullException(nameof(GameApplication.DisplayName)),
                    };
                }
            }
        }

        private async IAsyncEnumerable<GameApplication> GetGameApplicationsOnlineAsync(EpicGamesLibrary library)
        {
            if (_httpClient is not null)
            {
                var assetsResponses = await _httpClient.GetAssetsResponsesAsync(_options).ToListAsync();
                var catalogResponses = await _httpClient.GetCatalogResponsesAsync(assetsResponses)
                    .Where(catalog => !catalog.categories.Any(_ => _.path.Equals("dlc", StringComparison.InvariantCultureIgnoreCase)))
                    .GroupBy(catalog => catalog.id)
                    .SelectAwait(async group => await group.LastAsync())
                    .ToListAsync();

                foreach (var catalog in catalogResponses)
                {
                    yield return new(library, catalog)
                    {
                        AppId = catalog.releaseInfo.FirstOrDefault().appId,
                        DisplayName = catalog.title ?? throw new ArgumentNullException(nameof(GameApplication.DisplayName)),
                    };
                }
            }
        }
    }
}
