// -----------------------------------------------------------------------
// <copyright file="MaximumToastsCount.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.Avalonia.UI.Toasting.Lifetime;

public readonly struct MaximumToastsCount : IEquatable<MaximumToastsCount>
{
    public static MaximumToastsCount UnlimitedNotifications() => new(int.MaxValue);

    public static MaximumToastsCount FromCount(int count) => new(count);

    internal int Count { get; }

    private MaximumToastsCount(int count) => Count = count;

    public override bool Equals(object? obj) => obj is MaximumToastsCount other && Equals(other);

    public override int GetHashCode() => Count.GetHashCode();

    bool IEquatable<MaximumToastsCount>.Equals(MaximumToastsCount other) => Equals(other);

    public static bool operator ==(MaximumToastsCount left, MaximumToastsCount right) => left.Equals(right);

    public static bool operator !=(MaximumToastsCount left, MaximumToastsCount right) => !(left == right);
}
