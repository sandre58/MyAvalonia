// -----------------------------------------------------------------------
// <copyright file="ControlDefinitionConverter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Styling;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog.ContentProviders;
using MyNet.Avalonia.Theme.Palettes;
using MyNet.Utilities;

namespace MyNet.Avalonia.Demo.Converters;

internal sealed class ControlDefinitionConverter : IMultiValueConverter, IValueConverter
{
    public static readonly ControlDefinitionConverter Default = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => Convert([value], targetType, parameter, culture);

    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Count == 0 || values[0] is not ControlThemeDefinition themeDefinition) return AvaloniaProperty.UnsetValue;

        var result = new ControlDefinition(themeDefinition.Theme, ThemeRole.Default, [themeDefinition.Kind.OrEmpty()], themeDefinition.DisplayName, ThemeRole.Default, themeDefinition.DefaultContentType);

        if (values.Count > 1 && values[1] is AppearanceDefinition appearanceDefinition)
        {
            result.DisplayName = appearanceDefinition.DisplayName;
            switch (appearanceDefinition)
            {
                case ShapeDefinition shapeDefinition:
                    result.Classes.AddRange([shapeDefinition.Class]);
                    break;

                case VariantDefinition variantDefinition:
                    result.Classes.AddRange(variantDefinition.Classes);
                    break;

                case SizeDefinition sizeDefinition:
                    result.Classes.AddRange([sizeDefinition.Class]);
                    break;

                case RoleDefinition roleDefinition:
                    if (roleDefinition.IsItemsRole)
                        result.ItemsRole = roleDefinition.Role;
                    else
                        result.Role = roleDefinition.Role;
                    break;
            }
        }

        return result;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

/// <summary>
/// Base record for control definitions.
/// </summary>
/// <param name="theme">The control theme.</param>
/// <param name="role">The theme role.</param>
/// <param name="classes">The array of CSS classes.</param>
/// <param name="displayName">The display name.</param>
/// <param name="itemsRole">The theme role for items.</param>
/// <param name="defaultContentType">The default content to display.</param>
internal sealed class ControlDefinition(ControlTheme? theme, ThemeRole role, List<string> classes, string? displayName, ThemeRole itemsRole = ThemeRole.Default, ContentProviderType defaultContentType = ContentProviderType.None)
{
    public ControlTheme? Theme { get; set; } = theme;

    public ThemeRole Role { get; set; } = role;

    public List<string> Classes { get; set; } = classes;

    public string? DisplayName { get; set; } = displayName;

    public ThemeRole ItemsRole { get; set; } = itemsRole;

    public ContentProviderType DefaultContentType { get; set; } = defaultContentType;
}
