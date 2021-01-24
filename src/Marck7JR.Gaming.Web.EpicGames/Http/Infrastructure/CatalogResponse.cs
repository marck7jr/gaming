#nullable disable
#pragma warning disable IDE1006

using System;

namespace Marck7JR.Gaming.Web.EpicGames.Http.Infrastructure
{
    public class CatalogResponse
    {
        public string id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string longDescription { get; set; }
        public KeyImage[] keyImages { get; set; }
        public Category[] categories { get; set; }
        public string _namespace { get; set; }
        public string status { get; set; }
        public DateTime creationDate { get; set; }
        public DateTime lastModifiedDate { get; set; }
        public CustomAttributes customAttributes { get; set; }
        public string entitlementName { get; set; }
        public string entitlementType { get; set; }
        public string itemType { get; set; }
        public ReleaseInfo[] releaseInfo { get; set; }
        public string developer { get; set; }
        public string developerId { get; set; }
        public string[] eulaIds { get; set; }
        public bool endOfSupport { get; set; }
        public DlcItemList[] dlcItemList { get; set; }
        public AgeGatings ageGatings { get; set; }
        public bool selfRefundable { get; set; }
        public bool unsearchable { get; set; }

        public class CustomAttributes
        {
            public CanRunOffline CanRunOffline { get; set; }
            public PresenceId PresenceId { get; set; }
            public MonitorPresence MonitorPresence { get; set; }
            public UseAccessControl UseAccessControl { get; set; }
            public RequirementsJson RequirementsJson { get; set; }
            public CanSkipKoreanIdVerification CanSkipKoreanIdVerification { get; set; }
            public OwnershipToken OwnershipToken { get; set; }
            public FolderName FolderName { get; set; }
        }

        public class CanRunOffline
        {
            public string type { get; set; }
            public string value { get; set; }
        }

        public class PresenceId
        {
            public string type { get; set; }
            public string value { get; set; }
        }

        public class MonitorPresence
        {
            public string type { get; set; }
            public string value { get; set; }
        }

        public class UseAccessControl
        {
            public string type { get; set; }
            public string value { get; set; }
        }

        public class RequirementsJson
        {
            public string type { get; set; }
            public string value { get; set; }
        }

        public class CanSkipKoreanIdVerification
        {
            public string type { get; set; }
            public string value { get; set; }
        }

        public class OwnershipToken
        {
            public string type { get; set; }
            public string value { get; set; }
        }

        public class FolderName
        {
            public string type { get; set; }
            public string value { get; set; }
        }

        public class AgeGatings
        {

        }

        public class KeyImage
        {
            public string type { get; set; }
            public string url { get; set; }
            public string md5 { get; set; }
            public int width { get; set; }
            public int height { get; set; }
            public int size { get; set; }
            public DateTime uploadedDate { get; set; }
        }

        public class Category
        {
            public string path { get; set; }
        }

        public class ReleaseInfo
        {
            public string id { get; set; }
            public string appId { get; set; }
            public string[] platform { get; set; }
            public DateTime dateAdded { get; set; }
        }

        public class DlcItemList
        {
            public string id { get; set; }
            public string _namespace { get; set; }
            public bool unsearchable { get; set; }
        }
    }
}