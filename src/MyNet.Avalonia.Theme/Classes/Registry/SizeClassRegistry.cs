// -----------------------------------------------------------------------
// <copyright file="SizeClassRegistry.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Reactive.Disposables;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Layout;
using MyNet.Avalonia.Converters;
using MyNet.Avalonia.Extensions;

namespace MyNet.Avalonia.Theme.Classes.Registry;

/// <summary>
/// Provides utility methods for configuring and managing layout strategies for items within an ItemsControl, enabling
/// flexible arrangement of child elements using stack, uniform, or wrap layouts.
/// </summary>
/// <remarks>SizeClassRegistry allows developers to register and apply different layout behaviors to ItemsControl
/// elements by associating them with specific layout kinds and orientations. This facilitates dynamic and consistent
/// layout customization across controls, supporting scenarios such as horizontal or vertical stacking, uniform grids,
/// and wrapping panels. The class manages layout state through attached properties to ensure that changes are applied
/// reliably. This utility is intended for use in scenarios where layout flexibility and runtime configuration are
/// required for collections of items.</remarks>
public static class SizeClassRegistry
{
    /// <summary>
    /// Registers default layout configurations for supported controls, enabling consistent orientation and layout
    /// behaviors across StackPanel, WrapPanel, and ItemsControl instances.
    /// </summary>
    /// <remarks>Call this method during application initialization to ensure that flex-related utility
    /// registrations are in place before creating or displaying UI elements. This setup allows controls to respond to
    /// flex-related CSS classes and orientation settings, facilitating uniform styling and alignment throughout the
    /// application.</remarks>
    public static void Register() =>
        ClassRegistry.Register<TemplatedControl>(CssClass.Size(CssSuffix.Half), x => new CompositeDisposable
        {
            x.SetProperty(Layoutable.HeightProperty, new Binding("Bounds.Height")
            {
                RelativeSource = new(RelativeSourceMode.FindAncestor)
                {
                    AncestorType = typeof(TemplatedControl)
                },
                Converter = MathConverter.Divide,
                ConverterParameter = 2.0
            })
        });
}
