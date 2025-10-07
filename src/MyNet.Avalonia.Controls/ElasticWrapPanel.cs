// -----------------------------------------------------------------------
// <copyright file="ElasticWrapPanel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using MyNet.Utilities;
using static System.Math;

namespace MyNet.Avalonia.Controls;

public sealed class ElasticWrapPanel : WrapPanel, INavigableContainer
{
    private int _maxItemsByLine;

    static ElasticWrapPanel()
    {
        _ = IsFillHorizontalProperty.Changed.AddClassHandler<Control>(OnIsFillPropertyChanged);
        _ = IsFillVerticalProperty.Changed.AddClassHandler<Control>(OnIsFillPropertyChanged);

        AffectsMeasure<ElasticWrapPanel>(IsFillHorizontalProperty, IsFillVerticalProperty);
    }

    #region AttachedProperty

    public static void SetFixToRb(Control element, bool value)
    {
        _ = element ?? throw new ArgumentNullException(nameof(element));
        _ = element.SetValue(FixToRbProperty, value);
    }

    public static bool GetIsFixToRb(Control element)
    {
        _ = element ?? throw new ArgumentNullException(nameof(element));
        return element.GetValue(FixToRbProperty);
    }

    /// <summary>
    /// Fixed to [Right (Horizontal Mode) | Bottom (Vertical Mode)]
    /// which will cause line breaks.
    /// </summary>
    public static readonly AttachedProperty<bool> FixToRbProperty =
        AvaloniaProperty.RegisterAttached<ElasticWrapPanel, Control, bool>("FixToRB");

    #endregion

    #region StyledProperty

    public bool IsFillHorizontal
    {
        get => GetValue(IsFillHorizontalProperty);
        set => SetValue(IsFillHorizontalProperty, value);
    }

    public static readonly StyledProperty<bool> IsFillHorizontalProperty =
        AvaloniaProperty.Register<ElasticWrapPanel, bool>(nameof(IsFillHorizontal));

    public bool IsFillVertical
    {
        get => GetValue(IsFillVerticalProperty);
        set => SetValue(IsFillVerticalProperty, value);
    }

    public static readonly StyledProperty<bool> IsFillVerticalProperty =
        AvaloniaProperty.Register<ElasticWrapPanel, bool>(nameof(IsFillVertical));

    private static void OnIsFillPropertyChanged(AvaloniaObject d, AvaloniaPropertyChangedEventArgs e) => (d as ElasticWrapPanel)?.InvalidateMeasure();

    #endregion

    protected override Size MeasureOverride(Size constraint)
    {
        var itemWidth = ItemWidth;
        var itemHeight = ItemHeight;
        var orientation = Orientation;

        // Determine the space required for items in the same row/column based on horizontal/vertical arrangement
        var curLineSize = new UvSize(orientation);

        // Calculate the total space requirement for this ElasticWrapPanel
        var panelSize = new UvSize(orientation);

        // Measure UVSize with the given space constraint, used for measuring elements when ItemWidth and ItemHeight are not set
        var uvConstraint = new UvSize(orientation, constraint.Width, constraint.Height);
        var itemWidthSet = !double.IsNaN(itemWidth);
        var itemHeightSet = !double.IsNaN(itemHeight);

        var childConstraint = new Size(
            itemWidthSet ? itemWidth : constraint.Width,
            itemHeightSet ? itemHeight : constraint.Height);

        // Measurement space for elements with FixToRB=True
        var childFixConstraint = orientation switch
        {
            Orientation.Horizontal when itemHeightSet => new Size(constraint.Width, itemHeight),
            Orientation.Vertical when itemWidthSet => new Size(itemWidth, constraint.Height),
            _ => new Size(constraint.Width, constraint.Height)
        };

        // This is the size for non-space measurement
        var itemSetSize = new UvSize(orientation,
            itemWidthSet ? itemWidth : 0,
            itemHeightSet ? itemHeight : 0);

        foreach (var child in Children)
        {
            UvSize sz;
            if (GetIsFixToRb(child))
            {
                // Measure the element when it needs to be fixed to the right/bottom
                child.Measure(childFixConstraint);
                sz = new UvSize(orientation, child.DesiredSize.Width, child.DesiredSize.Height);

                // Ensure the width/height is within the constraint limits
                if (sz.U > 0 && itemSetSize.U > 0)
                {
                    sz.U = sz.U < itemSetSize.U ? itemSetSize.U : Min(sz.U, uvConstraint.U);
                }

                if (sz.V > 0 && itemSetSize.V > 0 && sz.V < itemSetSize.V)
                {
                    sz.V = itemSetSize.V;
                }

                if ((curLineSize.U + sz.U).GreaterThan(uvConstraint.U))
                {
                    panelSize.U = Max(curLineSize.U, panelSize.U);
                    panelSize.V += curLineSize.V;
                }
                else
                {
                    curLineSize.U += sz.U;
                    curLineSize.V = Max(sz.V, curLineSize.V);
                    panelSize.U = Max(curLineSize.U, panelSize.U);
                    panelSize.V += curLineSize.V;
                }

                curLineSize = new UvSize(orientation);
            }
            else
            {
                // Flow passes its own constraint to children
                child.Measure(childConstraint);

                // This is the size of the child in UV space
                sz = new UvSize(orientation,
                    itemWidthSet ? itemWidth : child.DesiredSize.Width,
                    itemHeightSet ? itemHeight : child.DesiredSize.Height);

                // Need to switch to another line
                if ((curLineSize.U + sz.U).GreaterThan(uvConstraint.U))
                {
                    panelSize.U = Max(curLineSize.U, panelSize.U);
                    panelSize.V += curLineSize.V;
                    curLineSize = sz;

                    // The element is wider than the constraint - give it a separate line
                    if (!sz.U.GreaterThan(uvConstraint.U))
                        continue;
                    panelSize.U = Max(sz.U, panelSize.U);
                    panelSize.V += sz.V;
                    curLineSize = new UvSize(orientation);
                }

                // Continue to accumulate a line
                else
                {
                    curLineSize.U += sz.U;
                    curLineSize.V = Max(sz.V, curLineSize.V);
                }
            }
        }

        // The last line size, if any should be added
        panelSize.U = Max(curLineSize.U, panelSize.U);
        panelSize.V += curLineSize.V;

        // Go from UV space to W/H space
        return new Size(panelSize.Width, panelSize.Height);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        _maxItemsByLine = 0;

        var itemWidthSet = !double.IsNaN(ItemWidth);
        var itemHeightSet = !double.IsNaN(ItemHeight);

        // This is the size for non-space measurement
        var itemSetSize = new UvSize(Orientation,
            itemWidthSet ? ItemWidth : 0,
            itemHeightSet ? ItemHeight : 0);

        // Measure UVSize with the given space constraint, used for measuring elements when ItemWidth and ItemHeight are not set
        var uvFinalSize = new UvSize(Orientation, finalSize.Width, finalSize.Height);

        // Collection of elements in the same direction (row/column)
        var lineUvCollection = new List<UvCollection>();

        // Current collection of elements in a row/column
        var curLineUIs = new UvCollection(Orientation);

        // Iterate over the child elements
        foreach (var child in Children)
        {
            UvSize sz;
            if (GetIsFixToRb(child))
            {
                // Measure the element when it needs to be fixed to the right/bottom
                sz = new UvSize(Orientation, child.DesiredSize.Width, child.DesiredSize.Height);
                double lengthCount = 1;
                if (sz.U > 0 && itemSetSize.U > 0)
                {
                    if (sz.U < itemSetSize.U)
                    {
                        sz.U = itemSetSize.U;
                    }
                    else
                    {
                        lengthCount = Ceiling(sz.U / itemSetSize.U);
                        sz.U = Min(sz.U, uvFinalSize.U);
                    }
                }

                if (sz.V > 0 && itemSetSize.V > 0 && sz.V < itemSetSize.V)
                {
                    sz.V = itemSetSize.V;
                }

                if ((curLineUIs.TotalU + sz.U).GreaterThan(uvFinalSize.U))
                {
                    if (curLineUIs.Count > 0)
                    {
                        lineUvCollection.Add(curLineUIs);
                    }

                    curLineUIs = new UvCollection(Orientation);
                    curLineUIs.Add(child, sz, Convert.ToInt32(lengthCount));
                }
                else
                {
                    curLineUIs.Add(child, sz, Convert.ToInt32(lengthCount));
                }

                lineUvCollection.Add(curLineUIs);
                curLineUIs = new UvCollection(Orientation);
            }
            else
            {
                sz = new UvSize(Orientation,
                    itemWidthSet ? ItemWidth : child.DesiredSize.Width,
                    itemHeightSet ? ItemHeight : child.DesiredSize.Height);

                // Need to switch to another line
                if ((curLineUIs.TotalU + sz.U).GreaterThan(uvFinalSize.U))
                {
                    if (curLineUIs.Count > 0)
                    {
                        lineUvCollection.Add(curLineUIs);
                    }

                    curLineUIs = new UvCollection(Orientation);
                    curLineUIs.Add(child, sz);
                    if (!sz.U.GreaterThan(uvFinalSize.U))
                        continue;
                    lineUvCollection.Add(curLineUIs);
                    curLineUIs = new UvCollection(Orientation);
                }
                else
                {
                    curLineUIs.Add(child, sz);
                }
            }
        }

        if (curLineUIs.Count > 0 && !lineUvCollection.Contains(curLineUIs))
        {
            lineUvCollection.Add(curLineUIs);
        }

        bool isFillU;
        bool isFillV;
        switch (Orientation)
        {
            case Orientation.Horizontal:
                isFillU = IsFillHorizontal;
                isFillV = IsFillVertical;
                break;

            case Orientation.Vertical:
                isFillU = IsFillVertical;
                isFillV = IsFillHorizontal;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(finalSize));
        }

        if (lineUvCollection.Count > 0)
        {
            _maxItemsByLine = lineUvCollection[0].Count;

            double accumulatedV = 0;
            double adaptULength = 0;
            var isAdaptV = false;
            double adaptVLength = 0;
            if (isFillU && itemSetSize.U > 0)
            {
                static int valueSelector(KeyValuePair<Control, UvLengthSize> b) => b.Value.ULengthCount;
                var maxElementCount = lineUvCollection
                    .Max(uiSet => uiSet.UiCollection.Sum(valueSelector));
                adaptULength = (uvFinalSize.U - (maxElementCount * itemSetSize.U)) / maxElementCount;
                adaptULength = Max(adaptULength, 0);
            }

            if (isFillV && itemSetSize.V > 0)
            {
                isAdaptV = true;
                adaptVLength = uvFinalSize.V / lineUvCollection.Count;
            }

            var isHorizontal = Orientation == Orientation.Horizontal;
            foreach (var uvCollection in lineUvCollection)
            {
                double u = 0;
                var lineUiEles = uvCollection.UiCollection.Keys.ToList();
                var linevV = isAdaptV ? adaptVLength : uvCollection.LineV;
                foreach (var child in lineUiEles)
                {
                    var childSize = uvCollection.UiCollection[child];

                    var layoutSlotU = childSize.UvSize.U + (childSize.ULengthCount * adaptULength);
                    var layoutSlotV = isAdaptV ? linevV : childSize.UvSize.V;
                    if (!GetIsFixToRb(child))
                    {
                        child.Arrange(new Rect(
                            isHorizontal ? u : accumulatedV,
                            isHorizontal ? accumulatedV : u,
                            isHorizontal ? layoutSlotU : layoutSlotV,
                            isHorizontal ? layoutSlotV : layoutSlotU));
                    }
                    else
                    {
                        if (itemSetSize.U > 0)
                        {
                            layoutSlotU = (childSize.ULengthCount * itemSetSize.U) +
                                          (childSize.ULengthCount * adaptULength);
                            var leaveULength = uvFinalSize.U - u;
                            layoutSlotU = Min(leaveULength, layoutSlotU);
                        }

                        child.Arrange(new Rect(
                            isHorizontal ? Max(0, uvFinalSize.U - layoutSlotU) : accumulatedV,
                            isHorizontal ? accumulatedV : Max(0, uvFinalSize.U - layoutSlotU),
                            isHorizontal ? layoutSlotU : layoutSlotV,
                            isHorizontal ? layoutSlotV : layoutSlotU));
                    }

                    u += layoutSlotU;
                }

                accumulatedV += linevV;
                lineUiEles.Clear();
            }
        }

        lineUvCollection.ForEach(col => col.Dispose());
        lineUvCollection.Clear();
        return finalSize;
    }

    /// <summary>
    /// Gets the next control in the specified direction.
    /// </summary>
    /// <param name="direction">The movement direction.</param>
    /// <param name="from">The control from which movement begins.</param>
    /// <param name="wrap">Whether to wrap around when the first or last item is reached.</param>
    /// <returns>The control.</returns>
    IInputElement? INavigableContainer.GetControl(NavigationDirection direction, IInputElement? from, bool wrap)
    {
        var orientation = Orientation;
        var horiz = orientation == Orientation.Horizontal;
        var index = from is not null ? Children.IndexOf((Control)from) : -1;

        switch (direction)
        {
            case NavigationDirection.First:
                index = 0;
                break;
            case NavigationDirection.Last:
                index = Children.Count - 1;
                break;
            case NavigationDirection.Next:
                ++index;
                break;
            case NavigationDirection.Previous:
                --index;
                break;
            case NavigationDirection.Left:
                index = horiz ? index - 1 : index - _maxItemsByLine;
                break;
            case NavigationDirection.Right:
                index = horiz ? index + 1 : index + _maxItemsByLine;
                break;
            case NavigationDirection.Up:
                index = horiz ? index - _maxItemsByLine : index - 1;
                break;
            case NavigationDirection.Down:
                index = horiz ? index + _maxItemsByLine : index + 1;
                break;
            case NavigationDirection.PageUp:
            case NavigationDirection.PageDown:
            default:
                break;
        }

        return index >= 0 && index < Children.Count ? Children[index] : (IInputElement?)null;
    }

    #region Protected Methods

    private struct UvSize
    {
        internal UvSize(Orientation orientation, double width, double height)
        {
            U = V = 0d;
            _orientation = orientation;
            Width = width;
            Height = height;
        }

        internal UvSize(Orientation orientation)
        {
            U = V = 0d;
            _orientation = orientation;
        }

        private readonly Orientation _orientation;

        internal double U { get; set; }

        internal double V { get; set; }

        internal double Width
        {
            readonly get => _orientation == Orientation.Horizontal ? U : V;
            private init
            {
                if (_orientation == Orientation.Horizontal) U = value;
                else V = value;
            }
        }

        internal double Height
        {
            readonly get => _orientation == Orientation.Horizontal ? V : U;
            private init
            {
                if (_orientation == Orientation.Horizontal) V = value;
                else U = value;
            }
        }
    }

    private sealed class UvLengthSize(UvSize uvSize, int uLengthCount)
    {
        public UvSize UvSize { get; } = uvSize;

        public int ULengthCount { get; } = uLengthCount;
    }

    /// <summary>
    /// Elements used to store the same row/column.
    /// </summary>
    private sealed class UvCollection(Orientation orientation) : IDisposable
    {
        public Dictionary<Control, UvLengthSize> UiCollection { get; } = [];

        private UvSize _lineDesireUvSize = new(orientation);

        public double TotalU => _lineDesireUvSize.U;

        public double LineV => _lineDesireUvSize.V;

        public void Add(Control element, UvSize childSize, int itemULength = 1)
        {
            if (UiCollection.ContainsKey(element))
                throw new InvalidOperationException("The element already exists and cannot be added repeatedly.");

            UiCollection[element] = new UvLengthSize(childSize, itemULength);
            _lineDesireUvSize.U += childSize.U;
            _lineDesireUvSize.V = Max(_lineDesireUvSize.V, childSize.V);
        }

        public int Count => UiCollection.Count;

        public void Dispose() => UiCollection.Clear();
    }

    #endregion
}
