using System;

namespace Marck7JR.Gaming.Data.Contracts
{
    public enum GameLibraryProtocolKind
    {
        Install,
        Run,
        Purchase,
        Uninstall,
        View,
    }

    public interface IGameLibraryProtocol
    {
        public string? AbsoluteUriFormat { get; }
        public GameLibraryProtocolKind Kind { get; }
    }
}
