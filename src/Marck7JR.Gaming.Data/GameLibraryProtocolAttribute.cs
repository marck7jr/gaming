using Marck7JR.Gaming.Data.Contracts;
using System;

namespace Marck7JR.Gaming.Data
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class GameLibraryProtocolAttribute : Attribute, IGameLibraryProtocol
    {
        public GameLibraryProtocolAttribute(GameLibraryProtocolKind kind, string? absoluteUriFormat)
        {
            Kind = kind;
            AbsoluteUriFormat = absoluteUriFormat;
        }

        public string? AbsoluteUriFormat { get; }
        public GameLibraryProtocolKind Kind { get; }
    }
}
