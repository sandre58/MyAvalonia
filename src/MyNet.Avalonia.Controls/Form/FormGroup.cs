// -----------------------------------------------------------------------
// <copyright file="FormGroup.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Layout;
using Avalonia.LogicalTree;
using MyNet.Avalonia.Controls.Enums;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class FormGroup : HeaderedItemsControl
{
    static FormGroup()
    {
        ItemsPanelProperty.OverrideDefaultValue<FormGroup>(new FuncTemplate<Panel?>(() => new FormItemsPanel()));

        // Propagate properties to panel when they change
        ColumnsProperty.Changed.AddClassHandler<FormGroup>((x, _) => x.UpdatePanelProperties());
        SpacingProperty.Changed.AddClassHandler<FormGroup>((x, _) => x.UpdatePanelProperties());
        LabelPositionProperty.Changed.AddClassHandler<FormGroup>((x, _) => x.UpdatePanelProperties());
        LabelWidthProperty.Changed.AddClassHandler<FormGroup>((x, _) => x.UpdatePanelProperties());
        LabelMarginProperty.Changed.AddClassHandler<FormGroup>((x, _) => x.UpdatePanelProperties());
        LabelAlignmentProperty.Changed.AddClassHandler<FormGroup>((x, _) => x.UpdatePanelProperties());
        RequiredIndicatorTemplateProperty.Changed.AddClassHandler<FormGroup>((x, _) => x.UpdatePanelProperties());
    }
    #region Styled Properties

    public static readonly StyledProperty<int> ColumnsProperty = AvaloniaProperty.Register<FormGroup, int>(nameof(Columns), 1);

    public static readonly StyledProperty<double> SpacingProperty = AvaloniaProperty.Register<FormGroup, double>(nameof(Spacing), 16d);

    public static readonly StyledProperty<GridLength> LabelWidthProperty = AvaloniaProperty.Register<FormGroup, GridLength>(nameof(LabelWidth), GridLength.Auto);

    public static readonly StyledProperty<Position> LabelPositionProperty = AvaloniaProperty.Register<FormGroup, Position>(nameof(LabelPosition));

    public static readonly StyledProperty<IDataTemplate?> RequiredIndicatorTemplateProperty = AvaloniaProperty.Register<FormGroup, IDataTemplate?>(nameof(RequiredIndicatorTemplate));

    public static readonly StyledProperty<Thickness> LabelMarginProperty = AvaloniaProperty.Register<FormGroup, Thickness>(nameof(LabelMargin), new(0));

    public static readonly StyledProperty<HorizontalAlignment> LabelAlignmentProperty = AvaloniaProperty.Register<FormGroup, HorizontalAlignment>(nameof(LabelAlignment), HorizontalAlignment.Left);

    /// <summary>
    /// Defines the <see cref="IsExpanded"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> IsExpandedProperty = AvaloniaProperty.Register<FormGroup, bool>(nameof(IsExpanded), defaultValue: true);

    /// <summary>
    /// Defines the <see cref="IsExpandable"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> IsExpandableProperty = AvaloniaProperty.Register<FormGroup, bool>(nameof(IsExpandable), defaultValue: false);

    #endregion

    #region CLR Wrappers

    public int Columns
    {
        get => GetValue(ColumnsProperty);
        set => SetValue(ColumnsProperty, value);
    }

    public double Spacing
    {
        get => GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    public GridLength LabelWidth
    {
        get => GetValue(LabelWidthProperty);
        set => SetValue(LabelWidthProperty, value);
    }

    public Position LabelPosition
    {
        get => GetValue(LabelPositionProperty);
        set => SetValue(LabelPositionProperty, value);
    }

    public IDataTemplate? RequiredIndicatorTemplate
    {
        get => GetValue(RequiredIndicatorTemplateProperty);
        set => SetValue(RequiredIndicatorTemplateProperty, value);
    }

    public Thickness LabelMargin
    {
        get => GetValue(LabelMarginProperty);
        set => SetValue(LabelMarginProperty, value);
    }

    public HorizontalAlignment LabelAlignment
    {
        get => GetValue(LabelAlignmentProperty);
        set => SetValue(LabelAlignmentProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets whether the group is expanded.
    /// </summary>
    public bool IsExpanded
    {
        get => GetValue(IsExpandedProperty);
        set => SetValue(IsExpandedProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the group can be collapsed/expanded. Default is false (not expandable).
    /// </summary>
    public bool IsExpandable
    {
        get => GetValue(IsExpandableProperty);
        set => SetValue(IsExpandableProperty, value);
    }

    #endregion

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        UpdatePanelProperties();
    }

    private void UpdatePanelProperties()
    {
        var panel = this.GetLogicalDescendants().OfType<FormItemsPanel>().FirstOrDefault();
        if (panel != null)
        {
            panel.Columns = Columns;
            panel.Spacing = Spacing;
            panel.LabelPosition = LabelPosition;
            panel.LabelWidth = LabelWidth;
        }
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey) => NeedsContainer<FormItemContainer>(item, out recycleKey);

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey) => item is FormItemContainer or FormGroup ? (Control)item : new FormItemContainer { Content = item };

    protected override void PrepareContainerForItemOverride(Control container, object? item, int index)
    {
        base.PrepareContainerForItemOverride(container, item, index);

        switch (container)
        {
            case FormItemContainer fic when item is Control c:
                fic.Label = FormItem.GetLabel(c);
                fic.LabelTemplate = FormItem.GetLabelTemplate(c);
                fic.ShowLabel = !FormItem.GetNoLabel(c) && fic.Label != null;
                fic.LabelPosition = FormItem.GetLabelPosition(c) ?? LabelPosition;
                fic.LabelWidth = FormItem.GetLabelWidth(c) ?? LabelWidth;
                fic.LabelAlignment = FormItem.GetLabelAlignment(c) ?? LabelAlignment;
                fic.LabelMargin = FormItem.GetLabelMargin(c) ?? LabelMargin;
                fic.IsRequired = FormItem.GetIsRequired(c);
                var requiredIndicatorTemplate = FormItem.GetRequiredIndicatorTemplate(c) ?? RequiredIndicatorTemplate;
                if (requiredIndicatorTemplate != null)
                    fic.RequiredIndicatorTemplate = requiredIndicatorTemplate;
                else
                    fic.ClearValue(FormItemContainer.RequiredIndicatorTemplateProperty);
                fic.HelpText = FormItem.GetHelpText(c);
                fic.TextWrapping = FormItem.GetTextWrapping(c);

                // Bind container visibility to content visibility
                fic.Bind(IsVisibleProperty, c.GetObservable(IsVisibleProperty));
                break;

            case FormItemContainer fic:
                // Item is not a Control (e.g., ViewModel): apply Form-level defaults.
                // FormItem properties will be applied later when the DataTemplate materializes.
                fic.LabelPosition = LabelPosition;
                fic.LabelWidth = LabelWidth;
                fic.LabelAlignment = LabelAlignment;
                fic.LabelMargin = LabelMargin;
                if (RequiredIndicatorTemplate != null)
                    fic.RequiredIndicatorTemplate = RequiredIndicatorTemplate;
                else
                    fic.ClearValue(FormItemContainer.RequiredIndicatorTemplateProperty);
                break;
        }
    }
}
