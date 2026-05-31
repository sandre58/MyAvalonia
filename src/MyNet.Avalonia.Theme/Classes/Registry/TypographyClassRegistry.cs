// -----------------------------------------------------------------------
// <copyright file="TypographyClassRegistry.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Data;
using Avalonia.Media;
using MyNet.Avalonia.Converters;
using MyNet.Avalonia.Theme.Assists;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.Avalonia.Theme.Classes.Registry.States;
using MyNet.Avalonia.Theme.Theming;

namespace MyNet.Avalonia.Theme.Classes.Registry;

/// <summary>
/// Provides utility methods for registering and managing typography settings for visual elements.
/// </summary>
/// <remarks>The TypographyClassRegistry class offers static methods to streamline the process of applying and
/// registering typography values for visual components. It is intended to centralize typography configuration, making it
/// easier to maintain consistent visual appearance across an application.</remarks>
public static class TypographyClassRegistry
{
    #region State

    /// <summary>
    /// Represents the state of text decorations (underline and strikethrough) for a TextBlock. This class is used to manage the combined state of text decorations, allowing for efficient application of multiple decorations without conflicts.
    /// </summary>
    private sealed class ControlState
    {
        /// <summary>
        /// Gets or sets a value indicating whether the text should be underlined. This property is used to determine if an underline decoration should be applied to the text. When set to true, an underline decoration will be added to the TextBlock; when false, it will not be applied.
        /// </summary>
        public bool IsUnderline { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the text should have a strikethrough. This property is used to determine if a strikethrough decoration should be applied to the text. When set to true, a strikethrough decoration will be added to the TextBlock; when false, it will not be applied.
        /// </summary>
        public bool IsStrikethrough { get; set; }

        /// <summary>
        /// Gets tracks all SetProperty bindings for the text decoration section so they can be properly disposed on state change.
        /// </summary>
        public BindingGroup Bindings { get; } = new();
    }

    /// <summary>
    /// Applies underline and strikethrough text decorations to the specified TextBlock control based on the provided
    /// text state.
    /// </summary>
    /// <remarks>If the TextState indicates underline or strikethrough, the corresponding text decoration is
    /// added to the control. Existing text decorations on the control are replaced.</remarks>
    /// <param name="control">The TextBlock control to which the text decorations will be applied. Cannot be null.</param>
    /// <param name="state">The TextState object that determines whether underline and/or strikethrough decorations are applied. Cannot be
    /// null.</param>
    private static void ApplyState(TextBlock control, ControlState state)
    {
        state.Bindings.Reset();

        var textDecorations = new TextDecorationCollection();
        if (state.IsUnderline)
        {
            textDecorations.Add(new()
            {
                Location = TextDecorationLocation.Underline
            });
        }

        if (state.IsStrikethrough)
        {
            textDecorations.Add(new()
            {
                Location = TextDecorationLocation.Strikethrough
            });
        }

        state.Bindings.Add(control.SetProperty(TextBlock.TextDecorationsProperty, textDecorations));
    }

    #endregion

    /// <summary>
    /// Registers text-related properties and behaviors for UI elements, including text alignment, font size, and text
    /// wrapping, as well as styles for helper and watermark text.
    /// </summary>
    /// <remarks>Call this method during application startup to configure default text and header styles for
    /// controls. This ensures consistent appearance and behavior for text elements, headers, and their associated
    /// helper and watermark styles throughout the application.</remarks>
    public static void RegisterTexts()
    {
        // Texts
        ClassRegistry.RegisterMany<TextAlignment, TextBlock>(CssPrefix.Text, (x, y) => x.SetProperty(TextBlock.TextAlignmentProperty, y));
        ClassRegistry.RegisterMany<FontSize, Control>(CssPrefix.Text, (x, y) => new CompositeDisposable
        {
            x.SetProperty(TextElement.FontSizeProperty, ThemeResources.Font.Size.Get(y).Value),
            y.ToString().StartsWith('H') ? x.SetProperty(TextElement.FontWeightProperty, ThemeResources.Font.Weight.Header.Value) : Disposable.Empty
        });
        ClassRegistry.RegisterMany<TextWrapping, TextBlock>(CssPrefix.Truncate, (x, y) => x.SetProperty(TextBlock.TextWrappingProperty, y));

        ClassRegistry.Register<Control>(CssClass.TextHelper, x => new CompositeDisposable
        {
            x.SetProperty(TextElement.FontSizeProperty, ThemeResources.Font.Size.Get(FontSize.Sm).Value),
            x.SetProperty(Visual.OpacityProperty, x.GetResourceObservable(ThemeResourceKeyFactory.Opacity(nameof(Opacity.Medium))))
        });

        ClassRegistry.Register<Control>(CssClass.TextWatermark, x => new CompositeDisposable
        {
            x.SetProperty(Visual.OpacityProperty, x.GetResourceObservable(ThemeResourceKeyFactory.Opacity(nameof(Opacity.Medium)))),
            x.SetProperty(TextElement.FontSizeProperty, new ReflectionBinding("(TextElement.FontSize)")
            {
                RelativeSource = new(RelativeSourceMode.FindAncestor)
                {
                    AncestorType = typeof(Control)
                },
                Converter = MathConverter.Multiply,
                ConverterParameter = 0.75,
                TypeResolver = ResolveType
            })
        });

        // Headers
        ClassRegistry.RegisterMany<FontSize, Control>(CssPrefix.Header, (x, y) => new CompositeDisposable
        {
            x.SetProperty(HeaderAssist.FontSizeProperty, ThemeResources.Font.Size.Get(y).Value),
            y.ToString().StartsWith('H') ? x.SetProperty(HeaderAssist.FontWeightProperty, ThemeResources.Font.Weight.Header.Value) : Disposable.Empty
        });

        ClassRegistry.Register<Control>(CssClass.HeaderHelper, x => new CompositeDisposable
        {
            x.SetProperty(HeaderAssist.FontSizeProperty, ThemeResources.Font.Size.Get(FontSize.Sm).Value),
            x.SetProperty(HeaderAssist.OpacityProperty, x.GetResourceObservable(ThemeResourceKeyFactory.Opacity(nameof(Opacity.Medium))))
        });

        ClassRegistry.Register<Control>(CssClass.HeaderWatermark, x => new CompositeDisposable
        {
            x.SetProperty(HeaderAssist.OpacityProperty, x.GetResourceObservable(ThemeResourceKeyFactory.Opacity(nameof(Opacity.Medium)))),
            x.SetProperty(HeaderAssist.FontSizeProperty, new ReflectionBinding("(TextElement.FontSize)")
            {
                RelativeSource = new(RelativeSourceMode.FindAncestor)
                {
                    AncestorType = typeof(Control)
                },
                Converter = MathConverter.Multiply,
                ConverterParameter = 0.75,
                TypeResolver = ResolveType
            })
        });
    }

    /// <summary>
    /// Registers font-related properties for use in styling controls and headers within the application.
    /// </summary>
    /// <remarks>This method enables consistent application of font weight, style, and stretch properties by
    /// registering them with the appropriate CSS prefixes for both general controls and header elements. Call this
    /// method during application initialization to ensure that font styling options are available throughout the user
    /// interface.</remarks>
    public static void RegisterFonts()
    {
        ClassRegistry.RegisterMany<FontSize, Control>(CssPrefix.Font, (x, y) => x.SetProperty(TextElement.FontSizeProperty, ThemeResources.Font.Size.Get(y).Value));
        ClassRegistry.RegisterMany<FontWeight, Control>(CssPrefix.Font, (x, y) => x.SetProperty(TextElement.FontWeightProperty, y));
        ClassRegistry.RegisterMany<FontStyle, Control>(CssPrefix.Font, (x, y) => x.SetProperty(TextElement.FontStyleProperty, y));
        ClassRegistry.RegisterMany<FontStretch, Control>(CssPrefix.Font, (x, y) => x.SetProperty(TextElement.FontStretchProperty, y));

        ClassRegistry.RegisterMany<FontSize, Control>(CssPrefix.Header, (x, y) => x.SetProperty(HeaderAssist.FontSizeProperty, ThemeResources.Font.Size.Get(y).Value));
        ClassRegistry.RegisterMany<FontWeight, Control>(CssPrefix.Header, (x, y) => x.SetProperty(HeaderAssist.FontWeightProperty, y));
        ClassRegistry.RegisterMany<FontStyle, Control>(CssPrefix.Header, (x, y) => x.SetProperty(HeaderAssist.FontStyleProperty, y));
    }

    /// <summary>
    /// Registers text decoration properties for TextBlock controls, allowing for the application of underline and strikethrough styles based on CSS classes. This method sets up handlers that modify the text decoration state of TextBlock elements when specific CSS classes are applied, enabling dynamic styling of text decorations in the user interface. Call this method during application startup to ensure that text decoration utilities are available for use throughout the application.
    /// </summary>
    public static void RegisterDecorations()
    {
        ClassRegistry.Register<TextBlock>(CssClass.TextUnderline, x => ClassContext.Create<TextBlock, ControlState>(x).Update(s => s.IsUnderline = true, ApplyState));
        ClassRegistry.Register<TextBlock>(CssClass.TextStrikethrough, x => ClassContext.Create<TextBlock, ControlState>(x).Update(s => s.IsStrikethrough = true, ApplyState));
    }

    /// <summary>
    /// Resolves a type based on the provided type name. This method is used to convert a string representation of a type into an actual Type object, which can be utilized in bindings or other scenarios where type information is required. The method currently supports resolving the "TextElement" type, and will throw an exception if an unsupported type name is provided.
    /// </summary>
    /// <param name="namespace">The namespace of the type to resolve.</param>
    /// <param name="typeName">The name of the type to resolve.</param>
    /// <returns>The resolved Type object.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the type name cannot be resolved.</exception>
    private static Type ResolveType(string? @namespace, string typeName) => typeName switch
    {
        nameof(TextElement) => typeof(TextElement),
        _ => throw new InvalidOperationException($"Cannot resolve type '{typeName}'")
    };
}
