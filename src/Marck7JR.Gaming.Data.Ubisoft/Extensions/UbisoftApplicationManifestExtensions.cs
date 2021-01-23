using Marck7JR.Core.Extensions;
using Marck7JR.Gaming.Data.Ubisoft.Infrastructure;
using System;

namespace Marck7JR.Gaming.Data.Ubisoft.Extensions
{
    internal static class UbisoftApplicationManifestExtensions
    {
        internal static string? GetManifestName(this UbisoftApplicationManifest applicationManifest)
        {
            if (applicationManifest is null)
            {
                throw new ArgumentNullException(nameof(applicationManifest));
            }

            var name = applicationManifest switch
            {
                var x when
                x.root?.name?.Equals("l1") ?? false => x?.localizations?.@default?["l1"],
                var x when
                x.root?.installer is not null &&
                x.root.installer.upgraded_from_legacy &&
                !x.root.installer.game_identifier.IsNullOrEmpty() => x.root.installer.game_identifier,
                _ => applicationManifest?.root?.name
            };

            return name;
        }
    }
}
