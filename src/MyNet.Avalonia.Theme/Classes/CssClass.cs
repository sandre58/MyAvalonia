// -----------------------------------------------------------------------
// <copyright file="CssClass.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using MyNet.Avalonia.Extensions;

namespace MyNet.Avalonia.Theme.Classes;

/// <summary>
/// Provides a record type for representing a CSS class name with an optional prefix that can be used to generate the full class name with the prefix applied.
/// </summary>
/// <param name="Name">The name of the CSS class.</param>
/// <param name="Prefix">An optional prefix to apply to the CSS class name.</param>
public record CssClass(string Name, string? Prefix = "")
{
    /// <summary>
    /// Provides a string representation of the CSS class name with the prefix applied, if a prefix is specified.
    /// </summary>
    /// <returns>The full CSS class name with the prefix applied, if any.</returns>
    public override string ToString() => Name.WithPrefix(Prefix).ToLower(CultureInfo.CurrentCulture);

    // Theme
    public static readonly CssClass HasRole = new("role", CssPrefix.Has);
    public static readonly CssClass KindFocus = new("focus", CssPrefix.Kind);
    public static readonly CssClass VariantHeader = new("header", CssPrefix.Variant);

    // States
    public static readonly CssClass IsDisablable = new("disablable", CssPrefix.Is);
    public static readonly CssClass UseTransitions = new("transitions", CssPrefix.Use);

    // Alignments
    public static readonly CssClass Centered = new("centered", CssPrefix.Alignment);
    public static readonly CssClass VerticalCentered = new("centered", CssPrefix.VerticalAlignment);

    // Layouts
    public static readonly CssClass Uniform = new("uniform", CssPrefix.Layout);
    public static readonly CssClass Wrap = new("wrap", CssPrefix.Layout);
    public static readonly CssClass IsStretch = new("stretch", CssPrefix.Is);

    // Effects
    public static readonly CssClass Hidden = new("hidden");
    public static readonly CssClass Visible = new("visible");
    public static readonly CssClass ShadowControl = new("control", CssPrefix.Shadow);
    public static readonly CssClass ShadowSurface = new("surface", CssPrefix.Shadow);
    public static readonly CssClass ShadowHeader = new("header", CssPrefix.Shadow);
    public static readonly CssClass ShadowItems = new("items", CssPrefix.Shadow);
    public static readonly CssClass FocusRectangle = new("rectangle", CssPrefix.Focus);
    public static readonly CssClass FocusEllipse = new("ellipse", CssPrefix.Focus);
    public static readonly CssClass FocusHidden = new("hidden", CssPrefix.Focus);

    // Shapes
    public static readonly CssClass ShapeCircle = new("circle", CssPrefix.Shape);
    public static readonly CssClass ShapeItemsCircle = new("items-circle", CssPrefix.Shape);

    // Texts
    public static readonly CssClass TextHelper = new("helper", CssPrefix.Text);
    public static readonly CssClass TextWatermark = new("watermark", CssPrefix.Text);
    public static readonly CssClass TextUnderline = new("underline", CssPrefix.Text);
    public static readonly CssClass TextStrikethrough = new("strikethrough", CssPrefix.Text);
    public static readonly CssClass HeaderHelper = new("helper", CssPrefix.Header);
    public static readonly CssClass HeaderWatermark = new("watermark", CssPrefix.Header);

    // Alignments
    public static CssClass Alignment(string name) => new(name, CssPrefix.Alignment);

    public static CssClass HeaderAlignment(string name) => new(name, CssPrefix.HeaderAlignment);

    public static CssClass ContentAlignment(string name) => new(name, CssPrefix.ContentAlignment);

    // Sizes
    public static CssClass Size(string name) => new(name, CssPrefix.Size);

    // Styles
    public static CssClass Border(int value) => new($"{value}", CssPrefix.Border);
}

/// <summary>
/// Provides a set of constant string prefixes used for constructing standardized CSS class names.
/// </summary>
/// <remarks>These prefixes help ensure consistency and clarity in CSS class naming conventions across the
/// application. Use these constants when generating or referencing CSS classes to promote maintainability and reduce
/// the risk of naming conflicts.</remarks>
public static class CssPrefix
{
    // Themes
    public const string Kind = "kind";
    public const string Role = "role";
    public const string Context = "context";
    public const string Category = "category";

    // States
    public const string Is = "is";
    public const string Use = "use";
    public const string Has = "has";

    // Variants
    public const string Variant = "variant";
    public const string HeaderVariant = "variant-header";
    public const string ItemsVariant = "variant-items";

    // Alignments
    public const string Alignment = "align";
    public const string VerticalAlignment = "valign";
    public const string HeaderAlignment = "align-header";
    public const string VerticalHeaderAlignment = "valign-header";
    public const string ContentAlignment = "align-content";
    public const string VerticalContentAlignment = "valign-content";

    // Sizes
    public const string Size = "size";

    // Spacing
    public const string Margin = "m";
    public const string LeftMargin = "ml";
    public const string RightMargin = "mr";
    public const string TopMargin = "mt";
    public const string BottomMargin = "mb";
    public const string HorizontalMargin = "mx";
    public const string VerticalMargin = "my";
    public const string Padding = "p";
    public const string LeftPadding = "pl";
    public const string RightPadding = "pr";
    public const string TopPadding = "pt";
    public const string BottomPadding = "pb";
    public const string HorizontalPadding = "px";
    public const string VerticalPadding = "py";
    public const string Spacing = "gap";
    public const string HorizontalSpacing = "gapx";
    public const string VerticalSpacing = "gapy";

    // Layouts
    public const string Layout = "flex";

    // Effects
    public const string Opacity = "opacity";
    public const string Shadow = "shadow";
    public const string Focus = "focus";

    // Shapes
    public const string Shape = "shape";

    // Icons
    public const string Icon = "icon";

    // Styles
    public const string Border = "border";
    public const string CornerRadius = "rounded";

    // Texts
    public const string Text = "text";
    public const string Font = "font";
    public const string Truncate = "truncate";
    public const string Header = "header";
}

public static class CssSuffix
{
    // Alignments
    public const string Middle = "middle";
}
