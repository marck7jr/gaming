#nullable disable
#pragma warning disable IDE1006

namespace Marck7JR.Gaming.Web.EpicGames.Http.Infrastructure
{
    public class AssetsResponse
    {
        public string appName { get; set; }
        public string labelName { get; set; }
        public string buildVersion { get; set; }
        public string catalogItemId { get; set; }
        public string @namespace { get; set; }
        public string assetId { get; set; }
        public Metadata metadata { get; set; }

        public class Metadata
        {
            public string installationPoolId { get; set; }
        }
    }
}
