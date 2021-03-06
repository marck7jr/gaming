﻿using Marck7JR.Core.Data;
using Marck7JR.Gaming.Data.Contracts;
using System;

namespace Marck7JR.Gaming.Data
{
    public class GameApplication : Entity, IGameApplication, IEquatable<GameApplication>
    {
        private string? args;
        private string? appId;
        private string? displayName;
        private bool isInstalled;
        private object? manifest;
        private string? path;
        private string? issuerName;

        public GameApplication() : base()
        {

        }

        public GameApplication(IGameLibrary library, object? manifest = null) : this()
        {
            Issuer = library.GetType();
            Manifest = manifest;
        }

        public static bool operator ==(GameApplication? x, GameApplication? y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x is null)
            {
                return false;
            }

            if (y is null)
            {
                return false;
            }

            return x.Equals(y);
        }

        public static bool operator !=(GameApplication? x, GameApplication? y) => !(x == y);

        public string? Args { get => GetValue(ref args); set => SetValue(ref args, value); }
        public string? AppId { get => GetValue(ref appId); set => SetValue(ref appId, value); }
        public string? DisplayName { get => GetValue(ref displayName); set => SetValue(ref displayName, value); }
        public bool IsInstalled { get => GetValue(ref isInstalled); set => SetValue(ref isInstalled, value); }
        public Type? Issuer { get => Type.GetType(GetValue(ref issuerName)); set => SetValue(ref issuerName, value?.FullName); }
        public string? IssuerName { get => GetValue(ref issuerName); set => SetValue(ref issuerName, value); }
        public object? Manifest { get => GetValue(ref manifest); set => SetValue(ref manifest, value); }
        public string? Path { get => GetValue(ref path); set => SetValue(ref path, value); }
        public virtual int CompareTo(GameApplication? other)
        {
            if (other is not { AppId: string appId, Issuer: Type issuer, })
            {
                return 1;
            }

            return AppId?.CompareTo(appId) ^ Issuer?.FullName.CompareTo(issuer.FullName) ?? default;
        }
        public virtual bool Equals(GameApplication? other) => AppId == other?.AppId && Issuer == other?.Issuer;
        public override bool Equals(object obj) => Equals(obj as GameApplication);
        public override int GetHashCode() => (AppId, Issuer).GetHashCode();
        bool IEquatable<GameApplication>.Equals(GameApplication other) => Equals(other);
    }
}
