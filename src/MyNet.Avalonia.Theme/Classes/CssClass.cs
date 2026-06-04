// -----------------------------------------------------------------------
// <copyright file="CssClass.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.Primitives;

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
    public override string? ToString() => Name.WithPrefix(Prefix)?.ToLower(CultureInfo.CurrentCulture);

    /// <summary>
    /// Attempts to convert the string representation of the current instance's name to an enumeration value of the
    /// specified type.
    /// </summary>
    /// <remarks>This method uses the <see langword="Enum.TryParse"/> method to perform the conversion. If the
    /// name does not match any enum value, null is returned.</remarks>
    /// <typeparam name="T">The type of enumeration to convert to. Must be a non-nullable value type that is an enum.</typeparam>
    /// <returns>The corresponding enum value if the conversion is successful; otherwise, null.</returns>
    public T? ToEnum<T>()
        where T : struct, Enum => Enum.TryParse<T>(Name, out var result) ? result : null;

    /// <summary>
    /// Implicit conversion from <see cref="CssClass"/> to <see cref="string"/>.
    /// Returns the result of <see cref="ToString"/> or <c>null</c> when the instance is <c>null</c>.
    /// </summary>
    public static implicit operator string?(CssClass value) => value.ToString();

    // Theme
    public static readonly CssClass HasRole = new("role", CssPrefix.Has);

    // Kinds
    public static readonly CssClass KindFocus = new("focus", CssPrefix.Kind);
    public static readonly CssClass KindCard = new("card", CssPrefix.Kind);
    public static readonly CssClass KindSection = new("section", CssPrefix.Kind);

    // States
    public static readonly CssClass IsDisablable = new("disablable", CssPrefix.Is);
    public static readonly CssClass UseTransitions = new("transitions", CssPrefix.Use);

    // Alignments
    public static readonly CssClass Centered = new("centered", CssPrefix.Alignment);
    public static readonly CssClass VerticalCentered = new("centered", CssPrefix.VerticalAlignment);

    // Layouts
    public static readonly CssClass Uniform = new("uniform", CssPrefix.Layout);
    public static readonly CssClass Wrap = new("wrap", CssPrefix.Layout);
    public static readonly CssClass Vertical = new("vertical", CssPrefix.Layout);
    public static readonly CssClass Horizontal = new("horizontal", CssPrefix.Layout);
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
    public static readonly CssClass ShapeAlternate = new("alternate", CssPrefix.Shape);
    public static readonly CssClass ShapeItemsCircle = new("items-circle", CssPrefix.Shape);

    // Texts
    public static readonly CssClass TextHelper = new("helper", CssPrefix.Text);
    public static readonly CssClass TextWatermark = new("watermark", CssPrefix.Text);
    public static readonly CssClass TextUnderline = new("underline", CssPrefix.Text);
    public static readonly CssClass TextStrikethrough = new("strikethrough", CssPrefix.Text);
    public static readonly CssClass HeaderHelper = new("helper", CssPrefix.Header);
    public static readonly CssClass HeaderWatermark = new("watermark", CssPrefix.Header);

    // Variant
    public static readonly CssClass Underline = new("underline", CssPrefix.Variant);
    public static readonly CssClass VariantHeader = new("header", CssPrefix.Variant);
    public static readonly CssClass Watermark = new("watermark", CssPrefix.Variant);

    // Alignments
    public static CssClass Alignment(string name) => new(name, CssPrefix.Alignment);

    public static CssClass HeaderAlignment(string name) => new(name, CssPrefix.HeaderAlignment);

    public static CssClass VerticalHeaderAlignment(string name) => new(name, CssPrefix.VerticalHeaderAlignment);

    public static CssClass ContentAlignment(string name) => new(name, CssPrefix.ContentAlignment);

    public static CssClass Position(string name) => new(name, CssPrefix.Position);

    // Sizes
    public static CssClass Size(string name) => new(name, CssPrefix.Size);

    public static CssClass Size(SpacingSize size) => Size(size.ToString());

    // Styles
    public static CssClass Border(int value) => new($"{value}", CssPrefix.Border);

    public static CssClass Icon(string name) => new(name, CssPrefix.Icon);

    public static CssClass Kind(string name) => new(name, CssPrefix.Kind);

    public static CssClass Variant(string name) => new(name, CssPrefix.Variant);

    public static CssClass Variant(ControlVariant variant) => Variant(variant.ToString());

    public static CssClass ItemsVariant(string name) => new(name, CssPrefix.ItemsVariant);

    public static CssClass ItemsVariant(ControlVariant variant) => ItemsVariant(variant.ToString());

    public static CssClass HeaderVariant(string name) => new(name, CssPrefix.HeaderVariant);

    public static CssClass HeaderVariant(ControlVariant variant) => HeaderVariant(variant.ToString());

    public static CssClass Indicator(string name) => new(name, CssPrefix.Indicator);

    public static CssClass FromEnum<T>(T defaultValue)
        where T : Enum => new(defaultValue.ToString(), typeof(T).Name);
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
    public const string Position = "position";

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
    public const string Indicator = "indicator";

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

    // Size
    public const string Half = "1/2";
}
