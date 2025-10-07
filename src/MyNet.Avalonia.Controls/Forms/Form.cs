// -----------------------------------------------------------------------
// <copyright file="Form.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Layout;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Controls.Forms;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[PseudoClasses(PseudoClassName.FixedWidth)]
public class Form : ItemsControl
{
    public static readonly StyledProperty<GridLength> LabelWidthProperty = AvaloniaProperty.Register<Form, GridLength>(nameof(LabelWidth));

    /// <summary>
    /// Gets or sets the label width.
    /// Behavior:
    /// <para>Fixed Width: all labels are with fixed length. </para>
    /// <para>Star: all labels are aligned by max length. </para>
    /// <para>Auto: labels are not aligned. </para>
    /// </summary>
    public GridLength LabelWidth
    {
        get => GetValue(LabelWidthProperty);
        set => SetValue(LabelWidthProperty, value);
    }

    public static readonly StyledProperty<Position> LabelPositionProperty = AvaloniaProperty.Register<Form, Position>(
        nameof(LabelPosition), defaultValue: Position.Top);

    public Position LabelPosition
    {
        get => GetValue(LabelPositionProperty);
        set => SetValue(LabelPositionProperty, value);
    }

    public static readonly StyledProperty<HorizontalAlignment> LabelAlignmentProperty = AvaloniaProperty.Register<Form, HorizontalAlignment>(
        nameof(LabelAlignment), defaultValue: HorizontalAlignment.Left);

    public HorizontalAlignment LabelAlignment
    {
        get => GetValue(LabelAlignmentProperty);
        set => SetValue(LabelAlignmentProperty, value);
    }

    static Form() => LabelWidthProperty.Changed.AddClassHandler<Form, GridLength>((x, args) => x.LabelWidthChanged(args));

    private void LabelWidthChanged(AvaloniaPropertyChangedEventArgs<GridLength> args)
    {
        var newValue = args.NewValue.Value;
        var isFixed = newValue.IsStar || newValue.IsAbsolute;
        PseudoClasses.Set(PseudoClassName.FixedWidth, isFixed);
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        recycleKey = null;
        return item is not FormItem and not FormGroup;
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey) => item is not Control control
            ? item is IFormGroup ? new FormGroup() : (Control)new FormItem()
            : new FormItem()
            {
                Content = control,
                [!FormItem.LabelProperty] = control[!FormItem.LabelProperty],
                [!FormItem.IsRequiredProperty] = control[!FormItem.IsRequiredProperty],
                [!FormItem.NoLabelProperty] = control[!FormItem.NoLabelProperty],
            };

    protected override void PrepareContainerForItemOverride(Control container, object? item, int index)
    {
        base.PrepareContainerForItemOverride(container, item, index);
        if (container is FormItem formItem && !formItem.IsSet(ContentControl.ContentTemplateProperty))
        {
            formItem.ContentTemplate = ItemTemplate;
        }

        if (container is FormGroup group && !group.IsSet(FormGroup.ItemTemplateProperty))
        {
            group.ItemTemplate = ItemTemplate;
        }
    }
}
