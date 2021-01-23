using System;

namespace Marck7JR.Gaming.Data.Contracts
{
    public interface IGameApplication
    {
        public string? Args { get; set; }
        public string? AppId { get; set; }
        public string? DisplayName { get; set; }
        public bool IsInstalled { get; set; }
        public Type? Issuer { get; set; }
        public object? Manifest { get; set; }
        public string? Path { get; set; }
    }
}
