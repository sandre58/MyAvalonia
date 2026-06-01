// -----------------------------------------------------------------------
// <copyright file="StyleRenderer.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using MyNet.Avalonia.Controls;

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Rendering;

/// <summary>
/// Provides functionality to apply styling configurations, including class and property adjustments, to UI controls.
/// </summary>
/// <remarks>Use this class to modify the appearance and behavior of controls based on specified configuration
/// settings. The target control should be properly initialized and part of the UI hierarchy before applying styles.
/// This class ensures that only the current configuration's classes and properties are active on the control,
/// maintaining a clean and consistent styling state.</remarks>
internal sealed class StyleRenderer : IStyleRenderer, IDisposable
{
    private readonly HashSet<string> _appliedClasses = [];
    private readonly HashSet<AvaloniaProperty> _appliedProperties = [];
    private CompositeDisposable _appliedActions = [];

    /// <summary>
    /// Applies the specified configuration settings to the given control, including class and property adjustments.
    /// </summary>
    /// <remarks>This method modifies the control's appearance and behavior based on the provided
    /// configuration. Ensure that the control is properly set up before calling this method.</remarks>
    /// <param name="control">The control to which the configuration settings will be applied. This control must be initialized and part of
    /// the UI hierarchy.</param>
    /// <param name="configuration">The configuration settings that define how the control should be modified. This parameter cannot be null and
    /// must contain valid settings.</param>
    public void Apply(Control control, ControlStyle configuration)
    {
        // Theme
        if (configuration.Theme is null)
            control.ClearValue(StyledElement.ThemeProperty);
        else
            control.SetValue(StyledElement.ThemeProperty, configuration.Theme);

        ApplyClasses(control, configuration.Classes);
        ApplyProperties(control, configuration.Properties);
        ApplyActions(control, configuration.Actions);
    }

    /// <summary>
    /// Applies the specified CSS classes from the configuration to the given control, replacing any previously applied
    /// classes.
    /// </summary>
    /// <remarks>This method removes any classes that were previously applied by this styler before adding the
    /// new classes from the configuration. Null or empty class names in the configuration are ignored.</remarks>
    /// <param name="control">The control to which the CSS classes will be applied.</param>
    /// <param name="classes">The collection of CSS classes to apply to the control.</param>
    private void ApplyClasses(Control control, IReadOnlyList<string> classes)
    {
        // Remove only previously applied classes
        foreach (var cls in _appliedClasses)
            control.Classes.Remove(cls);

        _appliedClasses.Clear();

        foreach (var @class in classes)
            addClass(@class);

        void addClass(string? cls)
        {
            control.Classes.Add(cls);
            _appliedClasses.Add(cls);
        }
    }

    /// <summary>
    /// Applies the properties defined in the configuration to the control. It first clears any previously applied properties to ensure that only the current configuration's properties are active on the control. Then, it iterates through the properties defined in the configuration and applies them to the control if their value differs from the default value. This ensures that only relevant properties are set on the control, maintaining a clean and efficient styling process.
    /// </summary>
    /// <param name="control">The control to which the properties will be applied.</param>
    /// <param name="properties">The collection of properties to apply to the control.</param>
    private void ApplyProperties(Control control, IReadOnlyList<StyleProperty> properties)
    {
        // Remove only previously applied properties
        foreach (var property in _appliedProperties)
            control.ClearValue(property);

        _appliedProperties.Clear();

        // Custom properties
        foreach (var prop in properties)
            addProperty(prop);

        void addProperty(StyleProperty prop)
        {
            var value = prop.Value is MaterialIcon icon ? new MaterialIcon { Data = icon.Data } : prop.Value;

            control.SetValue(prop.Property, value);
            _appliedProperties.Add(prop.Property);
        }
    }

    /// <summary>
    /// Applies the actions defined in the configuration to the control. It first disposes of any previously applied actions to ensure that only the current configuration's actions are active on the control. Then, it iterates through the actions defined in the configuration and subscribes to their subjects, invoking the corresponding action on the control whenever the subject emits a value. This allows for dynamic behavior changes on the control based on the configuration settings.
    /// </summary>
    /// <param name="control">The control to which the actions will be applied.</param>
    /// <param name="actions">The collection of actions to apply to the control.</param>
    private void ApplyActions(Control control, IReadOnlyList<StyleAction> actions)
    {
        _appliedActions.Dispose();
        _appliedActions = [];

        foreach (var action in actions)
            _appliedActions.Add(action.Subject.Subscribe(x => action.Action.Invoke(control, x)));
    }

    /// <summary>
    /// Releases all resources used by the current instance of the class.
    /// </summary>
    /// <remarks>Call this method when the object is no longer needed to free associated resources. Failing to
    /// call this method may result in resource leaks.</remarks>
    public void Dispose() => _appliedActions.Dispose();
}
