// -----------------------------------------------------------------------
// <copyright file="MultiComboBoxItem.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Automation.Peers;
using Avalonia.Controls;
using Avalonia.Controls.Mixins;
using Avalonia.Input;
using Avalonia.LogicalTree;
using MyNet.Avalonia.Extensions;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class MultiComboBoxItem : ContentControl
{
    private static readonly Point InvalidPoint = new(double.NaN, double.NaN);
    private MultiComboBox? _parent;
    private Point _pointerDownPoint = InvalidPoint;
    private bool _updateInternal;

    public static readonly StyledProperty<bool> IsSelectedProperty = AvaloniaProperty.Register<MultiComboBoxItem, bool>(
        nameof(IsSelected));

    public bool IsSelected
    {
        get => GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    static MultiComboBoxItem()
    {
        IsSelectedProperty.AffectsPseudoClass<MultiComboBoxItem>(PseudoClassName.Selected);
        PressedMixin.Attach<MultiComboBoxItem>();
        FocusableProperty.OverrideDefaultValue<MultiComboBoxItem>(true);
        _ = IsSelectedProperty.Changed.AddClassHandler<MultiComboBoxItem, bool>((item, args) => item.OnSelectionChanged(args));
    }

    private void OnSelectionChanged(AvaloniaPropertyChangedEventArgs<bool> args)
    {
        if (_updateInternal) return;
        var parent = this.FindLogicalAncestorOfType<MultiComboBox>();
        if (args.NewValue.Value)
        {
            _ = parent?.SelectedItems?.Add(DataContext);
        }
        else
        {
            parent?.SelectedItems?.Remove(DataContext);
        }
    }

    protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnAttachedToLogicalTree(e);
        _parent = this.FindLogicalAncestorOfType<MultiComboBox>();
        if (IsSelected)
            _ = _parent?.SelectedItems?.Add(DataContext);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        _pointerDownPoint = e.GetPosition(this);
        if (e.Handled)
        {
            return;
        }

        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            var p = e.GetCurrentPoint(this);
            if (p.Properties.PointerUpdateKind is PointerUpdateKind.LeftButtonPressed
                or PointerUpdateKind.RightButtonPressed)
            {
                if (p.Pointer.Type == PointerType.Mouse)
                {
                    IsSelected = !IsSelected;
                    e.Handled = true;
                }
                else
                {
                    _pointerDownPoint = p.Position;
                }
            }
        }
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);
        if (!e.Handled && !double.IsNaN(_pointerDownPoint.X) &&
            e.InitialPressMouseButton is MouseButton.Left or MouseButton.Right)
        {
            var point = e.GetCurrentPoint(this);
            if (new Rect(Bounds.Size).ContainsExclusive(point.Position) && e.Pointer.Type == PointerType.Touch)
            {
                IsSelected = !IsSelected;
                e.Handled = true;
            }
        }
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        UpdateSelection();
    }

    internal void UpdateSelection()
    {
        _updateInternal = true;
        if (_parent?.ItemsPanelRoot is VirtualizingPanel)
        {
            IsSelected = _parent?.SelectedItems?.Contains(DataContext) ?? false;
        }

        _updateInternal = false;
    }

    protected override AutomationPeer OnCreateAutomationPeer() => new ListItemAutomationPeer(this);
}
