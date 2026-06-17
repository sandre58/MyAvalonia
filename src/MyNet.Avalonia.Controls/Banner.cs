// -----------------------------------------------------------------------
// <copyright file="Banner.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Controls.Primitives;

namespace MyNet.Avalonia.Controls;

/// <summary>
/// Inline contextual message (info, warning, success). Non-blocking and optionally dismissable.
/// </summary>
/// <remarks>
/// <para><see cref="RegionControl.Header"/> is the primary heading; <see cref="ContentControl.Content"/> is the optional detail body.
/// For page sections, use <see cref="HeaderedContentControl"/> instead.</para>
/// <para>Leading icons use a <strong>plain</strong> presentation by default (no badge). Add the <c>leading-badge</c> style class
/// for a tonal rounded background behind custom leading content on neutral surfaces.</para>
/// </remarks>
[PseudoClasses(PseudoClassName.Error, PseudoClassName.Warning, PseudoClassName.Information, PseudoClassName.Success, PseudoClassName.HeaderEmpty)]
[TemplatePart(PartCloseButton, typeof(Button))]
public class Banner : RegionControl
{
    public const string PartCloseButton = "PART_CloseButton";

    static Banner() => HeaderProperty.Changed.AddClassHandler<Banner, object?>((banner, _) => banner.UpdateHeaderEmptyPseudoClass());

    public Banner() => UpdateHeaderEmptyPseudoClass();

    private Button? _closeButton;

    public static readonly StyledProperty<bool> CanCloseProperty = AvaloniaProperty.Register<Banner, bool>(nameof(CanClose), true);

    public bool CanClose
    {
        get => GetValue(CanCloseProperty);
        set => SetValue(CanCloseProperty, value);
    }

    #region Severity

    public static readonly StyledProperty<Severity> SeverityProperty =
        AvaloniaProperty.Register<Banner, Severity>(nameof(Severity));

    public Severity Severity
    {
        get => GetValue(SeverityProperty);
        set => SetValue(SeverityProperty, value);
    }

    #endregion

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        Button.ClickEvent.RemoveHandler(OnCloseClick, _closeButton);
        _closeButton = e.NameScope.Find<Button>(PartCloseButton);
        Button.ClickEvent.AddHandler(OnCloseClick, _closeButton);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == SeverityProperty)
            UpdateSeverity();
    }

    private void OnCloseClick(object? sender, RoutedEventArgs args) => IsVisible = false;

    private void UpdateSeverity()
    {
        PseudoClasses.Set(PseudoClassName.Error, Severity == Severity.Error);
        PseudoClasses.Set(PseudoClassName.Information, Severity == Severity.Information);
        PseudoClasses.Set(PseudoClassName.Success, Severity == Severity.Success);
        PseudoClasses.Set(PseudoClassName.Warning, Severity == Severity.Warning);
    }
}
