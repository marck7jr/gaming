using Marck7JR.Core.Extensions;
using Marck7JR.Gaming.Web.EpicGames.Http.Infrastructure;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Windows.Storage;

namespace Marck7JR.Gaming.Web.EpicGames.Http
{
    [Serializable]
    public class EpicGamesHttpClientOptions :
#if WINDOWS10_0_17763_0_OR_GREATER
        LocalApplicationDataContainer
#else
        ObservableObject
#endif
    {
        private string? userId;
        private string? userPassword;
        private string? setSidResponse;
        private string? oAuthTokenResponse;

        public string? UserId { get => GetValue(ref userId); set => SetValue(ref userId, value); }
        public string? UserPassword { get => GetValue(ref userPassword); set => SetValue(ref userPassword, value); }
        public SetSidResponse? SetSidResponse { get => GetValue(ref setSidResponse)?.FromJson<SetSidResponse>(); set => SetValue(ref setSidResponse, value.ToJson()); }
        public OAuthTokenResponse? OAuthTokenResponse { get => GetValue(ref oAuthTokenResponse)?.FromJson<OAuthTokenResponse>(); set => SetValue(ref oAuthTokenResponse, value.ToJson()); }

        protected override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(OAuthTokenResponse), oAuthTokenResponse);
            info.AddValue(nameof(SetSidResponse), setSidResponse);
            info.AddValue(nameof(UserId), userId);
            info.AddValue(nameof(UserPassword), userPassword);
        }
    }
}
