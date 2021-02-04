using Marck7JR.Core.Extensions;
using Marck7JR.Gaming.Web.Twitch.Http.Infrastructure;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Windows.Storage;

namespace Marck7JR.Gaming.Web.IGDB.Http
{
    [Serializable]
    public class IGDBHttpClientOptions :
#if WINDOWS10_0_17763_0_OR_GREATER
        LocalApplicationDataContainer
#else
        ObservableObject
#endif
        , IIGDBHttpClientOptions
    {
        private string? twitchOAuthResponse;

        public TwitchOAuthResponse? TwitchOAuthResponse { get => GetValue(ref twitchOAuthResponse)?.FromJson<TwitchOAuthResponse>(); set => SetValue(ref twitchOAuthResponse, value.ToJson()); }

        protected override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(TwitchOAuthResponse), twitchOAuthResponse);
        }
    }
}
