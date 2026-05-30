// -----------------------------------------------------------------------
// <copyright file="AvaloniaPropertyHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Frozen;
using System.Globalization;
using System.Linq;
using Avalonia;
using MyNet.Utilities;

namespace MyNet.Avalonia.Theme.Classes.Helpers;

/// <summary>
/// Provides helper methods for registering attached properties in Avalonia that automatically apply CSS classes to controls based on their values.
/// </summary>
public static class AvaloniaPropertyHelper
{
    /// <summary>
    /// Registers an attached property that automatically applies a CSS class to a control.
    /// Note: This method should only be called from static field initializers.
    /// </summary>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="class">The CSS class name to apply.</param>
    /// <param name="defaultValue">The default value of the property.</param>
    /// <param name="inherits">Indicates whether the property value should be inherited by child elements.</param>
    /// <returns>The registered attached property.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("AvaloniaProperty", "AVP1001:The same AvaloniaProperty should not be registered twice", Justification = "Unsafe registration - This method is designed to be called from static initializers")]
    public static AttachedProperty<bool> RegisterBoolProperty(string propertyName, CssClass @class, bool defaultValue = false, bool inherits = false)
    {
        var property = AvaloniaProperty.RegisterAttached<object, StyledElement, bool>(propertyName, defaultValue: defaultValue, inherits: inherits);

        var className = @class.ToString();

        property.Changed.Subscribe(args =>
        {
            if (args.Sender is not StyledElement styledElement)
                return;

            var newValue = args.NewValue.GetValueOrDefault<bool>();
            styledElement.Classes.Set(className, newValue);
        });

        return property;
    }

    /// <summary>
    /// Registers an attached property that automatically applies a CSS class to a control.
    /// Note: This method should only be called from static field initializers.
    /// </summary>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="ownerType">The owner type.</param>
    /// <param name="defaultValue">The default value of the property.</param>
    /// <param name="prefix">The prefix to use for the CSS class.</param>
    /// <param name="inherits">Indicates whether the property value should be inherited by child elements.</param>
    /// <returns>The registered attached property.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("AvaloniaProperty", "AVP1001:The same AvaloniaProperty should not be registered twice", Justification = "Unsafe registration - This method is designed to be called from static initializers")]
    public static AttachedProperty<TEnum> RegisterEnumProperty<TEnum>(string propertyName, Type ownerType, TEnum defaultValue, string prefix = "is", bool inherits = false)
        where TEnum : Enum
    {
        var property = AvaloniaProperty.RegisterAttached<StyledElement, TEnum>(propertyName, ownerType, defaultValue: defaultValue, inherits: inherits);

        var isFlags = typeof(TEnum).IsDefined(typeof(FlagsAttribute), false);

        // Pre-compute class names to avoid per-call string allocations
        var classNames = Enum.GetValues(typeof(TEnum))
            .Cast<TEnum>()
            .Distinct()
            .ToFrozenDictionary(
                v => v,
                v => new CssClass(v.ToString().OrEmpty().ToLower(CultureInfo.CurrentCulture), prefix).ToString());

        if (isFlags)
        {
            // Pre-compute non-zero flag values and their class names
            var flagEntries = classNames
                .Where(kv => Convert.ToInt64(kv.Key, CultureInfo.InvariantCulture) != 0)
                .ToArray();

            property.Changed.Subscribe(args =>
            {
                if (args.Sender is not StyledElement styledElement)
                    return;

                var newValue = args.NewValue.GetValueOrDefault<TEnum>();

                foreach (var (enumValue, className) in flagEntries)
                {
                    var isSet = newValue?.HasFlag(enumValue) ?? false;
                    styledElement.Classes.Set(className, isSet);
                }
            });
        }
        else
        {
            property.Changed.Subscribe(args =>
            {
                if (args.Sender is not StyledElement styledElement)
                    return;

                var oldValue = args.OldValue.GetValueOrDefault<TEnum>();
                var newValue = args.NewValue.GetValueOrDefault<TEnum>();

                if (Equals(oldValue, newValue))
                    return;

                // Remove only the old class, add only the new class (2 changes instead of N)
                if (classNames.TryGetValue(oldValue, out var oldClassName))
                    styledElement.Classes.Set(oldClassName, false);

                if (classNames.TryGetValue(newValue, out var newClassName))
                    styledElement.Classes.Set(newClassName, true);
            });
        }

        return property;
    }

    /// <summary>
    /// Registers an attached property that automatically applies a CSS class to a control.
    /// Note: This method should only be called from static field initializers.
    /// </summary>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="ownerType">The owner type.</param>
    /// <param name="defaultValue">The default value of the property.</param>
    /// <param name="prefix">The prefix to use for the CSS class.</param>
    /// <param name="inherits">Indicates whether the property value should be inherited by child elements.</param>
    /// <returns>The registered attached property.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("AvaloniaProperty", "AVP1001:The same AvaloniaProperty should not be registered twice", Justification = "Unsafe registration - This method is designed to be called from static initializers")]
    public static AttachedProperty<string> RegisterStringProperty(string propertyName, Type ownerType, string defaultValue, string prefix = "is", bool inherits = false)
    {
        var property = AvaloniaProperty.RegisterAttached<StyledElement, string>(propertyName, ownerType, defaultValue: defaultValue, inherits: inherits);

        property.Changed.Subscribe(args =>
        {
            if (args.Sender is not StyledElement styledElement)
                return;

            var oldValue = args.OldValue.GetValueOrDefault<string>();
            var newValue = args.NewValue.GetValueOrDefault<string>();

            if (!string.IsNullOrEmpty(oldValue))
            {
                var oldClassName = new CssClass(oldValue.ToLower(CultureInfo.CurrentCulture), prefix).ToString();
                styledElement.Classes.Set(oldClassName, false);
            }

            if (!string.IsNullOrEmpty(newValue))
            {
                var newClassName = new CssClass(newValue.ToLower(CultureInfo.CurrentCulture), prefix).ToString();
                styledElement.Classes.Set(newClassName, true);
            }
        });

        return property;
    }
}
