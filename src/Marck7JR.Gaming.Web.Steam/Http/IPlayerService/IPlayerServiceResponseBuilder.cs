using System.ComponentModel;
using System.Threading.Tasks;

namespace Marck7JR.Gaming.Web.Steam.Http.IPlayerService
{
    [Description("IPlayerService")]
    public interface IPlayerServiceResponseBuilder
    {
        public Task<GetOwnedGames?> GetOwnedGamesAsync(string? version = "v0001", params string[] queries);
    }
}
