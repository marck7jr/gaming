using Marck7JR.Core.Extensions;
using Marck7JR.Gaming.Web.Steam.Http.IPlayerService;
using Marck7JR.Gaming.Web.Steam.Http.ISteamUser;
using Marck7JR.Gaming.Web.Steam.Http.ISteamUserStats;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Marck7JR.Gaming.Web.Steam.Http
{
    public class SteamHttpClient : ISteamHttpClient
    {
        private const string PathFormat = "/{0}";
        private const string QueryFormat = "&{0}";

        private readonly HttpClient _httpClient;
        private readonly SteamHttpClientOptions _options;
        private readonly StringBuilder stringBuilder = new();

        public SteamHttpClient(HttpClient httpClient, IOptions<SteamHttpClientOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;

            _httpClient.BaseAddress = new Uri("http://api.steampowered.com");
        }

        private async Task<T?> GetAsync<T>(string? version = null, params string[] queries)
            where T : class
        {
            stringBuilder.AppendFormat(PathFormat, typeof(T).Name);
            stringBuilder.AppendFormat(PathFormat, version);

            stringBuilder.Append($"/?key={_options.WebApiKey}");

            foreach (var query in queries)
            {
                stringBuilder.AppendFormat(QueryFormat, query);
            }

            try
            {
                var response = await _httpClient.GetAsync(Query);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return json.FromJson<T>();
                }

                throw new InvalidOperationException($"{response}\n{await response.Content.ReadAsStringAsync()}");
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }

            return default;
        }

        public string Query => stringBuilder.ToString();

        private T From<T>(T t)
        {
            stringBuilder.AppendFormat(PathFormat, typeof(T).GetCustomAttribute<DescriptionAttribute>().Description);
            return t;
        }

        public IPlayerServiceResponseBuilder FromIPlayerService() => From<IPlayerServiceResponseBuilder>(this);
        public ISteamUserResponseBuilder FromISteamUser() => From<ISteamUserResponseBuilder>(this);
        public ISteamUserStatsResponseBuilder FromISteamUserStats() => From<ISteamUserStatsResponseBuilder>(this);

        public async Task<GetOwnedGames?> GetOwnedGamesAsync(string? version = "v0001", params string[] queries) => await GetAsync<GetOwnedGames>(version, queries);
        public async Task<GetPlayerSummaries?> GetGetPlayerSummariesAsync(string? version = "v0001", params string[] queries) => await GetAsync<GetPlayerSummaries>(version, queries);
        public async Task<GetSchemaForGame?> GetGetSchemaForGameAsync(string? version = "v2", params string[] queries) => await GetAsync<GetSchemaForGame>(version, queries);
    }
}
