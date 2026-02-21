// -----------------------------------------------------------------------
// <copyright file="Form.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using MyNet.Avalonia.Controls.Enums;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class Form : ItemsControl
{
    #region Styled Properties

    public static readonly StyledProperty<int> ColumnsProperty = AvaloniaProperty.Register<Form, int>(nameof(Columns), 1);

    public static readonly StyledProperty<double> SpacingProperty = AvaloniaProperty.Register<Form, double>(nameof(Spacing), 16d);

    public static readonly StyledProperty<Position> LabelPositionProperty = AvaloniaProperty.Register<Form, Position>(nameof(LabelPosition));

    public static readonly StyledProperty<GridLength> LabelWidthProperty = AvaloniaProperty.Register<Form, GridLength>(nameof(LabelWidth), GridLength.Auto);

    public static readonly StyledProperty<Thickness> LabelMarginProperty = AvaloniaProperty.Register<Form, Thickness>(nameof(LabelMargin), new Thickness(0));

    public static readonly StyledProperty<HorizontalAlignment> LabelAlignmentProperty = AvaloniaProperty.Register<Form, HorizontalAlignment>(nameof(LabelAlignment), HorizontalAlignment.Left);

    public static readonly StyledProperty<string?> RequiredIndicatorProperty = AvaloniaProperty.Register<Form, string?>(nameof(RequiredIndicator), "*");

    public static readonly StyledProperty<Thickness> GroupMarginProperty = AvaloniaProperty.Register<Form, Thickness>(nameof(GroupMargin), new Thickness(0, 16, 0, 16));

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

    public Position LabelPosition
    {
        get => GetValue(LabelPositionProperty);
        set => SetValue(LabelPositionProperty, value);
    }

    public GridLength LabelWidth
    {
        get => GetValue(LabelWidthProperty);
        set => SetValue(LabelWidthProperty, value);
    }

    public string? RequiredIndicator
    {
        get => GetValue(RequiredIndicatorProperty);
        set => SetValue(RequiredIndicatorProperty, value);
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

    public Thickness GroupMargin
    {
        get => GetValue(GroupMarginProperty);
        set => SetValue(GroupMarginProperty, value);
    }

    #endregion

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
                fic.RequiredIndicator = FormItem.GetRequiredIndicator(c) ?? RequiredIndicator;
                fic.HelpText = FormItem.GetHelpText(c);
                fic.TextWrapping = FormItem.GetTextWrapping(c);

                // Bind container visibility to content visibility
                fic.Bind(IsVisibleProperty, c.GetObservable(IsVisibleProperty));
                break;

            case FormGroup group:
                {
                    // Propagate Form properties to FormGroup if not explicitly set
                    if (!group.IsSet(FormGroup.LabelPositionProperty))
                        group.LabelPosition = LabelPosition;

                    if (!group.IsSet(FormGroup.LabelWidthProperty))
                        group.LabelWidth = LabelWidth;

                    if (!group.IsSet(FormGroup.LabelAlignmentProperty))
                        group.LabelAlignment = LabelAlignment;

                    if (!group.IsSet(FormGroup.LabelMarginProperty))
                        group.LabelMargin = LabelMargin;

                    if (!group.IsSet(FormGroup.RequiredIndicatorProperty))
                        group.RequiredIndicator = RequiredIndicator;
                    break;
                }
        }
    }
}
