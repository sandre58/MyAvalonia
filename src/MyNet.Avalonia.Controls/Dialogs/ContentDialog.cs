// -----------------------------------------------------------------------
// <copyright file="ContentDialog.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Automation;
using Avalonia.Automation.Peers;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using MyNet.Avalonia.Controls.Primitives;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class ContentDialog : FullContentControl
{
    static ContentDialog()
    {
        PaddingProperty.OverrideDefaultValue<ContentDialog>(new(10));
        MarginProperty.OverrideDefaultValue<ContentDialog>(new(0));
        AutomationProperties.ControlTypeOverrideProperty.OverrideDefaultValue<ContentDialog>(AutomationControlType.Pane);
        HeaderProperty.Changed.AddClassHandler<ContentDialog, object?>((dialog, _) => UpdateAutomationName(dialog));
    }

    #region ShowHeader

    /// <summary>
    /// Provides ShowHeader Property.
    /// </summary>
    public static readonly StyledProperty<bool> ShowHeaderProperty =
        AvaloniaProperty.Register<ContentDialog, bool>(nameof(ShowHeader), true);

    /// <summary>
    /// Gets or sets a value indicating whether the dialog header is shown in the content template.
    /// Set to <see langword="false"/> when the overlay shell already displays <see cref="Header"/>.
    /// </summary>
    public bool ShowHeader
    {
        get => GetValue(ShowHeaderProperty);
        set => SetValue(ShowHeaderProperty, value);
    }

    #endregion

    #region StartupLocation

    /// <summary>
    /// Provides StartupLocation Property.
    /// </summary>
    public static readonly StyledProperty<WindowStartupLocation> StartupLocationProperty = AvaloniaProperty.Register<ContentDialog, WindowStartupLocation>(nameof(StartupLocation));

    /// <summary>
    /// Gets or sets the StartupLocation property.
    /// </summary>
    public WindowStartupLocation StartupLocation
    {
        get => GetValue(StartupLocationProperty);
        set => SetValue(StartupLocationProperty, value);
    }

    #endregion

    #region Position

    /// <summary>
    /// Provides Position Property.
    /// </summary>
    public static readonly StyledProperty<PixelPoint?> PositionProperty = AvaloniaProperty.Register<ContentDialog, PixelPoint?>(nameof(Position));

    /// <summary>
    /// Gets or sets the Position property.
    /// </summary>
    public PixelPoint? Position
    {
        get => GetValue(PositionProperty);
        set => SetValue(PositionProperty, value);
    }

    #endregion

    #region ShowCloseButton

    /// <summary>
    /// Provides ShowCloseButton Property.
    /// </summary>
    public static readonly StyledProperty<bool> ShowCloseButtonProperty = AvaloniaProperty.Register<ContentDialog, bool>(nameof(ShowCloseButton), true);

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets the ShowCloseButton property.
    /// </summary>
    public bool ShowCloseButton
    {
        get => GetValue(ShowCloseButtonProperty);
        set => SetValue(ShowCloseButtonProperty, value);
    }

    #endregion

    #region ShowInTaskBar

    /// <summary>
    /// Provides ShowInTaskBar Property.
    /// </summary>
    public static readonly StyledProperty<bool> ShowInTaskBarProperty = AvaloniaProperty.Register<ContentDialog, bool>(nameof(ShowInTaskBar), true);

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets the ShowInTaskBar property.
    /// </summary>
    public bool ShowInTaskBar
    {
        get => GetValue(ShowInTaskBarProperty);
        set => SetValue(ShowInTaskBarProperty, value);
    }

    #endregion

    #region CanResize

    /// <summary>
    /// Provides CanResize Property.
    /// </summary>
    public static readonly StyledProperty<bool> CanResizeProperty = AvaloniaProperty.Register<ContentDialog, bool>(nameof(CanResize));

    /// <summary>
    /// Gets or sets a value indicating whether the window shell may be resized. Ignored for overlay presentation.
    /// </summary>
    public bool CanResize
    {
        get => GetValue(CanResizeProperty);
        set => SetValue(CanResizeProperty, value);
    }

    #endregion

    #region ParentClasses

    /// <summary>
    /// Provides ParentClasses Property.
    /// </summary>
    public static readonly StyledProperty<string?> ParentClassesProperty = AvaloniaProperty.Register<ContentDialog, string?>(nameof(ParentClasses));

    /// <summary>
    /// Gets or sets the ParentClasses property.
    /// </summary>
    public string? ParentClasses
    {
        get => GetValue(ParentClassesProperty);
        set => SetValue(ParentClassesProperty, value);
    }

    #endregion

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        DialogKeyboardNavigation.HandleKeyDown(this, e);
    }

    private static void UpdateAutomationName(ContentDialog dialog)
        => AutomationProperties.SetName(dialog, dialog.Header?.ToString() ?? string.Empty);
}

/// <summary>
/// Keyboard shortcuts for confirmation-style content dialogs.
/// </summary>
internal static class DialogKeyboardNavigation
{
    public static void HandleKeyDown(Control scope, KeyEventArgs e)
    {
        if (e.Handled)
            return;

        if (e.Key == Key.Escape)
        {
            if (TryExecuteButton(FindDismissButton(scope)))
                e.Handled = true;

            return;
        }

        if (e.Key is not Key.Enter || e.KeyModifiers != KeyModifiers.None || ShouldIgnoreEnter(e.Source))
            return;

        if (TryExecuteButton(FindAffirmativeButton(scope)))
            e.Handled = true;
    }

    private static bool ShouldIgnoreEnter(object? source)
        => source is TextBox textBox && (textBox.AcceptsReturn || textBox.AcceptsTab);

    private static Button? FindAffirmativeButton(Control scope)
        => FindButton(scope, static button => button.IsDefault)
           ?? FindButton(scope, static button => button.Classes.Contains("variant-filled"));

    private static Button? FindDismissButton(Control scope)
        => FindButton(scope, static button => button.Classes.Contains("variant-text"));

    private static Button? FindButton(Control scope, Func<Button, bool> predicate)
    {
        foreach (var element in scope.GetVisualDescendants())
        {
            if (element is not Button { IsVisible: true, IsEffectivelyEnabled: true } candidate)
                continue;

            if (predicate(candidate))
                return candidate;
        }

        return null;
    }

    private static bool TryExecuteButton(Button? button)
    {
        if (button is not { IsVisible: true, IsEffectivelyEnabled: true })
            return false;

        if (button.Command?.CanExecute(button.CommandParameter) == true)
        {
            button.Command.Execute(button.CommandParameter);
            return true;
        }

        button.RaiseEvent(new(Button.ClickEvent));
        return true;
    }
}
