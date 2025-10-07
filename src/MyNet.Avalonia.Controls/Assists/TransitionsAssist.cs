// -----------------------------------------------------------------------
// <copyright file="TransitionsAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Data;

namespace MyNet.Avalonia.Controls.Assists;

public static class TransitionsAssist
{
    static TransitionsAssist()
    {
        _ = TransitionsProperty.Changed.Subscribe(TransitionsPropertyChangedCallback);
        _ = DisableTransitionsProperty.Changed.Subscribe(args =>
        {
            if (args.Sender is not StyledElement styledElement) return;
            styledElement.Classes.Set("no-transitions", args.NewValue.Value);
        });
    }

    #region Transitions

    /// <summary>
    /// Provides Transitions Property for attached TransitionsAssist element.
    /// </summary>
    public static readonly AttachedProperty<Transitions> TransitionsProperty = AvaloniaProperty.RegisterAttached<StyledElement, Transitions>("Transitions", typeof(TransitionsAssist), []);

    /// <summary>
    /// Accessor for Attached  <see cref="TransitionsProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="TransitionsProperty"/>.</param>
    public static void SetTransitions(StyledElement element, Transitions value) => element.SetValue(TransitionsProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="TransitionsProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static Transitions GetTransitions(StyledElement element) => element.GetValue(TransitionsProperty);

    private static void TransitionsPropertyChangedCallback(AvaloniaPropertyChangedEventArgs obj)
    {
        if (obj is not { Sender: Animatable element, NewValue: Transitions transitions })
            return;
        if (element.Transitions is not null)
            element.Transitions.AddRange(transitions.Where(x => !element.Transitions.Contains(x)));
        else
            element.Transitions = transitions;
    }

    #endregion

    #region DisableTransitions

    /// <summary>
    ///     Allows transitions to be disabled where supported.  Note this is an inheritable property.
    /// </summary>
    public static readonly AvaloniaProperty<bool> DisableTransitionsProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, bool>("DisableTransitions", typeof(TransitionsAssist), false, true, BindingMode.TwoWay);

    /// <summary>
    ///     Allows transitions to be disabled where supported.  Note this is an inheritable property.
    /// </summary>
    public static void SetDisableTransitions(AvaloniaObject element, bool value) => element.SetValue(DisableTransitionsProperty, value);

    /// <summary>
    ///     Allows transitions to be disabled where supported.  Note this is an inheritable property.
    /// </summary>
    public static bool GetDisableTransitions(AvaloniaObject element) => element.GetValue<bool>(DisableTransitionsProperty);

    #endregion DisableTransitions
}
