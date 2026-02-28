// -----------------------------------------------------------------------
// <copyright file="AvaloniaPropertyHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using System.Linq;
using Avalonia;
using MyNet.Avalonia.Extensions;
using MyNet.Utilities;

namespace MyNet.Avalonia.Helpers;

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
    /// <returns>The registered attached property.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("AvaloniaProperty", "AVP1001:The same AvaloniaProperty should not be registered twice", Justification = "Unsafe registration - This method is designed to be called from static initializers")]
    public static AttachedProperty<bool> RegisterBoolProperty(string propertyName, CssClass @class, bool defaultValue = false)
    {
        var property = AvaloniaProperty.RegisterAttached<object, StyledElement, bool>(propertyName, defaultValue: defaultValue);

        property.Changed.Subscribe(args =>
        {
            if (args.Sender is not StyledElement styledElement)
                return;

            var newValue = args.NewValue.GetValueOrDefault<bool>();
            styledElement.Classes.Set(@class.ToString(), newValue);
        });

        return property;
    }

    /// <summary>
    /// Registers an attached property that automatically applies a CSS class to a control.
    /// Note: This method should only be called from static field initializers.
    /// </summary>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="defaultValue">The default value of the property.</param>
    /// <param name="prefix">The prefix to use for the CSS class.</param>
    /// <returns>The registered attached property.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("AvaloniaProperty", "AVP1001:The same AvaloniaProperty should not be registered twice", Justification = "Unsafe registration - This method is designed to be called from static initializers")]
    public static AttachedProperty<TEnum> RegisterEnumProperty<TEnum>(string propertyName, TEnum defaultValue, string prefix = "is")
        where TEnum : Enum
    {
        var property = AvaloniaProperty.RegisterAttached<object, StyledElement, TEnum>(propertyName, defaultValue: defaultValue);

        var isFlags = typeof(TEnum).IsDefined(typeof(FlagsAttribute), false);

        property.Changed.Subscribe(args =>
        {
            if (args.Sender is not StyledElement styledElement)
                return;

            var newValue = args.NewValue.GetValueOrDefault<TEnum>();

            if (isFlags)
            {
                // Pour chaque valeur de l'enum sauf 0, applique la classe si le flag est activé
                foreach (var enumValue in Enum.GetValues(typeof(TEnum)).OfType<TEnum>())
                {
                    var intValue = Convert.ToInt64(enumValue, CultureInfo.InvariantCulture);
                    if (intValue == 0) continue;

                    var className = enumValue.ToString().OrEmpty().ToLower(CultureInfo.CurrentCulture);
                    var isSet = newValue?.HasFlag(enumValue) ?? false;
                    styledElement.Classes.Set(new CssClass(className, prefix).ToString(), isSet);
                }
            }
            else
            {
                foreach (var enumValue in Enum.GetValues(typeof(TEnum)))
                {
                    var className = enumValue.ToString().OrEmpty().ToLower(CultureInfo.CurrentCulture);
                    styledElement.Classes.Set(new CssClass(className, prefix).ToString(), Equals(newValue, enumValue));
                }
            }
        });

        return property;
    }

    /// <summary>
    /// Registers an attached property that automatically applies a CSS class to a control.
    /// Note: This method should only be called from static field initializers.
    /// </summary>
    /// <param name="propertyName">The name of the property.</param>
    /// <param name="defaultValue">The default value of the property.</param>
    /// <param name="prefix">The prefix to use for the CSS class.</param>
    /// <returns>The registered attached property.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("AvaloniaProperty", "AVP1001:The same AvaloniaProperty should not be registered twice", Justification = "Unsafe registration - This method is designed to be called from static initializers")]
    public static AttachedProperty<string> RegisterStringProperty(string propertyName, string defaultValue, string prefix = "is")
    {
        var property = AvaloniaProperty.RegisterAttached<object, StyledElement, string>(propertyName, defaultValue: defaultValue);

        property.Changed.Subscribe(args =>
        {
            if (args.Sender is not StyledElement styledElement)
                return;

            var newValue = args.NewValue.GetValueOrDefault<string>();

            var className = newValue.OrEmpty().ToLower(CultureInfo.CurrentCulture);
            styledElement.Classes.Set(new CssClass(className, prefix).ToString(), true);
        });

        return property;
    }
}

/// <summary>
/// Provides a record type for representing a CSS class name with an optional prefix that can be used to generate the full class name with the prefix applied.
/// </summary>
/// <param name="Name">The name of the CSS class.</param>
/// <param name="Prefix">An optional prefix to apply to the CSS class name.</param>
public record CssClass(string Name, string? Prefix = "")
{
    /// <summary>
    /// Provides a string representation of the CSS class name with the prefix applied, if a prefix is specified.
    /// </summary>
    /// <returns>The full CSS class name with the prefix applied, if any.</returns>
    public override string ToString() => Name.WithPrefix(Prefix);
}
