using Marck7JR.Core.Extensions;
using Marck7JR.Gaming.Web.EpicGames.Http.Infrastructure;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Marck7JR.Gaming.Web.EpicGames.Http
{
    public class EpicGamesHttpClient
    {
        private const string _api_country = "lc";
        private const string _api_locale = "cc";

        private const string _oauth_host = "account-public-service-prod03.ol.epicgames.com";
        private const string _launcher_host = "launcher-public-service-prod06.ol.epicgames.com";
        private const string _entitlements_host = "entitlement-public-service-prod08.ol.epicgames.com";
        private const string _catalog_host = "catalog-public-service-prod06.ol.epicgames.com";
        private const string _ecommerce_host = "ecommerceintegration-public-service-ecomprod02.ol.epicgames.com";
        private const string _datastorage_host = "datastorage-public-service-liveegs.live.use1a.on.epicgames.com";
        private const string _library_host = "library-service.live.use1a.on.epicgames.com";

        private const string X_XSRF_TOKEN_CookieName = "X-XSRF-TOKEN";

        public const string AuthorizationUri = "https://www.epicgames.com/id/login?redirectUrl={0}";
        public const string AuthorizationUriRedirectUrl = "https://www.epicgames.com/id/api/redirect";

        private readonly HttpClient _httpClient;
        private readonly EpicGamesHttpClientOptions _options;

        public EpicGamesHttpClient(HttpClient httpClient, IOptions<EpicGamesHttpClientOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;

            _httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("UELauncher/11.0.1-14907503+++Portal+Release-Live Windows/10.0.19041.1.256.64bit");
        }

        private HttpClientHandler GetHttpClientHandler<T>(T t)
        {
            if (t is HttpClientHandler httpClientHandler)
            {
                return httpClientHandler;
            }
            else if (t is DelegatingHandler { InnerHandler: HttpClientHandler _httpClientHandler })
            {
                return _httpClientHandler;
            }
            else if (t is DelegatingHandler { InnerHandler: DelegatingHandler _delegatingHandler })
            {
                return GetHttpClientHandler(_delegatingHandler);
            }

            throw new InvalidOperationException();
        }

        private async Task<OAuthTokenResponse?> GetOAuthTokenResponseAsync(ExchangeGenerateResponse? exchangeGenerateResponse = null, OAuthTokenResponse? oAuthTokenResponse = null)
        {
            if (exchangeGenerateResponse is null && oAuthTokenResponse is null)
            {
                throw new ArgumentNullException();
            }

            try
            {
                var credentials = Encoding.ASCII.GetBytes($"{_options.UserId}:{_options.UserPassword}");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credentials));

                var formUrlEncodedContent = credentials switch
                {
                    var _ when exchangeGenerateResponse is not null => new FormUrlEncodedContent(new Dictionary<string?, string?>
                    {
                        { "grant_type", "exchange_code" },
                        { "exchange_code", $"{exchangeGenerateResponse.code}" },
                        { "token_type", "eg1" }
                    }),
                    var _ when oAuthTokenResponse is not null => new FormUrlEncodedContent(new Dictionary<string?, string?>
                    {
                        { "grant_type", "refresh_token" },
                        { "refresh_token", $"{oAuthTokenResponse.refresh_token}" },
                        { "token_type", "eg1" }
                    }),
                    _ => throw new NotSupportedException()
                };

                UriBuilder oauthTokenUriBuilder = new($"https://{_oauth_host}/account/api/oauth/token");

                var response = await _httpClient.PostAsync(oauthTokenUriBuilder.Uri, formUrlEncodedContent);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var oAuth = json.FromJson<OAuthTokenResponse>();

                    return oAuth;
                }

                throw new InvalidOperationException($"{response}\n{await response.Content.ReadAsStringAsync()}");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return default;
        }

        public async Task<OAuthTokenResponse?> GetOAuthTokenResponseAsync()
        {
            _options.OAuthTokenResponse = _options switch
            {
                var x when
                x.OAuthTokenResponse is not null &&
                x.OAuthTokenResponse?.refresh_expires_at > DateTime.UtcNow => await GetOAuthTokenResponseAsync(x.OAuthTokenResponse),
                var x when x.SetSidResponse is not null => await GetOAuthTokenResponseAsync(x.SetSidResponse),
                _ => default
            };

            return _options.OAuthTokenResponse;
        }

        public async Task<OAuthTokenResponse?> GetOAuthTokenResponseAsync(OAuthTokenResponse? oAuthTokenResponse) => await GetOAuthTokenResponseAsync(null, oAuthTokenResponse);

        public async Task<OAuthTokenResponse?> GetOAuthTokenResponseAsync(SetSidResponse? setSidResponse)
        {
            if (setSidResponse is null)
            {
                throw new ArgumentNullException(nameof(setSidResponse));
            }

            var _handler = _httpClient.GetType().BaseType.GetField("_handler", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_httpClient);
            var httpClientHandler = GetHttpClientHandler(_handler);

            if (httpClientHandler.CookieContainer is null)
            {
                throw new NullReferenceException();
            }

            try
            {
                _httpClient.DefaultRequestHeaders.Add("X-Epic-Event-Action", "login");
                _httpClient.DefaultRequestHeaders.Add("X-Epic-Event-Category", "login");
                _httpClient.DefaultRequestHeaders.Add("X-Epic-Strategy-Flags", "");
                _httpClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
                _httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) ");
                _httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("AppleWebKit/537.36 (KHTML, like Gecko) ");
                _httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("EpicGamesLauncher/11.0.1-14907503+++Portal+Release-Live ");
                _httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("UnrealEngine/4.23.0-14907503+++Portal+Release-Live ");
                _httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("Chrome/84.0.4147.38 Safari/537.36");

                UriBuilder setSidUriBuilder = new("https://www.epicgames.com/id/api/set-sid")
                {
                    Query = $"sid={setSidResponse.sid}"
                };

                _ = await _httpClient.GetAsync(setSidUriBuilder.Uri);

                UriBuilder csrfUriBuilder = new("https://www.epicgames.com/id/api/csrf");

                _ = await _httpClient.GetAsync(csrfUriBuilder.Uri);

                var cookie = httpClientHandler.CookieContainer.GetCookies(csrfUriBuilder.Uri)["XSRF-TOKEN"] ?? throw new ArgumentOutOfRangeException();
                _httpClient.DefaultRequestHeaders.Add(X_XSRF_TOKEN_CookieName, cookie.Value);

                var response = await _httpClient.PostAsync($"https://www.epicgames.com/id/api/exchange/generate", null);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var exchangeCode = json.FromJson<ExchangeGenerateResponse>();

                    return await GetOAuthTokenResponseAsync(exchangeCode);
                }

                throw new InvalidOperationException($"{response}\n{await response.Content.ReadAsStringAsync()}");
            }
            catch (InvalidOperationException invalidOperationException)
            {
                Debug.WriteLine(invalidOperationException);
            }

            return default;
        }

        public IAsyncEnumerable<AssetsResponse> GetAssetsResponsesAsync() => GetAssetsResponsesAsync(_options.OAuthTokenResponse);

        public async IAsyncEnumerable<AssetsResponse> GetAssetsResponsesAsync(OAuthTokenResponse? oAuthTokenResponse)
        {
            if (oAuthTokenResponse?.expires_at < DateTime.UtcNow)
            {
                yield break;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"{oAuthTokenResponse?.token_type}", $"{oAuthTokenResponse?.access_token}");

            var response = await _httpClient.GetAsync($"https://{_launcher_host}/launcher/api/public/assets/Windows?label=Live");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var assetsResponses = json.FromJson<AssetsResponse[]>();

                if (assetsResponses is not null)
                {
                    foreach (var assetResponse in assetsResponses)
                    {
                        yield return assetResponse;
                    }
                }
            }
        }

        public async IAsyncEnumerable<CatalogResponse> GetCatalogResponsesAsync(IEnumerable<AssetsResponse>? assetsResponses)
        {
            if (assetsResponses is null)
            {
                throw new ArgumentNullException(nameof(assetsResponses));
            }

            foreach (var assetsResponseItem in assetsResponses)
            {
                var response = await _httpClient.GetAsync($"https://{_catalog_host}/catalog/api/shared/namespace/{assetsResponseItem.@namespace}/bulk/items?id={assetsResponseItem.catalogItemId}&includeDLCDetails=True&includeMainGameDetails=True&country={_api_country}&locale={_api_locale}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var keyValuePairs = json.FromJson<Dictionary<string, CatalogResponse>>();

                    yield return keyValuePairs.FirstOrDefault().Value;
                }
                else
                {
                    throw new InvalidOperationException($"{response}\n{await response.Content.ReadAsStringAsync()}");
                }
            }
        }
    }
}
