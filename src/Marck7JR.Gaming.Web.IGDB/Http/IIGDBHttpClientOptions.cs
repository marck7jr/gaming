using Marck7JR.Gaming.Web.Twitch.Http.Infrastructure;

namespace Marck7JR.Gaming.Web.IGDB.Http
{
    public interface IIGDBHttpClientOptions
    {
        public TwitchOAuthResponse? TwitchOAuthResponse { get; set; }
    }
}