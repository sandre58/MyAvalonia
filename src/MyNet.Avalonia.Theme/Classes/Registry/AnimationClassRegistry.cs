// -----------------------------------------------------------------------
// <copyright file="AnimationClassRegistry.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Animation;

namespace MyNet.Avalonia.Theme.Classes.Registry;

/// <summary>
/// Represents a utility class that provides methods for registering and managing animation-related properties for various UI controls.
/// </summary>
public static class AnimationClassRegistry
{
    /// <summary>
    /// Registers default opacity transition animations for all visual elements that use the specified CSS class.
    /// </summary>
    /// <remarks>This method applies a double transition effect to the opacity property of visual elements,
    /// using a duration defined by a theme resource. The current implementation may have performance implications due
    /// to the repeated instantiation of static resource extensions for each element. Consider optimizing this approach
    /// if registering a large number of visuals.</remarks>
    public static void Register() => ClassRegistry.Register<Visual>(CssClass.UseTransitions, visual =>
                                                    {
                                                        var doubleTransition = new DoubleTransition
                                                        {
                                                            Property = Visual.OpacityProperty
                                                        };

                                                        doubleTransition.SetValue(TransitionBase.DurationProperty, ThemeResources.Animation.Opacity.Value);
                                                        return visual.SetProperty(Animatable.TransitionsProperty, new Transitions { doubleTransition });
                                                    });
}
