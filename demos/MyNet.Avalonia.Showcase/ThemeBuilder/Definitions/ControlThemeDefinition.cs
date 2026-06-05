// -----------------------------------------------------------------------
// <copyright file="ControlThemeDefinition.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Avalonia.Styling;
using MyNet.Avalonia.Theme.Classes;
using MyNet.Avalonia.Theme.Controls.Assists;
using MyNet.Avalonia.Theme.Theming.Core;

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;

/// <summary>
/// Represents the definition of a control theme, which includes various properties and options that define the appearance and behavior of a control in a theming system. This class encapsulates information about the control's theme, such as its variants, roles, sizes, shapes, and custom settings. The <see cref="ControlThemeDefinition"/> class serves as a central point for defining the characteristics of a control's theme, allowing for consistent styling and theming across different controls in an application. The constructor takes an optional <see cref="ControlTheme"/> instance and an optional key to identify the theme definition, while the properties provide access to various aspects of the control's theme configuration.
/// </summary>
/// <param name="theme">The optional control theme instance.</param>
/// <param name="key">The optional key to identify the theme definition.</param>
internal sealed class ControlThemeDefinition(ControlTheme? theme, string? key = null)
{
    /// <summary>
    /// Gets the theme that defines the visual appearance of the control.
    /// </summary>
    /// <remarks>The theme specifies the control's colors, fonts, and other stylistic elements. This property
    /// is read-only and reflects the theme assigned during initialization.</remarks>
    public ControlTheme? Theme { get; } = theme;

    /// <summary>
    /// Gets the unique key associated with the current instance.
    /// </summary>
    public string? Key { get; } = key;

    /// <summary>
    /// Gets the CSS class associated with the current instance, if any.
    /// </summary>
    /// <remarks>This property may return null if no CSS class is assigned. It is intended to provide a way to
    /// specify styling for the associated element.</remarks>
    public CssClass? Kind { get; init; }

    /// <summary>
    /// Gets the definition of the available variants for the control class, which specifies the different
    /// configurations that can be applied to the control.
    /// </summary>
    /// <remarks>Use this property to access or initialize the set of variant definitions that determine how
    /// the control can be customized or themed. Variants allow for flexible styling and behavior adjustments based on
    /// different scenarios or requirements.</remarks>
    public ICollection<CssClass> Variants { get; init; } = [];

    /// <summary>
    /// Gets the property definition that specifies the theme role associated with the control.
    /// </summary>
    /// <remarks>This property enables consistent theming by associating the control with a specific role
    /// defined in the theme system. The role determines the control's appearance and behavior within the applied
    /// theme.</remarks>
    public ICollection<ThemeRole> Roles { get; init; } = [];

    /// <summary>
    /// Gets the property definition that specifies the role for items within the control, which determines their visual
    /// styling and behavior.
    /// </summary>
    /// <remarks>This property is initialized with the default role property from the ItemsAssist class,
    /// ensuring consistent theming and role assignment across controls. The role definition can be used to apply
    /// specific styles or behaviors to items based on their assigned role.</remarks>
    public ICollection<ThemeRole> ItemsRoles { get; init; } = [];

    /// <summary>
    /// Gets the size configuration for the control, defining the dimensions of its visual elements.
    /// </summary>
    /// <remarks>Use this property to customize the sizing of control components to fit specific layout or
    /// design requirements. The returned definition can be modified to adjust the appearance of the control as
    /// needed.</remarks>
    public ICollection<CssClass> Sizes { get; init; } = [];

    /// <summary>
    /// Gets the collection of shape definitions associated with the control class.
    /// </summary>
    /// <remarks>The collection is initialized as empty and can be populated with shape definitions as needed
    /// to describe the visual structure of the control. Modifying this collection allows customization of the control's
    /// appearance.</remarks>
    public ICollection<CssClass> Shapes { get; init; } = [];

    /// <summary>
    /// Gets a read-only collection of custom settings that configure the control options.
    /// </summary>
    /// <remarks>Each item in the collection defines a configurable option for the control. Use these settings
    /// to customize the behavior or appearance of the control as needed.</remarks>
    public IReadOnlyList<IControlOptionDefinition> CustomSettings { get; init; } = [];

    /// <summary>
    /// Gets the content definition that specifies the structure and behavior of the control's content.
    /// </summary>
    /// <remarks>The value may be null if no content definition is set. Check for null before accessing
    /// members of the content definition.</remarks>
    public ControlContentDefinition? ContentDefinition { get; init; }

    /// <summary>
    /// Gets the definition of the icon property associated with the control.
    /// </summary>
    /// <remarks>This property provides access to the icon definition, which can be used to customize the
    /// appearance of the control's icon. It is initialized with a default value based on the
    /// IconAssist.IconProperty.</remarks>
    public IControlPropertyDefinition IconDefinition { get; init; } = new ControlAttachedPropertyDefinition<object>(IconAssist.IconProperty);

    /// <summary>
    /// Gets a value indicating whether the control should use an icon by default when no specific icon is provided.
    /// </summary>
    public bool UseIconByDefault { get; init; }
}
