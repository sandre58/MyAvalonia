// -----------------------------------------------------------------------
// <copyright file="ClassName.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Helpers;

namespace MyNet.Avalonia.Theme;

public static class ClassName
{
    public static readonly CssClass IsDisablable = new("disablable", Prefix.Is);
    public static readonly CssClass UseTransitions = new("transitions", Prefix.Use);
    public static readonly CssClass HasRole = new("role", Prefix.Has);

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Public nested class is intentional for organizing prefix constants.")]
    public static class Prefix
    {
        public const string Is = "is";
        public const string Use = "use";
        public const string Has = "has";
        public const string Role = "role";
        public const string Kind = "kind";
        public const string Context = "context";
        public const string Category = "category";
        public const string Variant = "variant";
    }
}
