namespace Marck7JR.Gaming.Web.Twitch.Http
{
    public class TwitchHttpClientOptions : ITwitchHttpClientOptions
    {
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
    }
}