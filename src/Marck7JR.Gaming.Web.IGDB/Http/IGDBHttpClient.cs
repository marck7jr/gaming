using Google.Protobuf;
using Marck7JR.Gaming.Web.Twitch.Http;
using Marck7JR.Gaming.Web.Twitch.Http.Infrastructure;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Marck7JR.Gaming.Web.IGDB.Http
{
    public class IGDBHttpClient : APICalypseQueryBuilder, IIGDBHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly TwitchOAuthResponse _oauth;
        private readonly TwitchHttpClientOptions _options;

        public IGDBHttpClient(HttpClient httpClient, IOptions<TwitchHttpClientOptions> options, IOptions<TwitchOAuthResponse> oauth)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _oauth = oauth.Value;

            _httpClient.BaseAddress = new Uri("https://api.igdb.com/v4/");
            _httpClient.DefaultRequestHeaders.Add("Client-Id", _options.ClientId);
        }

        public override async Task<T?> QueryAsync<T>(string requestUri, CancellationToken cancellationToken = default)
            where T : class
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _oauth.access_token);

                var stringContent = new StringContent(Query);
                var response = await _httpClient.PostAsync(requestUri, stringContent, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var bytes = await response.Content.ReadAsByteArrayAsync();
                    var parser = typeof(T).GetProperty("Parser", BindingFlags.Public | BindingFlags.Static).GetValue(null, null) as MessageParser;
                    var t = parser!.ParseFrom(bytes);

                    return (T)t;
                }

                throw new InvalidOperationException($"{response}\n{response.Content.ReadAsStringAsync()}");
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
            finally
            {
                KeyValuePairs.Clear();
            }

            return default;
        }
    }
}
