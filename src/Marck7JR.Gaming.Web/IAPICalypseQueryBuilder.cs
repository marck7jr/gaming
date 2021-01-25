using System;
using System.Threading;
using System.Threading.Tasks;

namespace Marck7JR.Gaming.Web
{
    public interface IAPICalypseQueryBuilder
    {
        public string Query { get; }
        public IAPICalypseQueryBuilder Fields(params object[] args);
        public IAPICalypseQueryBuilder Exclude(params object[] args);
        public IAPICalypseQueryBuilder Where(params object[] args);
        public IAPICalypseQueryBuilder Limit(params object[] args);
        public IAPICalypseQueryBuilder Offset(params object[] args);
        public IAPICalypseQueryBuilder Sort(params object[] args);
        public IAPICalypseQueryBuilder Search(params object[] args);
        public Task<T?> QueryAsync<T>(string requestUri, CancellationToken cancellationToken = default) where T : class;
        public Task<T?> QueryAsync<T>(Enum @enum, CancellationToken cancellationToken = default)
            where T : class;
    }
}
