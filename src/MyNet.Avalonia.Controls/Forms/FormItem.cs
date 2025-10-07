// -----------------------------------------------------------------------
// <copyright file="FormItem.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Reactive;
using Avalonia.VisualTree;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Extensions;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[PseudoClasses(PseudoClassName.Horizontal, PseudoClassName.NoLabel)]
[TemplatePart(PartLabel, typeof(Label))]
public class FormItem : ContentControl
{
    public const string PartLabel = "PART_Label";

    #region Attached Properties

    public static readonly AttachedProperty<object?> LabelProperty = AvaloniaProperty.RegisterAttached<FormItem, Control, object?>("Label");

    public static void SetLabel(Control obj, object? value) => obj.SetValue(LabelProperty, value);

    public static object? GetLabel(Control obj) => obj.GetValue(LabelProperty);

    public static readonly AttachedProperty<bool> IsRequiredProperty = AvaloniaProperty.RegisterAttached<FormItem, Control, bool>("IsRequired");

    public static void SetIsRequired(Control obj, bool value) => obj.SetValue(IsRequiredProperty, value);

    public static bool GetIsRequired(Control obj) => obj.GetValue(IsRequiredProperty);

    public static readonly AttachedProperty<bool> NoLabelProperty = AvaloniaProperty.RegisterAttached<FormItem, Control, bool>("NoLabel");

    public static void SetNoLabel(Control obj, bool value) => obj.SetValue(NoLabelProperty, value);

    public static bool GetNoLabel(Control obj) => obj.GetValue(NoLabelProperty);

    #endregion

    private readonly List<IDisposable> _formSubscriptions = [];
    private Label? _label;

    public static readonly StyledProperty<double> LabelWidthProperty = AvaloniaProperty.Register<FormItem, double>(
        nameof(LabelWidth));

    public double LabelWidth
    {
        get => GetValue(LabelWidthProperty);
        set => SetValue(LabelWidthProperty, value);
    }

    public static readonly StyledProperty<HorizontalAlignment> LabelAlignmentProperty = AvaloniaProperty.Register<FormItem, HorizontalAlignment>(
        nameof(LabelAlignment));

    public HorizontalAlignment LabelAlignment
    {
        get => GetValue(LabelAlignmentProperty);
        set => SetValue(LabelAlignmentProperty, value);
    }

    static FormItem()
    {
        NoLabelProperty.AffectsPseudoClass<FormItem>(PseudoClassName.NoLabel);
        _ = LabelProperty.Changed.AddClassHandler<FormItem>((o, _) => o.SetLabelTarget());
        _ = ContentProperty.Changed.AddClassHandler<FormItem>((o, _) => o.SetLabelTarget());
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        var form = this.GetVisualAncestors().OfType<Form>().FirstOrDefault();
        if (form is not null)
        {
            _formSubscriptions.Clear();
            var labelSubscription = form
                .GetObservable(Form.LabelWidthProperty)
                .Subscribe(new AnonymousObserver<GridLength>(length => LabelWidth = length.IsAbsolute ? length.Value : double.NaN));
            var positionSubscription = form
                .GetObservable(Form.LabelPositionProperty)
                .Subscribe(new AnonymousObserver<Position>(position => PseudoClasses.Set(PseudoClassName.Horizontal, position == Position.Left)));
            var alignmentSubscription = form
                .GetObservable(Form.LabelAlignmentProperty)
                .Subscribe(new AnonymousObserver<HorizontalAlignment>(alignment => LabelAlignment = alignment));
            _formSubscriptions.Add(labelSubscription);
            _formSubscriptions.Add(positionSubscription);
            _formSubscriptions.Add(alignmentSubscription);
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _label = e.NameScope.Find<Label>(PartLabel);
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        SetLabelTarget();
    }

    private void SetLabelTarget()
    {
        if (_label is null) return;

        // Set it directly if content is a control, this is faster than looking up logical tree.
        if (Content is InputElement input)
        {
            _label.Target = input;
        }
        else
        {
            var logical = LogicalChildren.OfType<InputElement>().FirstOrDefault(a => a.Focusable);
            if (logical is not null)
                _label.Target = logical;
        }
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        foreach (var subscription in _formSubscriptions)
        {
            subscription.Dispose();
        }
    }
}
