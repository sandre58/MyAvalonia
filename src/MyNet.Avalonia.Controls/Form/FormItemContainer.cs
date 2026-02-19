// -----------------------------------------------------------------------
// <copyright file="FormItemContainer.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Avalonia;
using Avalonia.Automation;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Layout;
using MyNet.Avalonia.Controls.Enums;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// A container for form item content with label support.
/// </summary>
[PseudoClasses(PseudoClassName.Left, PseudoClassName.Right, PseudoClassName.Top, PseudoClassName.Bottom, PseudoClassName.Invalid)]
[TemplatePart(PartLabel, typeof(Label), IsRequired = true)]
[TemplatePart(PartGrid, typeof(Grid), IsRequired = true)]
public class FormItemContainer : ContentControl
{
    public const string PartLabel = "PART_Label";
    public const string PartGrid = "PART_Grid";

    private static readonly Dictionary<(Type, string), PropertyInfo?> PropertyCache = [];

    private Label? _label;
    private Grid? _grid;
    private double _effectiveLabelWidth;

    static FormItemContainer()
    {
        LabelPositionProperty.Changed.AddClassHandler<FormItemContainer>((x, _) => x.UpdatePseudoClasses());
        EffectiveLabelWidthProperty.Changed.AddClassHandler<FormItemContainer>((x, _) => x.UpdateGridColumnDefinitions());
        AffectsMeasure<FormItemContainer>(PanelComputedWidthProperty);
    }

    /// <summary>
    /// Gets the effective width used for the label after considering LabelWidth settings and panel calculations.
    /// This is the actual width that will be applied to the label in the template.
    /// </summary>
    public static readonly DirectProperty<FormItemContainer, double> EffectiveLabelWidthProperty =
        AvaloniaProperty.RegisterDirect<FormItemContainer, double>(
            nameof(EffectiveLabelWidth),
            o => o.EffectiveLabelWidth);

    /// <summary>
    /// Internal property used by FormItemsPanel to communicate the computed max label width.
    /// </summary>
    internal static readonly StyledProperty<double> PanelComputedWidthProperty = AvaloniaProperty.Register<FormItemContainer, double>(nameof(PanelComputedWidth), 0d);

    /// <summary>
    /// Gets the effective label width that will be used in the template.
    /// Takes into account: LabelWidth setting, panel computed width, and measured width.
    /// </summary>
    public double EffectiveLabelWidth
    {
        get => _effectiveLabelWidth;
        private set => SetAndRaise(EffectiveLabelWidthProperty, ref _effectiveLabelWidth, value);
    }

    /// <summary>
    /// Gets or sets the computed max label width from the panel.
    /// Used as input for calculating EffectiveLabelWidth.
    /// </summary>
    internal double PanelComputedWidth
    {
        get => GetValue(PanelComputedWidthProperty);
        set => SetValue(PanelComputedWidthProperty, value);
    }

    internal bool IsLabelWidthAuto => LabelWidth.IsAuto;

    internal bool IsLabelWidthStar => LabelWidth.IsStar;

    /// <summary>
    /// Defines the <see cref="Label"/> property.
    /// </summary>
    public static readonly StyledProperty<object?> LabelProperty = AvaloniaProperty.Register<FormItemContainer, object?>(nameof(Label));

    /// <summary>
    /// Defines the <see cref="LabelTemplate"/> property.
    /// </summary>
    public static readonly StyledProperty<IDataTemplate?> LabelTemplateProperty = AvaloniaProperty.Register<FormItemContainer, IDataTemplate?>(nameof(LabelTemplate));

    /// <summary>
    /// Defines the <see cref="ShowLabel"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> ShowLabelProperty = AvaloniaProperty.Register<FormItemContainer, bool>(nameof(ShowLabel), defaultValue: true);

    /// <summary>
    /// Defines the <see cref="LabelPosition"/> property.
    /// </summary>
    public static readonly StyledProperty<Position> LabelPositionProperty = AvaloniaProperty.Register<FormItemContainer, Position>(nameof(LabelPosition), defaultValue: Position.Left);

    /// <summary>
    /// Defines the <see cref="LabelWidth"/> property.
    /// </summary>
    public static readonly StyledProperty<GridLength> LabelWidthProperty = AvaloniaProperty.Register<FormItemContainer, GridLength>(nameof(LabelWidth), defaultValue: GridLength.Auto);

    /// <summary>
    /// Defines the <see cref="LabelAlignment"/> property.
    /// </summary>
    public static readonly StyledProperty<HorizontalAlignment> LabelAlignmentProperty = AvaloniaProperty.Register<FormItemContainer, HorizontalAlignment>(nameof(LabelAlignment), defaultValue: HorizontalAlignment.Left);

    /// <summary>
    /// Defines the <see cref="LabelMargin"/> property.
    /// </summary>
    public static readonly StyledProperty<Thickness> LabelMarginProperty = AvaloniaProperty.Register<FormItemContainer, Thickness>(nameof(LabelMargin), defaultValue: new Thickness(0, 0, 8, 0));

    /// <summary>
    /// Defines the <see cref="IsRequired"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> IsRequiredProperty = AvaloniaProperty.Register<FormItemContainer, bool>(nameof(IsRequired), defaultValue: false);

    /// <summary>
    /// Defines the <see cref="RequiredIndicator"/> property.
    /// </summary>
    public static readonly StyledProperty<string?> RequiredIndicatorProperty = AvaloniaProperty.Register<FormItemContainer, string?>(nameof(RequiredIndicator), defaultValue: "*");

    /// <summary>
    /// Defines the <see cref="HelpText"/> property.
    /// </summary>
    public static readonly StyledProperty<string?> HelpTextProperty = AvaloniaProperty.Register<FormItemContainer, string?>(nameof(HelpText));

    /// <summary>
    /// Defines the <see cref="TextWrapping"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> TextWrappingProperty = AvaloniaProperty.Register<FormItemContainer, bool>(nameof(TextWrapping), defaultValue: false);

    /// <summary>
    /// Gets or sets the label content.
    /// </summary>
    public object? Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    /// <summary>
    /// Gets or sets the label template.
    /// </summary>
    public IDataTemplate? LabelTemplate
    {
        get => GetValue(LabelTemplateProperty);
        set => SetValue(LabelTemplateProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets whether to show the label.
    /// </summary>
    public bool ShowLabel
    {
        get => GetValue(ShowLabelProperty);
        set => SetValue(ShowLabelProperty, value);
    }

    /// <summary>
    /// Gets or sets the label position.
    /// </summary>
    public Position LabelPosition
    {
        get => GetValue(LabelPositionProperty);
        set => SetValue(LabelPositionProperty, value);
    }

    /// <summary>
    /// Gets or sets the label width.
    /// </summary>
    public GridLength LabelWidth
    {
        get => GetValue(LabelWidthProperty);
        set => SetValue(LabelWidthProperty, value);
    }

    /// <summary>
    /// Gets or sets the label horizontal alignment.
    /// </summary>
    public HorizontalAlignment LabelAlignment
    {
        get => GetValue(LabelAlignmentProperty);
        set => SetValue(LabelAlignmentProperty, value);
    }

    /// <summary>
    /// Gets or sets the label margin.
    /// </summary>
    public Thickness LabelMargin
    {
        get => GetValue(LabelMarginProperty);
        set => SetValue(LabelMarginProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets whether the field is required.
    /// </summary>
    public bool IsRequired
    {
        get => GetValue(IsRequiredProperty);
        set => SetValue(IsRequiredProperty, value);
    }

    /// <summary>
    /// Gets or sets the required indicator text.
    /// </summary>
    public string? RequiredIndicator
    {
        get => GetValue(RequiredIndicatorProperty);
        set => SetValue(RequiredIndicatorProperty, value);
    }

    /// <summary>
    /// Gets or sets the help text.
    /// </summary>
    public string? HelpText
    {
        get => GetValue(HelpTextProperty);
        set => SetValue(HelpTextProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the label text should wrap when it exceeds the available space.
    /// </summary>
    public bool TextWrapping
    {
        get => GetValue(TextWrappingProperty);
        set => SetValue(TextWrappingProperty, value);
    }

    /// <inheritdoc/>
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _label = e.NameScope.Get<Label>(PartLabel);
        _grid = e.NameScope.Find<Grid>(PartGrid);

        UpdateGridColumnDefinitions();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        TryResolveDataAnnotations();

        if (Content is StyledElement content)
            AutomationProperties.SetName(content, Label?.ToString());
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        UpdatePseudoClasses();
        HookValidation();
    }

    public Size MeasureLabel()
    {
        if (_label != null && ShowLabel)
        {
            _label.Measure(Size.Infinity);
            return _label.DesiredSize;
        }
        else
        {
            return new(0, 0);
        }
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var labelSize = MeasureLabel();

        EffectiveLabelWidth = !ShowLabel
            ? 0
            : (double)(LabelWidth.IsAbsolute
                ? LabelWidth.Value
                : (LabelWidth.IsAuto || LabelWidth.IsStar) && LabelPosition is Position.Left or Position.Right
                    ? PanelComputedWidth > 0
                        ? PanelComputedWidth
                        : labelSize.Width
                    : labelSize.Width);

        // ⚠️ IMPORTANT
        return base.MeasureOverride(availableSize);
    }

    private void UpdatePseudoClasses()
    {
        PseudoClasses.Set(PseudoClassName.Left, LabelPosition == Position.Left);
        PseudoClasses.Set(PseudoClassName.Top, LabelPosition == Position.Top);
        PseudoClasses.Set(PseudoClassName.Right, LabelPosition == Position.Right);
        PseudoClasses.Set(PseudoClassName.Bottom, LabelPosition == Position.Bottom);

        UpdateGridColumnDefinitions();
    }

    private void UpdateGridColumnDefinitions()
    {
        if (_grid == null) return;

        _grid.ColumnDefinitions.Clear();
        _grid.RowDefinitions.Clear();

        switch (LabelPosition)
        {
            case Position.Left:
                _grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(EffectiveLabelWidth, GridUnitType.Pixel)));
                _grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
                _grid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
                break;

            case Position.Right:
                _grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
                _grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(EffectiveLabelWidth, GridUnitType.Pixel)));
                _grid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
                break;

            case Position.Top:
            case Position.Bottom:
                _grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
                _grid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Auto)));
                _grid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Auto)));
                break;
        }
    }

    private void HookValidation()
    {
        if (Content is not Control field) return;

        field.GetObservable(DataValidationErrors.ErrorsProperty)
            .Subscribe(errors =>
            {
                var hasError = errors?.Any() ?? false;
                PseudoClasses.Set(PseudoClassName.Invalid, hasError);
            });
    }

    private void TryResolveDataAnnotations()
    {
        if (IsRequired) return;
        if (Label is not null) return;
        if (Content is not Control field) return;
        if (field.DataContext is null) return;
        if (field.Name is null) return;

        var key = (field.DataContext.GetType(), field.Name);

        if (!PropertyCache.TryGetValue(key, out var prop))
        {
            prop = key.Item1.GetProperty(key.Name);
            PropertyCache[key] = prop;
        }

        if (prop == null) return;

        var display = prop.GetCustomAttribute<DisplayAttribute>();
        if (display != null && Label == null)
            Label = display.Name;

        var required = prop.GetCustomAttribute<RequiredAttribute>();
        if (required != null)
            IsRequired = true;
    }
}
