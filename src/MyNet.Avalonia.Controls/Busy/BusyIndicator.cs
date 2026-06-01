// -----------------------------------------------------------------------
// <copyright file="BusyIndicator.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130

/// <summary>
/// Full-surface busy overlay. Bind <see cref="IsOpen"/> and <see cref="BusyContent"/> from a view model,
/// or host it behind an <c>IBusyService</c> adapter from MyNet.Avalonia.Extended.
/// </summary>
public class BusyIndicator : TemplatedControl
{
    /// <summary>
    /// Defines the <see cref="IsOpen"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> IsOpenProperty =
        AvaloniaProperty.Register<BusyIndicator, bool>(nameof(IsOpen));

    /// <summary>
    /// Defines the <see cref="BusyContent"/> property.
    /// </summary>
    public static readonly StyledProperty<object?> BusyContentProperty =
        AvaloniaProperty.Register<BusyIndicator, object?>(nameof(BusyContent));

    static BusyIndicator()
    {
        IsHitTestVisibleProperty.OverrideDefaultValue<BusyIndicator>(false);
        IsVisibleProperty.OverrideDefaultValue<BusyIndicator>(false);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the overlay is visible and blocks input.
    /// </summary>
    public bool IsOpen
    {
        get => GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    /// <summary>
    /// Gets or sets the content displayed in the busy card (for example a busy view model).
    /// </summary>
    public object? BusyContent
    {
        get => GetValue(BusyContentProperty);
        set => SetValue(BusyContentProperty, value);
    }

    /// <inheritdoc />
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == IsOpenProperty)
            ApplyOpenState(change.GetNewValue<bool>());
    }

    private void ApplyOpenState(bool isOpen)
    {
        IsVisible = isOpen;
        IsHitTestVisible = isOpen;
        PseudoClasses.Set(":open", isOpen);
    }
}
