#nullable disable
#pragma warning disable IDE1006

namespace Marck7JR.Gaming.Web.Steam.Http.ISteamUser
{
    public class GetPlayerSummaries
    {
        public Response response { get; set; }

        public class Response
        {
            public Players players { get; set; }
        }

        public class Players
        {
            public Player[] player { get; set; }
        }

        public class Player
        {
            public string steamid { get; set; }
            public int communityvisibilitystate { get; set; }
            public int profilestate { get; set; }
            public string personaname { get; set; }
            public string profileurl { get; set; }
            public string avatar { get; set; }
            public string avatarmedium { get; set; }
            public string avatarfull { get; set; }
            public string avatarhash { get; set; }
            public int lastlogoff { get; set; }
            public int personastate { get; set; }
            public string realname { get; set; }
            public string primaryclanid { get; set; }
            public int timecreated { get; set; }
            public int personastateflags { get; set; }
            public string loccountrycode { get; set; }
            public string locstatecode { get; set; }
            public int loccityid { get; set; }
        }
    }
}