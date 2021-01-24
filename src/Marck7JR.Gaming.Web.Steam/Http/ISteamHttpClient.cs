using Marck7JR.Gaming.Web.Steam.Http.IPlayerService;
using Marck7JR.Gaming.Web.Steam.Http.ISteamUser;
using Marck7JR.Gaming.Web.Steam.Http.ISteamUserStats;

namespace Marck7JR.Gaming.Web.Steam.Http
{
    public interface ISteamHttpClient :
            IPlayerServiceRequestBuilder,
            IPlayerServiceResponseBuilder,
            ISteamUserRequestBuilder,
            ISteamUserResponseBuilder,
            ISteamUserStatsRequestBuilder,
            ISteamUserStatsResponseBuilder
    {

    }
}
