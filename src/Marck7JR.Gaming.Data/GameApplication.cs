using Marck7JR.Gaming.Data.Contracts;
using System;

namespace Marck7JR.Gaming.Data
{
    public class GameApplication : IGameApplication, IEquatable<GameApplication>
    {
        public GameApplication()
        {

        }

        public GameApplication(IGameLibrary library, object? manifest = null)
        {
            Issuer = library.GetType();
            Manifest = manifest;
        }

        public string? Args { get; set; }
        public string? AppId { get; set; }
        public string? DisplayName { get; set; }
        public bool IsInstalled { get; set; }
        public Type? Issuer { get; set; }
        public object? Manifest { get; set; }
        public string? Path { get; set; }

        public bool Equals(GameApplication? other) => AppId == other?.AppId && Issuer == other?.Issuer;
        public override bool Equals(object obj) => Equals(obj as GameApplication);
        public override int GetHashCode() => (AppId, Issuer).GetHashCode();
    }
}
