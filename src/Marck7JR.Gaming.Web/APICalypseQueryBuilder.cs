using Marck7JR.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Marck7JR.Gaming.Web
{
    public abstract class APICalypseQueryBuilder : IAPICalypseQueryBuilder
    {
        private const string SyntaxFormat = "{0} {1}; ";

        protected Dictionary<string, string> KeyValuePairs { get; } = new Dictionary<string, string>();
        public string Query
        {
            get
            {
                var stringBuilder = new StringBuilder();

                KeyValuePairs.ToList().ForEach(item =>
                {
                    stringBuilder.AppendFormat(SyntaxFormat, item.Key.ToLowerInvariant(), item.Value);
                });

                return stringBuilder.ToString().Trim();
            }
        }

        private IAPICalypseQueryBuilder Add([CallerMemberName] string? callerMemberName = null, params object[] args)
        {
            var syntaxArgs = callerMemberName switch
            {
                var x when
                x == nameof(Where) => string.Join(" & ", args),
                var x when
                x == nameof(Search) => string.Format("\"{0}\"", args.FirstOrDefault()),
                _ => string.Join(",", args)
            };

            KeyValuePairs[callerMemberName!] = syntaxArgs;

            return this;
        }

        public IAPICalypseQueryBuilder Exclude(params object[] args) => Add(args: args);
        public IAPICalypseQueryBuilder Fields(params object[] args) => Add(args: args);
        public IAPICalypseQueryBuilder Limit(params object[] args) => Add(args: args);
        public IAPICalypseQueryBuilder Offset(params object[] args) => Add(args: args);
        public IAPICalypseQueryBuilder Search(params object[] args) => Add(args: args);
        public IAPICalypseQueryBuilder Sort(params object[] args) => Add(args: args);
        public IAPICalypseQueryBuilder Where(params object[] args) => Add(args: args);
        public abstract Task<T?> QueryAsync<T>(string requestUri, CancellationToken cancellationToken = default)
            where T : class;

        public Task<T?> QueryAsync<T>(Enum @enum, CancellationToken cancellationToken)
            where T : class
            => QueryAsync<T>(@enum.GetDescription(), cancellationToken);
    }
}
