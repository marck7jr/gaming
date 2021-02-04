using Google.Protobuf;
using Marck7JR.Gaming.Web.Twitch.Http;
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
        private readonly IGDBHttpClientOptions _options;
        private readonly TwitchHttpClient _twitchHttpClient;
        private readonly TwitchHttpClientOptions _twitchHttpClientOptions;

        public IGDBHttpClient(HttpClient httpClient, TwitchHttpClient twitchHttpClient, IOptions<IGDBHttpClientOptions> options, IOptions<TwitchHttpClientOptions> twitchHttpClientOptions)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _twitchHttpClient = twitchHttpClient;
            _twitchHttpClientOptions = twitchHttpClientOptions.Value;

            _httpClient.BaseAddress = new Uri("https://api.igdb.com/v4/");
            _httpClient.DefaultRequestHeaders.Add("Client-Id", _twitchHttpClientOptions.ClientId);
        }

        public override async Task<T?> QueryAsync<T>(string requestUri, CancellationToken cancellationToken = default)
            where T : class
        {
            try
            {
                if (_options.TwitchOAuthResponse is null || _options.TwitchOAuthResponse?.expires_at < DateTime.UtcNow)
                {
                    _options.TwitchOAuthResponse = await _twitchHttpClient.GetOAuthResponseAsync();
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _options.TwitchOAuthResponse?.access_token);

                var stringContent = new StringContent(Query);
                var response = await _httpClient.PostAsync(requestUri, stringContent, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    byte[]? bytes = null;

#if NET5_0_OR_GREATER
                    bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);
#else
                    bytes = await response.Content.ReadAsByteArrayAsync();
#endif
                    var parser = typeof(T).GetProperty("Parser", BindingFlags.Public | BindingFlags.Static)?.GetValue(null, null) as MessageParser;

                    if (parser is not null)
                    {
                        var t = parser.ParseFrom(bytes);

                        return (T?)t;
                    }

                    throw new NullReferenceException($"An instance of 'IMessageParser<{typeof(T).Name}>' was not founded.");
                }

                throw new InvalidOperationException($"{response}\n{response.Content.ReadAsStringAsync()}");
            }
            catch (InvalidOperationException invalidOperationException)
            {
                Debug.WriteLine(invalidOperationException.Message);
            }
            finally
            {
                KeyValuePairs.Clear();
            }

            return default;
        }
    }
}
