using Marck7JR.Core.Extensions;
using Marck7JR.Gaming.Web.Twitch.Http.Infrastructure;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace Marck7JR.Gaming.Web.Twitch.Http
{
    public class TwitchHttpClient : ITwitchHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly TwitchHttpClientOptions _options;

        public TwitchHttpClient(HttpClient httpClient, IOptions<TwitchHttpClientOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<TwitchOAuthResponse?> GetOAuthResponseAsync()
        {
            try
            {
                _httpClient.BaseAddress = new Uri(TwitchHttpClientEndpoints.OAuth.GetDescription());

                var response = await _httpClient.PostAsync($"token?client_id={_options.ClientId}&client_secret={_options.ClientSecret}&grant_type=client_credentials", null);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    return json.FromJson<TwitchOAuthResponse>();
                }

                throw new InvalidOperationException($"{response}\n{await response.Content.ReadAsStringAsync()}");
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }

            return default;
        }
    }
}
