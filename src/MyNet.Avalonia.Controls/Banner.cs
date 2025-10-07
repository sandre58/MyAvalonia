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
using MyNet.Avalonia.Extensions;

namespace MyNet.Avalonia.Controls;

[PseudoClasses(PseudoClassName.Error, PseudoClassName.Warning, PseudoClassName.Information, PseudoClassName.Success)]
[TemplatePart(PartCloseButton, typeof(Button))]
public class Banner : HeaderedContentControl
{
    public const string PartCloseButton = "PART_CloseButton";

    private Button? _closeButton;

    public static readonly StyledProperty<bool> CanCloseProperty = AvaloniaProperty.Register<Banner, bool>(nameof(CanClose), true);

    public bool CanClose
    {
        get => GetValue(CanCloseProperty);
        set => SetValue(CanCloseProperty, value);
    }

    #region Severity

    /// <summary>
    /// Provides Severity Property.
    /// </summary>
    public static readonly StyledProperty<Severity> SeverityProperty = AvaloniaProperty.Register<Banner, Severity>(nameof(Severity), Severity.Custom);

    /// <summary>
    /// Gets or sets the Severity property.
    /// </summary>
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
        {
            UpdateSeverity();
        }
    }

    private void OnCloseClick(object? sender, RoutedEventArgs args) => IsVisible = false;

    private void UpdateSeverity()
    {
        switch (Severity)
        {
            case Severity.Error:
                PseudoClasses.Add(PseudoClassName.Error);
                break;

            case Severity.Information:
                PseudoClasses.Add(PseudoClassName.Information);
                break;

            case Severity.Success:
                PseudoClasses.Add(PseudoClassName.Success);
                break;

            case Severity.Warning:
                PseudoClasses.Add(PseudoClassName.Warning);
                break;
            case Severity.Custom:
            default:
                break;
        }
    }
}
