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
using Avalonia.Controls.Presenters;
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
[TemplatePart(PartLabelContainer, typeof(Panel), IsRequired = true)]
[TemplatePart(PartGrid, typeof(Grid), IsRequired = true)]
[TemplatePart(PartContentPresenter, typeof(ContentPresenter))]
public class FormItemContainer : ContentControl
{
    public const string PartLabelContainer = "PART_LabelContainer";
    public const string PartGrid = "PART_Grid";
    public const string PartContentPresenter = "PART_ContentPresenter";

    private static readonly Dictionary<(Type, string), PropertyInfo?> PropertyCache = [];

    private Grid? _grid;
    private Panel? _labelContainer;
    private ContentPresenter? _contentPresenter;
    private IDisposable? _childSubscription;
    private List<IDisposable>? _propertySubscriptions;

    static FormItemContainer()
    {
        LabelPositionProperty.Changed.AddClassHandler<FormItemContainer>((x, _) => x.UpdatePseudoClasses());
        EffectiveLabelWidthProperty.Changed.AddClassHandler<FormItemContainer>((x, _) => x.UpdateGridColumnDefinitions());
        LabelProperty.Changed.AddClassHandler<FormItemContainer>((x, _) => x.InvalidateLabelWidthMeasure());
        LabelTemplateProperty.Changed.AddClassHandler<FormItemContainer>((x, _) => x.InvalidateLabelWidthMeasure());
        ShowLabelProperty.Changed.AddClassHandler<FormItemContainer>((x, _) => x.InvalidateLabelWidthMeasure());
        LabelWidthProperty.Changed.AddClassHandler<FormItemContainer>((x, _) => x.InvalidateLabelWidthMeasure());
        LabelMarginProperty.Changed.AddClassHandler<FormItemContainer>((x, _) => x.InvalidateLabelWidthMeasure());
        IsRequiredProperty.Changed.AddClassHandler<FormItemContainer>((x, _) => x.InvalidateLabelWidthMeasure());
        RequiredIndicatorTemplateProperty.Changed.AddClassHandler<FormItemContainer>((x, _) => x.InvalidateLabelWidthMeasure());
        TextWrappingProperty.Changed.AddClassHandler<FormItemContainer>((x, _) => x.InvalidateLabelWidthMeasure());
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
    internal static readonly StyledProperty<double> PanelComputedWidthProperty = AvaloniaProperty.Register<FormItemContainer, double>(nameof(PanelComputedWidth));

    /// <summary>
    /// Gets the effective label width that will be used in the template.
    /// Takes into account: LabelWidth setting, panel computed width, and measured width.
    /// </summary>
    public double EffectiveLabelWidth
    {
        get;
        private set => SetAndRaise(EffectiveLabelWidthProperty, ref field, value);
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
    public static readonly StyledProperty<Thickness> LabelMarginProperty = AvaloniaProperty.Register<FormItemContainer, Thickness>(nameof(LabelMargin), defaultValue: new(0, 0, 8, 0));

    /// <summary>
    /// Defines the <see cref="IsRequired"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> IsRequiredProperty = AvaloniaProperty.Register<FormItemContainer, bool>(nameof(IsRequired), defaultValue: false);

    /// <summary>
    /// Defines the <see cref="RequiredIndicatorTemplate"/> property.
    /// </summary>
    public static readonly StyledProperty<IDataTemplate?> RequiredIndicatorTemplateProperty = AvaloniaProperty.Register<FormItemContainer, IDataTemplate?>(nameof(RequiredIndicatorTemplate));

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
    /// Gets or sets the required indicator template.
    /// </summary>
    public IDataTemplate? RequiredIndicatorTemplate
    {
        get => GetValue(RequiredIndicatorTemplateProperty);
        set => SetValue(RequiredIndicatorTemplateProperty, value);
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

        _labelContainer = e.NameScope.Get<Panel>(PartLabelContainer);
        _grid = e.NameScope.Find<Grid>(PartGrid);
        _contentPresenter = e.NameScope.Find<ContentPresenter>(PartContentPresenter);

        UpdateGridColumnDefinitions();

        _childSubscription?.Dispose();
        _propertySubscriptions?.ForEach(s => s.Dispose());
        _propertySubscriptions = null;
        if (_contentPresenter != null)
        {
            _childSubscription = _contentPresenter.GetObservable(ContentPresenter.ChildProperty)
                .Subscribe(OnContentPresenterChildChanged);
        }
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

    public Size MeasureLabelContainer()
    {
        if (_labelContainer != null && ShowLabel)
        {
            _labelContainer.Measure(Size.Infinity);
            return _labelContainer.DesiredSize;
        }

        return new(0, 0);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var labelSize = MeasureLabelContainer();

        EffectiveLabelWidth = !ShowLabel
            ? 0
            : LabelWidth.IsAbsolute
                ? LabelWidth.Value
                : (LabelWidth.IsAuto || LabelWidth.IsStar) && LabelPosition is Position.Left or Position.Right
                    ? PanelComputedWidth > 0
                        ? PanelComputedWidth
                        : labelSize.Width
                    : labelSize.Width;

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

        global::Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            if (_grid == null)
                return;

            // Apply exactly the same structure as before but deferred to avoid
            // mutating Grid definitions during its MeasureOverride.
            _grid.ColumnDefinitions.Clear();
            _grid.RowDefinitions.Clear();

            switch (LabelPosition)
            {
                case Position.Left:
                    _grid.ColumnDefinitions.Add(new(new(EffectiveLabelWidth, GridUnitType.Pixel)));
                    _grid.ColumnDefinitions.Add(new(new(1, GridUnitType.Star)));
                    _grid.RowDefinitions.Add(new(new(1, GridUnitType.Star)));
                    break;

                case Position.Right:
                    _grid.ColumnDefinitions.Add(new(new(1, GridUnitType.Star)));
                    _grid.ColumnDefinitions.Add(new(new(EffectiveLabelWidth, GridUnitType.Pixel)));
                    _grid.RowDefinitions.Add(new(new(1, GridUnitType.Star)));
                    break;

                case Position.Top:
                case Position.Bottom:
                    _grid.ColumnDefinitions.Add(new(new(1, GridUnitType.Star)));
                    _grid.RowDefinitions.Add(new(new(1, GridUnitType.Auto)));
                    _grid.RowDefinitions.Add(new(new(1, GridUnitType.Auto)));
                    break;
            }

            // Trigger a new layout pass with the updated definitions.
            _grid.InvalidateMeasure();
        },
        global::Avalonia.Threading.DispatcherPriority.Background);
    }

    private void InvalidateLabelWidthMeasure()
    {
        InvalidateMeasure();

        // The shared label width is computed by FormItemsPanel, so bubble a measure invalidation.
        if (Parent is Layoutable parentLayoutable)
            parentLayoutable.InvalidateMeasure();
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

    private void OnContentPresenterChildChanged(Control? child)
    {
        // Only apply when child comes from a DataTemplate (not when Content is already a Control)
        if (child == null || ReferenceEquals(child, Content)) return;

        _propertySubscriptions?.ForEach(s => s.Dispose());
        _propertySubscriptions = [];

        ApplyFormItemProperties(child);
    }

    private void ApplyFormItemProperties(Control c)
    {
        Label = FormItem.GetLabel(c);
        LabelTemplate = FormItem.GetLabelTemplate(c);
        ShowLabel = !FormItem.GetNoLabel(c) && Label != null;

        _propertySubscriptions!.Add(c.GetObservable(FormItem.LabelPositionProperty)
            .Subscribe(pos => { if (pos.HasValue) LabelPosition = pos.Value; }));

        _propertySubscriptions!.Add(c.GetObservable(FormItem.LabelWidthProperty)
            .Subscribe(w => { if (w.HasValue) LabelWidth = w.Value; }));

        _propertySubscriptions!.Add(c.GetObservable(FormItem.LabelAlignmentProperty)
            .Subscribe(a => { if (a.HasValue) LabelAlignment = a.Value; }));

        _propertySubscriptions!.Add(c.GetObservable(FormItem.LabelMarginProperty)
            .Subscribe(m => { if (m.HasValue) LabelMargin = m.Value; }));

        IsRequired = FormItem.GetIsRequired(c);

        var requiredIndicatorTemplate = FormItem.GetRequiredIndicatorTemplate(c);
        if (requiredIndicatorTemplate != null)
            RequiredIndicatorTemplate = requiredIndicatorTemplate;
        else
            ClearValue(RequiredIndicatorTemplateProperty);

        HelpText = FormItem.GetHelpText(c);
        TextWrapping = FormItem.GetTextWrapping(c);

        // Bind container visibility to content visibility
        Bind(IsVisibleProperty, c.GetObservable(IsVisibleProperty));
    }
}
