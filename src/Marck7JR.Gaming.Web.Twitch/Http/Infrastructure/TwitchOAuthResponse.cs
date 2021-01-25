#nullable enable
#pragma warning disable IDE1006

using System;
using System.Text.Json.Serialization;

namespace Marck7JR.Gaming.Web.Twitch.Http.Infrastructure
{
    public class TwitchOAuthResponse
    {
        public string? access_token { get; set; }
        public DateTime? created_at { get; set; } = DateTime.UtcNow;
        public double? expires_in { get; set; }
        [JsonIgnore]
        public DateTime expires_at => created_at.GetValueOrDefault().AddSeconds(expires_in.GetValueOrDefault());
        public string? token_type { get; set; }
    }
}
