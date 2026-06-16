// -----------------------------------------------------------------------
// <copyright file="Form.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Layout;
using MyNet.Avalonia.Controls.Enums;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class Form : ItemsControl
{
    static Form()
    {
        LabelPositionProperty.Changed.AddClassHandler<Form>((x, _) => x.OnItemLayoutChanged());
        LabelAlignmentProperty.Changed.AddClassHandler<Form>((x, _) => x.OnItemLayoutChanged());
        LabelWidthProperty.Changed.AddClassHandler<Form>((x, _) => x.OnItemLayoutChanged());
        LabelMarginProperty.Changed.AddClassHandler<Form>((x, _) => x.OnItemLayoutChanged());
        RequiredIndicatorTemplateProperty.Changed.AddClassHandler<Form>((x, _) => x.OnItemLayoutChanged());
    }

    #region Styled Properties

    public static readonly StyledProperty<int> ColumnsProperty = AvaloniaProperty.Register<Form, int>(nameof(Columns), 1);

    public static readonly StyledProperty<double> SpacingProperty = AvaloniaProperty.Register<Form, double>(nameof(Spacing), 16d);

    public static readonly StyledProperty<Position> LabelPositionProperty = AvaloniaProperty.Register<Form, Position>(nameof(LabelPosition));

    public static readonly StyledProperty<GridLength> LabelWidthProperty = AvaloniaProperty.Register<Form, GridLength>(nameof(LabelWidth), GridLength.Auto);

    public static readonly StyledProperty<Thickness> LabelMarginProperty = AvaloniaProperty.Register<Form, Thickness>(nameof(LabelMargin), new(0));

    public static readonly StyledProperty<HorizontalAlignment> LabelAlignmentProperty = AvaloniaProperty.Register<Form, HorizontalAlignment>(nameof(LabelAlignment), HorizontalAlignment.Left);

    public static readonly StyledProperty<IDataTemplate?> RequiredIndicatorTemplateProperty = AvaloniaProperty.Register<Form, IDataTemplate?>(nameof(RequiredIndicatorTemplate));

    public static readonly StyledProperty<Thickness> GroupMarginProperty = AvaloniaProperty.Register<Form, Thickness>(nameof(GroupMargin), new(0, 16, 0, 16));

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
                ApplyItemMetadata(fic, c);
                fic.Bind(IsVisibleProperty, c.GetObservable(IsVisibleProperty));
                break;

            case FormItemContainer fic:
                ApplyItemLayoutDefaults(fic);
                break;

            case FormGroup group:
                SyncGroupDefaults(group);
                break;
        }
    }

    private void OnItemLayoutChanged()
    {
        for (var i = 0; i < ItemCount; i++)
        {
            switch (ContainerFromIndex(i))
            {
                case FormItemContainer fic:
                    RefreshItemContainer(fic);
                    break;

                case FormGroup group:
                    SyncGroupDefaults(group);
                    group.RefreshItemContainers();
                    break;
            }
        }
    }

    private void SyncGroupDefaults(FormGroup group)
    {
        if (!group.IsSet(FormGroup.LabelPositionProperty))
            group.LabelPosition = LabelPosition;

        if (!group.IsSet(FormGroup.LabelWidthProperty))
            group.LabelWidth = LabelWidth;

        if (!group.IsSet(FormGroup.LabelAlignmentProperty))
            group.LabelAlignment = LabelAlignment;

        if (!group.IsSet(FormGroup.LabelMarginProperty))
            group.LabelMargin = LabelMargin;

        if (!group.IsSet(FormGroup.RequiredIndicatorTemplateProperty))
            group.RequiredIndicatorTemplate = RequiredIndicatorTemplate;
    }

    private void RefreshItemContainer(FormItemContainer fic)
    {
        if (fic.Content is Control c)
            ApplyItemLayout(fic, c);
        else
            ApplyItemLayoutDefaults(fic);
    }

    private void ApplyItemMetadata(FormItemContainer fic, Control c)
    {
        fic.Label = FormItem.GetLabel(c);
        fic.LabelTemplate = FormItem.GetLabelTemplate(c);
        fic.ShowLabel = !FormItem.GetNoLabel(c) && fic.Label != null;
        ApplyItemLayout(fic, c);
        fic.IsRequired = FormItem.GetIsRequired(c);
        fic.HelpText = FormItem.GetHelpText(c);
        fic.TextWrapping = FormItem.GetTextWrapping(c);
    }

    private void ApplyItemLayout(FormItemContainer fic, Control c)
    {
        fic.LabelPosition = FormItem.GetLabelPosition(c) ?? LabelPosition;
        fic.LabelWidth = FormItem.GetLabelWidth(c) ?? LabelWidth;
        fic.LabelAlignment = FormItem.GetLabelAlignment(c) ?? LabelAlignment;
        fic.LabelMargin = FormItem.GetLabelMargin(c) ?? LabelMargin;
        ApplyRequiredIndicatorTemplate(fic, FormItem.GetRequiredIndicatorTemplate(c) ?? RequiredIndicatorTemplate);
    }

    private void ApplyItemLayoutDefaults(FormItemContainer fic)
    {
        fic.LabelPosition = LabelPosition;
        fic.LabelWidth = LabelWidth;
        fic.LabelAlignment = LabelAlignment;
        fic.LabelMargin = LabelMargin;
        ApplyRequiredIndicatorTemplate(fic, RequiredIndicatorTemplate);
    }

    private static void ApplyRequiredIndicatorTemplate(FormItemContainer fic, IDataTemplate? template)
    {
        if (template != null)
            fic.RequiredIndicatorTemplate = template;
        else
            fic.ClearValue(FormItemContainer.RequiredIndicatorTemplateProperty);
    }
}
