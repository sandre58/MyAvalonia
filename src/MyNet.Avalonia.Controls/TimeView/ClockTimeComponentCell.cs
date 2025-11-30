// -----------------------------------------------------------------------
// <copyright file="ClockTimeComponentCell.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[PseudoClasses(PseudoClassName.Selected, PseudoClassName.Dot)]
public class ClockTimeComponentCell : TemplatedControl
{
    private int _value;

    static ClockTimeComponentCell()
    {
        AffectsArrange<ClockTimeComponentCell>(IsDotProperty);
        AffectsRender<ClockTimeComponentCell>(IsSelectedProperty);

        IsSelectedProperty.Changed.AddClassHandler<ClockTimeComponentCell>(PropertyChangedHandler);
        IsDotProperty.Changed.AddClassHandler<ClockTimeComponentCell>(PropertyChangedHandler);
    }

    #region IsSelected

    public static readonly StyledProperty<bool> IsSelectedProperty = AvaloniaProperty.Register<ClockTimeComponentCell, bool>(nameof(IsSelected));

    public bool IsSelected
    {
        get => GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    #endregion

    #region IsDot

    public static readonly StyledProperty<bool> IsDotProperty = AvaloniaProperty.Register<ClockTimeComponentCell, bool>(nameof(IsDot));

    public bool IsDot
    {
        get => GetValue(IsDotProperty);
        set => SetValue(IsDotProperty, value);
    }

    #endregion

    #region Value

    public static readonly DirectProperty<ClockTimeComponentCell, int> ValueProperty = AvaloniaProperty.RegisterDirect<ClockTimeComponentCell, int>(nameof(Value), o => o.Value, (o, v) => o.Value = v);

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1721:Property names should not match get methods", Justification = "GetValue is in base class")]
    public int Value
    {
        get => _value;
        set => SetAndRaise(ValueProperty, ref _value, value);
    }

    #endregion

    private static void PropertyChangedHandler(ClockTimeComponentCell t, AvaloniaPropertyChangedEventArgs a) => t.UpdatePseudoClasses();

    private void UpdatePseudoClasses()
    {
        PseudoClasses.Set(PseudoClassName.Selected, IsSelected);
        PseudoClasses.Set(PseudoClassName.Dot, IsDot);
    }
}
