// -----------------------------------------------------------------------
// <copyright file="StateClassRegistry.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.Avalonia.Theme.Theming;

namespace MyNet.Avalonia.Theme.Classes.Registry;

/// <summary>
/// Provides utility methods for managing the state of UI elements, including the ability to register disablable
/// elements that visually indicate their enabled or disabled state.
/// </summary>
/// <remarks>This class includes methods that facilitate the registration of UI elements that can be disabled,
/// ensuring that their visual representation updates accordingly based on their enabled state. It is particularly
/// useful for maintaining consistent UI behavior in applications that utilize input elements.</remarks>
public static class StateClassRegistry
{
    /// <summary>
    /// Registers an input element to support a disablable visual state, adjusting its opacity based on the enabled
    /// state.
    /// </summary>
    /// <remarks>This method subscribes to changes in the IsEnabled property of the input element and updates
    /// its opacity accordingly. The opacity is set to a predefined disabled value when the element is not enabled, and
    /// cleared when it is enabled.</remarks>
    public static void Register() => ClassRegistry.Register<InputElement>(CssClass.IsDisablable, x =>
                                                    {
                                                        // Create resource observable once
                                                        var opacityObservable = x.GetResourceObservable(ThemeResourceKeyFactory.Opacity(nameof(Opacity.Disabled)));

                                                        IDisposable? opacityBinding = null;

                                                        return x.GetObservable(InputElement.IsEffectivelyEnabledProperty)
                                                            .Subscribe(enabled =>
                                                            {
                                                                if (!enabled)
                                                                {
                                                                    opacityBinding ??=
                                                                        x.Bind(Visual.OpacityProperty, opacityObservable);
                                                                }
                                                                else
                                                                {
                                                                    opacityBinding?.Dispose();
                                                                    opacityBinding = null;
                                                                    x.ClearValue(Visual.OpacityProperty);
                                                                }
                                                            });
                                                    });
}
