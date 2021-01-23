using System;

namespace Marck7JR.Gaming.Data.Tests.Extensions
{
    public static class GameApplicationExtensions
    {
        public static Func<GameApplication, string> ToTestContext_WriteLine { get; set; } = (application) => $"{(application.IsInstalled ? '✔' : '❌')} [{application.AppId}] {application.DisplayName}";

        public static string ToTestContext(this GameApplication application) => ToTestContext_WriteLine(application);
    }
}
