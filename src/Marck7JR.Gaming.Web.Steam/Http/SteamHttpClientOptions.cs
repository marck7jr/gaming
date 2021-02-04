using Marck7JR.Core.Extensions;
using Marck7JR.Gaming.Web.Steam.Http.ISteamUser;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Windows.Storage;

namespace Marck7JR.Gaming.Web.Steam.Http
{
    [Serializable]
    public class SteamHttpClientOptions :
#if WINDOWS10_0_17763_0_OR_GREATER
        LocalApplicationDataContainer
#else
        ObservableObject
#endif
        , ISteamHttpClientOptions
    {
        private string? webApiKey;
        private string? getPlayerSummaries;

        public string? WebApiKey { get => GetValue(ref webApiKey); set => SetValue(ref webApiKey, value); }
        public GetPlayerSummaries? GetPlayerSummaries { get => GetValue(ref getPlayerSummaries)?.FromJson<GetPlayerSummaries>(); set => SetValue(ref getPlayerSummaries, value.ToJson()); }

        protected override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(WebApiKey), webApiKey);
            info.AddValue(nameof(GetPlayerSummaries), getPlayerSummaries);
        }
    }
}