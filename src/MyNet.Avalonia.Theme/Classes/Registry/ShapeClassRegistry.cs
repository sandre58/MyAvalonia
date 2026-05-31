// -----------------------------------------------------------------------
// <copyright file="ShapeClassRegistry.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using MyNet.Avalonia.Theme.Assists;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.Avalonia.Theme.MarkupExtensions;

namespace MyNet.Avalonia.Theme.Classes.Registry;

/// <summary>
/// Provides utility methods for registering and managing shape-related properties for various UI controls.
/// </summary>
/// <remarks>The ShapeClassRegistry class centralizes the registration of shape settings, such as spacing, for controls
/// including StackPanel, WrapPanel, UniformGrid, and Grid. This enables consistent application of spacing values across
/// different control types, facilitating uniform layout and appearance in user interfaces.</remarks>
public static class ShapeClassRegistry
{
    /// <summary>
    /// Registers shape settings for various layout controls, allowing for consistent spacing configurations across
    /// different control types.
    /// </summary>
    /// <remarks>This method utilizes the UtilityRegistry to apply spacing sizes to controls such as
    /// StackPanel, WrapPanel, UniformGrid, and Grid. It sets the appropriate spacing properties based on the control
    /// type, ensuring that the layout adheres to the specified shape parameters.</remarks>
    public static void Register()
    {
        ClassRegistry.Register<TemplatedControl>(CssClass.ShapeCircle, x => new CompositeDisposable
        {
            ThemeAssist.GetCategory(x) == ControlCategory.Input ? x.SetProperty(TemplatedControl.PaddingProperty, new Thickness(8, 0, 0, 0)) : Disposable.Empty,
            ThemeAssist.GetCategory(x) == ControlCategory.Input ? x.SetProperty(InputAssist.InnerPaddingProperty, new Thickness(0, 4, 8, 4)) : Disposable.Empty,
            x.SetProperty(TemplatedControl.CornerRadiusProperty, ThemeResources.Corners.Get(CornerSize.Round).Value),
            x.SetProperty(Control.FocusAdornerProperty, new FuncTemplate<Control>(() =>
            {
                var border = new Border
                {
                    BorderThickness = new(2)
                };

                border.SetProperty(TemplatedControl.CornerRadiusProperty, ThemeResources.Corners.Get(CornerSize.Round).Value);
                border.SetProperty(TemplatedControl.BorderBrushProperty, new ThemeExtension("Control.Border.Focus"));

                return border;
            }))
        });
        ClassRegistry.Register<TemplatedControl>(CssClass.ShapeItemsCircle, x => x.SetProperty(ItemsAssist.CornerRadiusProperty, ThemeResources.Corners.Get(CornerSize.Round).Value));
    }
}
