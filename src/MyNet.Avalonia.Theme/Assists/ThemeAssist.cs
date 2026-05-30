// -----------------------------------------------------------------------
// <copyright file="ThemeAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using MyNet.Avalonia.Theme.Classes;
using MyNet.Avalonia.Theme.Classes.Helpers;
using MyNet.Avalonia.Theme.Theming.Core;

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
    public static readonly AttachedProperty<ThemeContext> ContextProperty = AvaloniaPropertyHelper.RegisterEnumProperty("Context", typeof(ThemeAssist), ThemeContext.Default, CssPrefix.Context, true);

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
                flyout.Opened -= onOpened;
                flyout.Opened += onOpened;
                break;

            case ContextMenu contextMenu:
                contextMenu.Opened -= onOpened;
                contextMenu.Opened += onOpened;
                break;
        }

        void applyContext()
        {
            var context = GetContext(source);
            SetContext(popup, context);
        }

        void onOpened(object? sender, EventArgs e) => applyContext();
    }

    #endregion

    #region Role

    /// <summary>
    /// Defines the Role attached property for assigning a semantic color role to a control.
    /// </summary>
    public static readonly AttachedProperty<ThemeRole> RoleProperty = AvaloniaPropertyHelper.RegisterEnumProperty("Role", typeof(ThemeAssist), ThemeRole.Default, CssPrefix.Role);

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
    public static readonly AttachedProperty<bool> HasRoleProperty = AvaloniaPropertyHelper.RegisterBoolProperty("HasRole", CssClass.HasRole);

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

    #region Category

    /// <summary>
    /// Provides Category Property for attached ThemeAssist element.
    /// </summary>
    public static readonly AttachedProperty<ControlCategory> CategoryProperty = AvaloniaPropertyHelper.RegisterEnumProperty("Category", typeof(ThemeAssist), ControlCategory.Unknown, CssPrefix.Category);

    /// <summary>
    /// Accessor for Attached  <see cref="CategoryProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="CategoryProperty"/>.</param>
    public static void SetCategory(StyledElement element, ControlCategory value) => element.SetValue(CategoryProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="CategoryProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static ControlCategory GetCategory(StyledElement element) => element.GetValue(CategoryProperty);

    #endregion

    #region Kind

    /// <summary>
    /// Provides Kind Property for attached ThemeAssist element.
    /// </summary>
    public static readonly AttachedProperty<string> KindProperty = AvaloniaPropertyHelper.RegisterStringProperty("Kind", typeof(ThemeAssist), "default", CssPrefix.Kind);

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

/// <summary>
/// Defines categories of controls for theming purposes, allowing for consistent styling and behavior across different types of UI elements.
/// </summary>
public enum ControlCategory
{
    /// <summary>
    /// Represents an unspecified or undetermined category or value.
    /// </summary>
    /// <remarks>Use this value as a placeholder when the specific category or value is not known or cannot be
    /// determined at compile time. This is commonly used in scenarios where category information is unavailable or
    /// deferred.</remarks>
    Unknown,

    /// <summary>
    /// Represents a surface that can be rendered or interacted with in a graphical context.
    /// </summary>
    /// <remarks>This class provides methods and properties to manipulate the surface's appearance and
    /// behavior. It is commonly used in graphical applications to define areas where drawing or user interaction
    /// occurs.</remarks>
    Surface,

    /// <summary>
    /// Represents an input control that allows users to enter or manipulate data, such as text boxes, sliders,
    /// or other interactive elements.
    /// </summary>
    /// <remarks>This control is commonly used in user interfaces to capture user input or provide interactive functionality.
    /// It may have various properties and events to handle user interactions and data validation.</remarks>
    Input,

    /// <summary>
    /// Represents a delegate that encapsulates a method with no parameters and does not return a value.
    /// </summary>
    /// <remarks>Use this delegate to pass methods as arguments for callbacks, event handling, or deferred
    /// execution scenarios where no input or output is required. This delegate is commonly used in asynchronous
    /// programming and command patterns.</remarks>
    Action,

    /// <summary>
    /// Represents a navigation element that facilitates user movement within the application.
    /// </summary>
    /// <remarks>This class may include methods and properties that allow for navigating between different
    /// views or pages in the application. It is essential for implementing a user-friendly interface that enhances user
    /// experience.</remarks>
    Navigation,

    /// <summary>
    /// Represents a selection of items within a collection, allowing for operations on the selected items.
    /// </summary>
    /// <remarks>This class provides methods to manipulate and retrieve information about the selected items.
    /// It is commonly used in scenarios where user interaction with a list or grid is required, such as in UI
    /// applications.</remarks>
    Selection,

    /// <summary>
    /// Represents a visual indicator that provides feedback or status information within a user interface.
    /// </summary>
    /// <remarks>Use this class to display various states, such as loading, success, or error, to inform users
    /// about ongoing processes or outcomes. The indicator is designed to be customizable to accommodate different
    /// design requirements and can be integrated into a variety of UI scenarios.</remarks>
    Indicator
}
