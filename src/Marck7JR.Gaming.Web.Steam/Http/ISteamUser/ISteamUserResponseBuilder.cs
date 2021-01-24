using System.ComponentModel;
using System.Threading.Tasks;

namespace Marck7JR.Gaming.Web.Steam.Http.ISteamUser
{
    [Description("ISteamUser")]
    public interface ISteamUserResponseBuilder
    {
        public Task<GetPlayerSummaries?> GetGetPlayerSummariesAsync(string? version = "v0001", params string[] queries);
    }
}