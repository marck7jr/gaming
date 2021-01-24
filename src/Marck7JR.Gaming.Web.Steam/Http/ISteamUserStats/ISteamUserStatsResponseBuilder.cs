using System.ComponentModel;
using System.Threading.Tasks;

namespace Marck7JR.Gaming.Web.Steam.Http.ISteamUserStats
{
    [Description("ISteamUserStats")]

    public interface ISteamUserStatsResponseBuilder
    {
        public Task<GetSchemaForGame?> GetGetSchemaForGameAsync(string? version = "v2", params string[] queries);
    }
}