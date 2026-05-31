// -----------------------------------------------------------------------
// <copyright file="FocusClassRegistry.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Controls.Templates;

namespace MyNet.Avalonia.Theme.Classes.Registry;

/// <summary>
/// Provides utility methods for registering and managing focus-related properties for various UI controls.
/// </summary>
/// <remarks>The FocusClassRegistry class centralizes the registration of focus settings, such as focus indicators, for controls
/// including StackPanel, WrapPanel, UniformGrid, and Grid. This enables consistent application of focus values across
/// different control types, facilitating uniform focus behavior and appearance in user interfaces.</remarks>
public static class FocusClassRegistry
{
    /// <summary>
    /// Registers focus settings for various layout controls, allowing for consistent focus configurations across
    /// different control types.
    /// </summary>
    /// <remarks>This method utilizes the UtilityRegistry to apply focus settings to controls such as
    /// StackPanel, WrapPanel, UniformGrid, and Grid. It sets the appropriate focus properties based on the control
    /// type, ensuring that the layout adheres to the specified focus parameters.</remarks>
    public static void Register()
    {
        ClassRegistry.Register<Control>(CssClass.FocusRectangle, x => x.SetProperty(Control.FocusAdornerProperty, new FuncTemplate<Control>(() =>
            {
                var rectangle = new Rectangle();
                rectangle.AddClasses(CssClass.KindFocus.ToString());

                return rectangle;
            })));
        ClassRegistry.Register<Control>(CssClass.FocusEllipse, x => x.SetProperty(Control.FocusAdornerProperty, new FuncTemplate<Control>(() =>
        {
                var rectangle = new Ellipse();
                rectangle.AddClasses(CssClass.KindFocus.ToString());

                return rectangle;
            })));
        ClassRegistry.Register<Control>(CssClass.FocusHidden, x => x.SetProperty(Control.FocusAdornerProperty, null));
    }
}
