// -----------------------------------------------------------------------
// <copyright file="ThemeAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using MyNet.Avalonia.Theme.Palettes;

namespace MyNet.Avalonia.Theme.Assists;

public static class ThemeAssist
{
    static ThemeAssist()
    {
        RoleProperty.Changed.AddClassHandler<AvaloniaObject>(OnRoleChangedCallback);
        ContextProperty.Changed.AddClassHandler<AvaloniaObject>(OnContextChangedCallback);
        FlyoutBase.AttachedFlyoutProperty.Changed.AddClassHandler<Control>(OnAttachedFlyoutChanged);
        Control.ContextFlyoutProperty.Changed.AddClassHandler<Control>(OnContextFlyoutChanged);
        Control.ContextMenuProperty.Changed.AddClassHandler<Control>(OnContextMenuChanged);
    }

    #region Context

    /// <summary>
    /// Provides Context Property for attached ThemeAssist element.
    /// </summary>
    public static readonly AttachedProperty<ThemeContext> ContextProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, ThemeContext>("Context", typeof(ThemeAssist), inherits: true);

    /// <summary>
    /// Accessor for Attached  <see cref="ContextProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ContextProperty"/>.</param>
    public static void SetContext(AvaloniaObject element, ThemeContext value) => element.SetValue(ContextProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ContextProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static ThemeContext GetContext(AvaloniaObject element) => element.GetValue(ContextProperty);

    private static void OnContextChangedCallback(AvaloniaObject avaloniaObject, AvaloniaPropertyChangedEventArgs args)
    {
        switch (avaloniaObject)
        {
            case Button button:
                PropagateContextToPopup(button, button.Flyout);
                PropagateContextToPopup(button, button.ContextFlyout);
                PropagateContextToPopup(button, button.ContextMenu);
                break;

            case Control control:
                PropagateContextToPopup(control, control.ContextMenu);
                PropagateContextToPopup(control, control.ContextFlyout);
                break;
        }
    }

    private static void OnAttachedFlyoutChanged(Control control, AvaloniaPropertyChangedEventArgs args)
    {
        if (args.NewValue is FlyoutBase flyout)
        {
            PropagateContextToPopup(control, flyout);
        }
    }

    private static void OnContextFlyoutChanged(Control control, AvaloniaPropertyChangedEventArgs args)
    {
        if (args.NewValue is FlyoutBase flyout)
        {
            PropagateContextToPopup(control, flyout);
        }
    }

    private static void OnContextMenuChanged(Control control, AvaloniaPropertyChangedEventArgs args)
    {
        if (args.NewValue is ContextMenu contextMenu)
        {
            PropagateContextToPopup(control, contextMenu);
        }
    }

    private static void PropagateContextToPopup(Control source, AvaloniaObject? popup)
    {
        switch (popup)
        {
            case null:
                return;
            case FlyoutBase flyout:
                flyout.Opened += (_, _) =>
                {
                    var context = GetContext(source);
                    SetContext(flyout, context);
                };
                break;
            case ContextMenu contextMenu:
                contextMenu.Opened += (_, _) =>
                {
                    var context = GetContext(source);
                    SetContext(contextMenu, context);
                };
                break;
        }
    }

    #endregion

    #region Role

    /// <summary>
    /// Defines the Role attached property for assigning a semantic color role to a control.
    /// </summary>
    public static readonly AttachedProperty<ThemeRole> RoleProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, ThemeRole>("Role", typeof(ThemeAssist));

    /// <summary>
    /// Gets the theme role for the specified control.
    /// </summary>
    /// <param name="element">The control to query.</param>
    /// <returns>The assigned theme role.</returns>
    public static ThemeRole GetRole(AvaloniaObject element) => element.GetValue(RoleProperty);

    /// <summary>
    /// Sets the theme role for the specified control.
    /// </summary>
    /// <param name="element">The control to update.</param>
    /// <param name="value">The theme role to assign.</param>
    public static void SetRole(AvaloniaObject element, ThemeRole value) => element.SetValue(RoleProperty, value);

    private static void OnRoleChangedCallback(AvaloniaObject avaloniaObject, AvaloniaPropertyChangedEventArgs args)
    {
        if (avaloniaObject is StyledElement c)
        {
            SetHasRole(c, args.NewValue is ThemeRole role && role != ThemeRole.Default);
        }
    }

    #endregion

    #region HasRole

    /// <summary>
    /// Provides HasRole Property for attached ThemeAssist element.
    /// </summary>
    public static readonly AttachedProperty<bool> HasRoleProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("HasRole", typeof(ThemeAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="HasRoleProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="HasRoleProperty"/>.</param>
    private static void SetHasRole(StyledElement element, bool value) => element.SetValue(HasRoleProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="HasRoleProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetHasRole(StyledElement element) => element.GetValue(HasRoleProperty);

    #endregion

    #region Kind

    /// <summary>
    /// Provides Kind Property for attached ThemeAssist element.
    /// </summary>
    public static readonly AttachedProperty<string> KindProperty = AvaloniaProperty.RegisterAttached<StyledElement, string>("Kind", typeof(ThemeAssist), "default");

    /// <summary>
    /// Accessor for Attached  <see cref="KindProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="KindProperty"/>.</param>
    public static void SetKind(StyledElement element, string value) => element.SetValue(KindProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="KindProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static string GetKind(StyledElement element) => element.GetValue(KindProperty);

    #endregion
}
