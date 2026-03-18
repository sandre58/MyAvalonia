// -----------------------------------------------------------------------
// <copyright file="TransitionsAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Avalonia;
using Avalonia.Animation;
using MyNet.Avalonia.Theme.Classes;
using MyNet.Avalonia.Theme.Classes.Helpers;

namespace MyNet.Avalonia.Theme.Assists;

public static class TransitionsAssist
{
    static TransitionsAssist() => TransitionsProperty.Changed.Subscribe(TransitionsPropertyChangedCallback);

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

    #region UseTransitions

    /// <summary>
    /// Provides UseTransitions Property for attached TransitionsAssist element.
    /// </summary>
    public static readonly AttachedProperty<bool> UseTransitionsProperty = AvaloniaPropertyHelper.RegisterBoolProperty("UseTransitions", CssClass.UseTransitions);

    /// <summary>
    /// Accessor for Attached  <see cref="UseTransitionsProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="UseTransitionsProperty"/>.</param>
    public static void SetUseTransitions(StyledElement element, bool value) => element.SetValue(UseTransitionsProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="UseTransitionsProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetUseTransitions(StyledElement element) => element.GetValue(UseTransitionsProperty);

    #endregion
}
