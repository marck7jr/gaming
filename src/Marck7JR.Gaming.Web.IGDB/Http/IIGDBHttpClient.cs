using System.ComponentModel;

namespace Marck7JR.Gaming.Web.IGDB.Http
{
    public enum IGDBHttpClientEndpoint
    {
        [Description("games.pb")]
        Games,
        [Description("release_dates.pb")]
        ReleaseDates,
    }

    public interface IIGDBHttpClient : IAPICalypseQueryBuilder
    {

    }
}
