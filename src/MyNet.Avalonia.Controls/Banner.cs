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

[PseudoClasses(PseudoClassName.Error, PseudoClassName.Warning, PseudoClassName.Information, PseudoClassName.Success)]
[TemplatePart(PartCloseButton, typeof(Button))]
public class Banner : RegionControl
{
    public const string PartCloseButton = "PART_CloseButton";

    private Button? _closeButton;

    public static readonly StyledProperty<bool> CanCloseProperty =
        AvaloniaProperty.Register<Banner, bool>(nameof(CanClose), true);

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
