using Marck7JR.Gaming.Web.Twitch.Http.Infrastructure;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Marck7JR.Gaming.Web.Twitch.Http
{
    public enum TwitchHttpClientEndpoints
    {
        [Description("https://id.twitch.tv/oauth2/")]
        OAuth,
    }

    public interface ITwitchHttpClient
    {
        public Task<TwitchOAuthResponse?> GetOAuthResponseAsync();
    }
}