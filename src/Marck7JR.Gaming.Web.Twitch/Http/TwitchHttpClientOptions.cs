using System.ComponentModel;
using System.Runtime.Serialization;
using Windows.Storage;

namespace Marck7JR.Gaming.Web.Twitch.Http
{
    public class TwitchHttpClientOptions :
#if WINDOWS10_0_17763_0_OR_GREATER
        LocalApplicationDataContainer
#else
        ObservableObject
#endif
        , ITwitchHttpClientOptions
    {
        private string? clientId;
        private string? clientSecret;

        public string? ClientId { get => GetValue(ref clientId); set => SetValue(ref clientId, value); }
        public string? ClientSecret { get => GetValue(ref clientSecret); set => SetValue(ref clientSecret, value); }

        protected override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(ClientId), clientId);
            info.AddValue(nameof(ClientSecret), clientSecret);
        }
    }
}