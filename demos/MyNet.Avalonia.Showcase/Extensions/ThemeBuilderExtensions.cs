// -----------------------------------------------------------------------
// <copyright file="ThemeBuilderExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Theme.Classes;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.Avalonia.Theme.Theming.Core;
using MyNet.Humanizer.Facade;

namespace MyNet.Avalonia.Showcase.Extensions;

/// <summary>
/// Provides extension methods for <see cref="ControlThemeBuilder"/> to add custom CSS classes related to control positioning and header alignment.
/// </summary>
internal static class ThemeBuilderExtensions
{
    extension(ControlThemeBuilder themeBuilder)
    {
        #region Variants

        /// <summary>
        /// Adds one or more variant values to the control theme builder.
        /// </summary>
        /// <remarks>Use this method to add multiple variants in a single call, allowing for flexible theme
        /// customization.</remarks>
        /// <param name="values">An array of variant values to add. Each value specifies a variant that customizes the appearance or behavior of
        /// the control. Cannot be null.</param>
        /// <returns>The current instance of the ControlThemeBuilder, enabling method chaining.</returns>
        public ControlThemeBuilder AddVariants(params ControlVariant[] values) => themeBuilder.AddVariants([.. values.Select(CssClass.Variant)]);

        /// <summary>
        /// Adds one or more variant values to the control theme builder.
        /// </summary>
        /// <remarks>Use this method to add multiple variants in a single call, allowing for flexible theme
        /// customization.</remarks>
        /// <param name="values">An array of variant values to add. Each value specifies a variant that customizes the appearance or behavior of
        /// the control. Cannot be null.</param>
        /// <returns>The current instance of the ControlThemeBuilder, enabling method chaining.</returns>
        public ControlThemeBuilder AddVariants(params string[] values) => themeBuilder.AddVariants([.. values.Select(x => new CssClass(x))]);

        /// <summary>
        /// Adds a standard set of common control variants (Light, Outlined, Text) to the control theme configuration.
        /// </summary>
        /// <returns>The current instance of the ControlThemeBuilder, enabling method chaining.</returns>
        public ControlThemeBuilder AddDefaultVariants() => themeBuilder.AddVariants(ControlVariant.Light, ControlVariant.Outlined, ControlVariant.Text);

        /// <summary>
        /// Adds a standard set of common control variants (Solid, Light, Outlined, Text) to the control theme configuration.
        /// </summary>
        /// <returns>The current instance of the ControlThemeBuilder, enabling method chaining.</returns>
        public ControlThemeBuilder AddStandardVariants() => themeBuilder.AddVariants(ControlVariant.Solid, ControlVariant.Light, ControlVariant.Outlined, ControlVariant.Text);

        /// <summary>
        /// Adds a standard set of common control variants (Solid, Light, Outlined, Text, Transparent) to the control theme configuration.
        /// </summary>
        /// <returns>The current instance of the ControlThemeBuilder, enabling method chaining.</returns>
        public ControlThemeBuilder AddAllVariants() => themeBuilder.AddVariants(ControlVariant.Solid, ControlVariant.Light, ControlVariant.Outlined, ControlVariant.Text, ControlVariant.Transparent);

        /// <summary>
        /// Adds one or more variant values to the control theme builder.
        /// </summary>
        /// <remarks>Use this method to add multiple variants in a single call, allowing for flexible theme
        /// customization.</remarks>
        /// <param name="values">An array of variant values to add. Each value specifies a variant that customizes the appearance or behavior of
        /// the control. Cannot be null.</param>
        /// <returns>The current instance of the ControlThemeBuilder, enabling method chaining.</returns>
        public ControlThemeBuilder AddItemsVariants(params ControlVariant[] values) => themeBuilder.AddVariants([.. values.Select(CssClass.ItemsVariant)]);

        /// <summary>
        /// Adds a standard set of common control variants (Light, Outlined, Text) to the control theme configuration.
        /// </summary>
        /// <returns>The current instance of the ControlThemeBuilder, enabling method chaining.</returns>
        public ControlThemeBuilder AddItemsDefaultVariants() => themeBuilder.AddItemsVariants(ControlVariant.Light, ControlVariant.Outlined, ControlVariant.Text);

        /// <summary>
        /// Adds a standard set of common control variants (Solid, Light, Outlined, Text) to the control theme configuration.
        /// </summary>
        /// <returns>The current instance of the ControlThemeBuilder, enabling method chaining.</returns>
        public ControlThemeBuilder AddItemsStandardVariants() => themeBuilder.AddItemsVariants(ControlVariant.Solid, ControlVariant.Light, ControlVariant.Outlined, ControlVariant.Text);

        /// <summary>
        /// Adds a standard set of common control variants (Solid, Light, Outlined, Text, Transparent) to the control theme configuration.
        /// </summary>
        /// <returns>The current instance of the ControlThemeBuilder, enabling method chaining.</returns>
        public ControlThemeBuilder AddItemsAllVariants() => themeBuilder.AddItemsVariants(ControlVariant.Solid, ControlVariant.Light, ControlVariant.Outlined, ControlVariant.Text, ControlVariant.Transparent);

        /// <summary>
        /// Adds one or more variant values to the control theme builder.
        /// </summary>
        /// <remarks>Use this method to add multiple variants in a single call, allowing for flexible theme
        /// customization.</remarks>
        /// <param name="values">An array of variant values to add. Each value specifies a variant that customizes the appearance or behavior of
        /// the control. Cannot be null.</param>
        /// <returns>The current instance of the ControlThemeBuilder, enabling method chaining.</returns>
        public ControlThemeBuilder AddHeaderVariants(params ControlVariant[] values) => themeBuilder.AddVariants([.. values.Select(CssClass.HeaderVariant)]);

        /// <summary>
        /// Adds a standard set of common control variants (Light, Outlined, Text) to the control theme configuration.
        /// </summary>
        /// <returns>The current instance of the ControlThemeBuilder, enabling method chaining.</returns>
        public ControlThemeBuilder AddHeaderDefaultVariants() => themeBuilder.AddHeaderVariants(ControlVariant.Light, ControlVariant.Outlined, ControlVariant.Text);

        /// <summary>
        /// Adds a standard set of common control variants (Solid, Light, Outlined, Text) to the control theme configuration.
        /// </summary>
        /// <returns>The current instance of the ControlThemeBuilder, enabling method chaining.</returns>
        public ControlThemeBuilder AddHeaderStandardVariants() => themeBuilder.AddHeaderVariants(ControlVariant.Solid, ControlVariant.Light, ControlVariant.Outlined, ControlVariant.Text);

        /// <summary>
        /// Adds a standard set of common control variants (Solid, Light, Outlined, Text, Transparent) to the control theme configuration.
        /// </summary>
        /// <returns>The current instance of the ControlThemeBuilder, enabling method chaining.</returns>
        public ControlThemeBuilder AddHeaderAllVariants() => themeBuilder.AddHeaderVariants(ControlVariant.Solid, ControlVariant.Light, ControlVariant.Outlined, ControlVariant.Text, ControlVariant.Transparent);

        #endregion

        #region Sizes

        /// <summary>
        /// Adds one or more size values to the control theme builder.
        /// </summary>
        /// <remarks>This method allows multiple size values to be added in a single call, which can simplify the
        /// configuration of control dimensions.</remarks>
        /// <param name="values">The size values to add. Each value specifies a size for the control theme. Cannot be null.</param>
        /// <returns>The current instance of the ControlThemeBuilder, enabling method chaining.</returns>
        public ControlThemeBuilder AddSizes(params string[] values) => themeBuilder.AddSizes([.. values.Select(x => new CssClass(x))]);

        /// <summary>
        /// Adds one or more size options to the control theme builder based on the provided spacing size values.
        /// </summary>
        /// <param name="values">An array of spacing size values to add. Each value specifies a size for the control theme. Cannot be null.</param>
        /// <returns>The current instance of the ControlThemeBuilder, enabling method chaining.</returns>
        public ControlThemeBuilder AddSizes(params SpacingSize[] values) => themeBuilder.AddSizes([.. values.Select(CssClass.Size)]);

        /// <summary>
        /// Adds a standard set of small, medium, and large spacing sizes to the control theme configuration.
        /// </summary>
        /// <remarks>Use this method to quickly apply consistent default spacing values to a control theme. This
        /// helps ensure uniformity across user interface elements.</remarks>
        /// <returns>A reference to the current instance of the ControlThemeBuilder, enabling method chaining.</returns>
        public ControlThemeBuilder AddDefaultSizes() => themeBuilder.AddSizes(SpacingSize.Sm, SpacingSize.Md, SpacingSize.Lg);

        /// <summary>
        /// Adds a comprehensive set of spacing sizes (extra small, small, medium, large, extra large) to the control theme configuration.
        /// </summary>
        /// <returns>A reference to the current instance of the ControlThemeBuilder, enabling method chaining.</returns>
        public ControlThemeBuilder AddStandardSizes() => themeBuilder.AddSizes(SpacingSize.Xs, SpacingSize.Sm, SpacingSize.Md, SpacingSize.Lg, SpacingSize.Xl);

        /// <summary>
        /// Adds a comprehensive set of spacing sizes (extra small, small, medium, large, extra large) to the control theme configuration.
        /// </summary>
        /// <returns>A reference to the current instance of the ControlThemeBuilder, enabling method chaining.</returns>
        public ControlThemeBuilder AddAllSizes() => themeBuilder.AddSizes([.. Enum.GetValues<SpacingSize>()]);

        #endregion

        #region Roles

        /// <summary>
        /// Adds default role definitions (Primary, Accent, Inverse, Success, Warning, Error, Information).
        /// </summary>
        /// <returns>The current builder instance for method chaining.</returns>
        public ControlThemeBuilder AddAllRoles() => themeBuilder.AddRoles([.. Enum.GetValues<ThemeRole>()]);

        /// <summary>
        /// Adds default role definitions (Primary, Accent, Contrast, Success, Warning, Error, Information).
        /// </summary>
        /// <returns>The current builder instance for method chaining.</returns>
        public ControlThemeBuilder AddDefaultRoles() => themeBuilder.AddRoles([.. Enum.GetValues<ThemeRole>().Except([ThemeRole.Neutral, ThemeRole.Inverse])]);

        /// <summary>
        /// Adds theme role definitions (Primary, Accent, Contrast).
        /// </summary>
        /// <returns>The current builder instance for method chaining.</returns>
        public ControlThemeBuilder AddThemeRoles() => themeBuilder.AddRoles(ThemeRole.Default, ThemeRole.Primary, ThemeRole.Accent, ThemeRole.Contrast);

        /// <summary>
        /// Adds default role definitions (Primary, Accent, Inverse, Success, Warning, Error, Information).
        /// </summary>
        /// <returns>The current builder instance for method chaining.</returns>
        public ControlThemeBuilder AddItemsAllRoles() => themeBuilder.AddItemsRoles([.. Enum.GetValues<ThemeRole>()]);

        /// <summary>
        /// Adds default role definitions (Primary, Accent, Contrast, Success, Warning, Error, Information).
        /// </summary>
        /// <returns>The current builder instance for method chaining.</returns>
        public ControlThemeBuilder AddItemsDefaultRoles() => themeBuilder.AddItemsRoles([.. Enum.GetValues<ThemeRole>().Except([ThemeRole.Neutral, ThemeRole.Inverse])]);

        /// <summary>
        /// Adds theme role definitions (Primary, Accent, Contrast).
        /// </summary>
        /// <returns>The current builder instance for method chaining.</returns>
        public ControlThemeBuilder AddItemsThemeRoles() => themeBuilder.AddItemsRoles(ThemeRole.Default, ThemeRole.Primary, ThemeRole.Accent, ThemeRole.Contrast);

        #endregion

        #region Options

        /// <summary>
        /// Adds a selection control for an enumeration type, allowing users to choose from a predefined set of options.
        /// </summary>
        /// <typeparam name="T">The enumeration type to display in the selection control. Must be a value type that derives from Enum.</typeparam>
        /// <typeparam name="TEditor">The type of the editor to use for displaying the enumeration values. Must implement <see cref="IEditorWithChoicesBuilder"/> and have a parameterless constructor.</typeparam>
        /// <param name="options">A collection of enumeration values representing the available options for selection. Cannot be null or
        /// empty.</param>
        /// <param name="defaultValue">The value to be selected by default when the control is initialized. If not specified, the default value of
        /// the enumeration type is used.</param>
        /// <param name="configure">An optional action to configure the metadata for the selection options, enabling customization of display
        /// properties.</param>
        /// <param name="configureEditor">An optional action to configure the metadata for the editor of the property.</param>
        /// <param name="configureChoice">An optional action to configure the metadata for each individual choice, allowing further customization
        /// based on the specific enumeration value.</param>
        /// <param name="onValueChanged">An optional action that is invoked when the value of the selection control changes, allowing for dynamic response to user interactions.</param>
        /// <returns>The updated instance of ControlThemeBuilder, enabling method chaining.</returns>
        public ControlThemeBuilder AddEnumClass<T, TEditor>(ICollection<T> options,
                                                            T defaultValue = default,
                                                            Action<OptionMetadataBuilder>? configure = null,
                                                            Action<TEditor>? configureEditor = null,
                                                            Action<T, ChoiceMetadataBuilder>? configureChoice = null,
                                                            Action<Control, object?>? onValueChanged = null)
            where T : struct, Enum
            where TEditor : IEditorWithChoicesBuilder, new()
            => themeBuilder.AddClass(CssClass.FromEnum(defaultValue),
                x =>
                {
                    x.Of<TEditor>(editor =>
                    {
                        editor.AddChoices([.. options.Select(CssClass.FromEnum)], (cssClass, y) =>
                                            {
                                                var value = cssClass.ToEnum<T>();
                                                if (!value.HasValue) return;

                                                y.DisplayName(() => value.Value.Humanize());

                                                configureChoice?.Invoke(value.Value, y);
                                            });

                        configureEditor?.Invoke(editor);
                    });

                    configure?.Invoke(x);
                },
                onValueChanged);

        /// <summary>
        /// Adds a selection control to the theme builder that allows users to choose from all values of the specified
        /// enumeration type.
        /// </summary>
        /// <remarks>Use this method to create a UI element that presents a set of predefined options
        /// based on an enumeration. This is useful for scenarios where the user must select from a fixed set of
        /// choices, such as modes or categories. Both the default selection and the appearance or behavior of each
        /// option can be customized through the provided configuration actions.</remarks>
        /// <typeparam name="T">The enumeration type that defines the available options for selection.</typeparam>
        /// <typeparam name="TEditor">The type of the editor to use for displaying the enumeration values. Must implement <see cref="IEditorWithChoicesBuilder"/> and have a parameterless constructor.</typeparam>
        /// <param name="defaultValue">The value to be selected by default. If not specified, the default value of the enumeration type is used.</param>
        /// <param name="configure">An optional action to configure metadata for each option in the selection control.</param>
        /// <param name="configureEditor">An optional action to configure the metadata for the editor of the property.</param>
        /// <param name="configureChoice">An optional action to configure additional metadata for the selected choice, based on the enumeration value.</param>
        /// <param name="onValueChanged">An optional action that is invoked when the value of the selection control changes, allowing for dynamic response to user interactions.</param>
        /// <returns>The current instance of the ControlThemeBuilder, enabling method chaining.</returns>
        public ControlThemeBuilder AddEnumClass<T, TEditor>(T defaultValue = default,
                                                            Action<OptionMetadataBuilder>? configure = null,
                                                            Action<TEditor>? configureEditor = null,
                                                            Action<T, ChoiceMetadataBuilder>? configureChoice = null,
                                                            Action<Control, object?>? onValueChanged = null)
            where T : struct, Enum
            where TEditor : IEditorWithChoicesBuilder, new()
            => themeBuilder.AddEnumClass(Enum.GetValues<T>(), defaultValue, configure, configureEditor, configureChoice, onValueChanged);

        /// <summary>
        /// Adds a selection control for an enumeration type based on a specific Avalonia property, allowing users to choose from a predefined set of options.
        /// </summary>
        /// <typeparam name="T">The enumeration type that defines the available options for selection.</typeparam>
        /// <typeparam name="TEditor">The type of the editor to use for displaying the enumeration values. Must implement <see cref="IEditorWithChoicesBuilder"/> and have a parameterless constructor.</typeparam>
        /// <param name="property">The Avalonia property associated with the selection control.</param>
        /// <param name="options">A collection of enumeration values representing the available options for selection.</param>
        /// <param name="defaultValue">The value to be selected by default. If not specified, the default value of the enumeration type is used.</param>
        /// <param name="configure">An optional action to configure metadata for each option in the selection control.</param>
        /// <param name="configureEditor">An optional action to configure the metadata for the editor of the property.</param>
        /// <param name="configureChoice">An optional action to configure additional metadata for the selected choice, based on the enumeration value.</param>
        /// <param name="onValueChanged">An optional action that is invoked when the value of the selection control changes, allowing for dynamic response to user interactions.</param>
        /// <returns>The current instance of the ControlThemeBuilder, enabling method chaining.</returns>
        public ControlThemeBuilder AddEnumProperty<T, TEditor>(AvaloniaProperty<T> property,
                                                               ICollection<T> options,
                                                               T defaultValue = default,
                                                               Action<OptionMetadataBuilder>? configure = null,
                                                               Action<TEditor>? configureEditor = null,
                                                               Action<T, ChoiceMetadataBuilder>? configureChoice = null,
                                                               Action<Control, object?>? onValueChanged = null)
            where T : struct, Enum
            where TEditor : IEditorWithChoicesBuilder, new()
            => themeBuilder.AddProperty(property,
                defaultValue,
                x =>
                {
                    x.Of<TEditor>(editor =>
                    {
                        editor.AddChoices(options, (@enum, y) =>
                                                {
                                                    y.DisplayName(() => @enum.Humanize());

                                                    configureChoice?.Invoke(@enum, y);
                                                });

                        configureEditor?.Invoke(editor);
                    });

                    configure?.Invoke(x);
                },
                onValueChanged);

        /// <summary>
        /// Adds an enumeration property to the control theme builder, enabling customization of the property with a set
        /// of predefined choices.
        /// </summary>
        /// <remarks>Use this method to add an enum property to a control theme, specifying available
        /// choices and optional configuration for both the property and its individual choices. This is useful for
        /// exposing a set of selectable options in a themeable control.</remarks>
        /// <typeparam name="T">The enumeration type of the property. Must be a value type that implements <see cref="System.Enum"/>.</typeparam>
        /// <typeparam name="TEditor">The type of the editor to use for displaying the enumeration values. Must implement <see cref="IEditorWithChoicesBuilder"/> and have a parameterless constructor.</typeparam>
        /// <param name="property">The Avalonia property to associate with the enumeration values.</param>
        /// <param name="defaultValue">The default value to assign to the property if no value is specified.</param>
        /// <param name="configure">An optional action to configure the metadata for the property.</param>
        /// <param name="configureEditor">An optional action to configure the metadata for the editor of the property.</param>
        /// <param name="configureChoice">An optional action to configure the metadata for each enumeration choice.</param>
        /// <param name="onValueChanged">An optional action that is invoked when the value of the selection control changes, allowing for dynamic response to user interactions.</param>
        /// <returns>The current instance of <see cref="ControlThemeBuilder"/>, allowing for method chaining.</returns>
        public ControlThemeBuilder AddEnumProperty<T, TEditor>(AvaloniaProperty<T> property,
                                                               T defaultValue = default,
                                                               Action<OptionMetadataBuilder>? configure = null,
                                                               Action<TEditor>? configureEditor = null,
                                                               Action<T, ChoiceMetadataBuilder>? configureChoice = null,
                                                               Action<Control, object?>? onValueChanged = null)
            where T : struct, Enum
            where TEditor : IEditorWithChoicesBuilder, new()
            => themeBuilder.AddEnumProperty(property, Enum.GetValues<T>(), defaultValue, configure, configureEditor, configureChoice, onValueChanged);

        /// <summary>
        /// Adds a selection control for an enumeration type based on a specific Avalonia property, allowing users to choose from all values of the specified enumeration type.
        /// </summary>
        /// <typeparam name="T">The enumeration type of the property. Must be a value type that implements <see cref="System.Enum"/>.</typeparam>
        /// <typeparam name="TEditor">The type of the editor to use for displaying the enumeration values. Must implement <see cref="IEditorWithChoicesBuilder"/> and have a parameterless constructor.</typeparam>
        /// <param name="options">The collection of enumeration values to display as choices.</param>
        /// <param name="onValueChanged">An action that is invoked when the value of the selection control changes, allowing for dynamic response to user interactions.</param>
        /// <param name="defaultValue">The default value to assign to the property if no value is specified.</param>
        /// <param name="configure">An optional action to configure the metadata for the property.</param>
        /// <param name="configureEditor">An optional action to configure the metadata for the editor of the property.</param>
        /// <param name="configureChoice">An optional action to configure the metadata for each enumeration choice.</param>
        /// <returns>The current instance of <see cref="ControlThemeBuilder"/>, allowing for method chaining.</returns>
        public ControlThemeBuilder AddEnumValue<T, TEditor>(ICollection<T> options,
                                                            Action<Control, T?> onValueChanged,
                                                            T defaultValue = default,
                                                            Action<OptionMetadataBuilder>? configure = null,
                                                            Action<TEditor>? configureEditor = null,
                                                            Action<T, ChoiceMetadataBuilder>? configureChoice = null)
            where T : struct, Enum
            where TEditor : IEditorWithChoicesBuilder, new()
            => themeBuilder.AddValueAction((x, y) => onValueChanged.Invoke(x, (T?)y),
                defaultValue,
                x =>
                {
                    x.Of<TEditor>(editor =>
                    {
                        editor.AddChoices(options, (@enum, y) =>
                                            {
                                                y.DisplayName(() => @enum.Humanize());

                                                configureChoice?.Invoke(@enum, y);
                                            });

                        configureEditor?.Invoke(editor);
                    });

                    configure?.Invoke(x);
                });

        /// <summary>
        /// Adds a selection control for an enumeration type, allowing users to choose from all values of the specified enumeration type.
        /// </summary>
        /// <typeparam name="T">The enumeration type.</typeparam>
        /// <typeparam name="TEditor">The editor type.</typeparam>
        /// <param name="onValueChanged">The action to perform when the value changes.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="configure">The action to configure the option metadata.</param>
        /// <param name="configureEditor">The action to configure the editor.</param>
        /// <param name="configureChoice">The action to configure each choice.</param>
        /// <returns>The updated ControlThemeBuilder instance.</returns>
        public ControlThemeBuilder AddEnumValue<T, TEditor>(Action<Control, T?> onValueChanged,
                                                            T defaultValue = default,
                                                            Action<OptionMetadataBuilder>? configure = null,
                                                            Action<TEditor>? configureEditor = null,
                                                            Action<T, ChoiceMetadataBuilder>? configureChoice = null)
            where T : struct, Enum
            where TEditor : IEditorWithChoicesBuilder, new()
            => themeBuilder.AddEnumValue(Enum.GetValues<T>(), onValueChanged, defaultValue, configure, configureEditor, configureChoice);

        #endregion
    }
}
