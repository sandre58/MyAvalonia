// -----------------------------------------------------------------------
// <copyright file="ControlThemeBuilder.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;
using MyNet.Avalonia.Showcase.ThemeBuilder.Metadata;
using MyNet.Avalonia.Showcase.ThemeBuilder.Registry;
using MyNet.Avalonia.Theme;
using MyNet.Avalonia.Theme.Classes;
using MyNet.Avalonia.Theme.Controls.Assists;
using MyNet.Avalonia.Theme.Theming;
using MyNet.Avalonia.Theme.Theming.Core;
using MyNet.Observable;
using MyNet.Text;

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Builders;

/// <summary>
/// Provides a builder for constructing control themes with customizable options such as shapes, variants, sizes, and
/// roles.
/// </summary>
/// <remarks>This builder allows for a fluent API to configure various aspects of a control theme, enabling
/// developers to create themes tailored to specific controls. It supports adding shapes, variants, sizes, and roles,
/// and provides methods to build the final theme definition.</remarks>
/// <param name="themeKey">The optional name of the theme to be applied. If null, the default theme will be used.</param>
internal sealed class ControlThemeBuilder(string? themeKey = null)
{
    private static readonly ConcurrentDictionary<string, ControlTheme> ThemeCache = new();
    private static int _themeChangedSubscribed;

    private readonly List<CssClass> _shapes = [];
    private readonly List<CssClass> _variants = [];
    private readonly List<CssClass> _sizes = [];
    private readonly List<ThemeRole> _roles = [];
    private readonly List<ThemeRole> _itemsRoles = [];
    private readonly List<IControlOptionDefinition> _customOptions = [];
    private CssClass? _kind;
    private ControlContentDefinition? _controlContentDefinition;
    private IControlPropertyDefinition _iconContentDefinition = new ControlAttachedPropertyDefinition<object>(IconAssist.IconProperty);
    private readonly OptionMetadataRegistry _optionMetadataRegistry = new();
    private readonly ChoiceMetadataRegistry _choiceMetadataRegistry = new();

    #region Fluent API

    #region Kind

    /// <summary>
    /// Sets the kind of the control theme to the specified value.
    /// </summary>
    /// <remarks>This method is part of a fluent API design, allowing for expressive and readable
    /// configuration of control themes.</remarks>
    /// <param name="kind">The kind of the control theme to apply. This value influences the appearance and behavior of the control theme.</param>
    /// <returns>The current instance of the ControlThemeBuilder, enabling method chaining.</returns>
    public ControlThemeBuilder WithKind(string kind)
    {
        _kind = CssClass.Kind(kind);
        return this;
    }

    /// <summary>
    /// Sets the theme kind for the control, allowing customization of its appearance based on the specified theme
    /// option.
    /// </summary>
    /// <remarks>Call this method before building or rendering the control to ensure the desired theme kind is
    /// applied. This method supports a fluent configuration style.</remarks>
    /// <param name="kind">A <see cref="ControlClassOption"/> representing the kind of theme to apply. This parameter determines the visual
    /// style to be used for the control.</param>
    /// <returns>The current instance of <see cref="ControlThemeBuilder"/>, enabling method chaining for additional
    /// configuration.</returns>
    public ControlThemeBuilder WithKind(CssClass kind)
    {
        _kind = kind;
        return this;
    }

    #endregion

    #region Content

    /// <summary>
    /// Sets the default content type to be used by the control theme builder.
    /// </summary>
    /// <remarks>Use this method to specify a default content type that applies to all controls configured by
    /// this builder unless explicitly overridden. Ensure that the provided content type is valid for the intended theme
    /// scenario.</remarks>
    /// <param name="property">The Avalonia property associated with the content definition.</param>
    /// <param name="contentType">The content type to set as the default. This value determines which content provider type will be used for
    /// subsequent theme building operations.</param>
    /// <returns>The current instance of the ControlThemeBuilder to allow for method chaining.</returns>
    public ControlThemeBuilder WithContent(AvaloniaProperty<object> property, ContentProviderType contentType)
    {
        _controlContentDefinition = new(property, contentType);
        return this;
    }

    #endregion

    #region Icon

    /// <summary>
    /// Sets the default content property for the control theme builder, allowing for consistent content configuration across
    /// all controls configured by this builder.
    /// </summary>
    /// <param name="property">The Avalonia property to be used for the icon.</param>
    /// <returns>The current instance of the ControlThemeBuilder to allow for method chaining.</returns>
    public ControlThemeBuilder WithIcon(AvaloniaProperty<object> property)
    {
        _iconContentDefinition = new ControlPropertyDefinition<object>(property);
        return this;
    }

    /// <summary>
    /// Configures the control theme to use the specified attached property as its icon.
    /// </summary>
    /// <remarks>Use this method to customize the appearance of a control by associating an icon via an
    /// attached property. Ensure that the provided property corresponds to a valid icon resource.</remarks>
    /// <param name="property">The attached property that identifies the icon resource to associate with the control theme. Cannot be null.</param>
    /// <param name="prefix">An optional prefix for the theme resource key. If not specified, the default XAML prefix is used.</param>
    /// <returns>The current instance of the ControlThemeBuilder, enabling method chaining.</returns>
    public ControlThemeBuilder WithIcon(AttachedProperty<object> property, string? prefix = ThemeResourceKeyFactory.XamlPrefix)
    {
        _iconContentDefinition = new ControlAttachedPropertyDefinition<object>(property, prefix);
        return this;
    }

    #endregion

    #region Shapes

    /// <summary>
    /// Adds a shape option to the control theme using the specified CSS class and optional metadata configuration.
    /// </summary>
    /// <remarks>Use this method to customize the available shape options for a control theme. Additional
    /// metadata can be supplied to further define the behavior or appearance of the shape option.</remarks>
    /// <param name="class">The CSS class that defines the visual style of the shape option. Cannot be null.</param>
    /// <param name="actionOnBuilder">An optional action to configure additional metadata for the shape option using the provided
    /// ChoiceMetadataBuilder.</param>
    /// <returns>The current ControlThemeBuilder instance, enabling method chaining.</returns>
    public ControlThemeBuilder AddShape(CssClass @class, Action<ChoiceMetadataBuilder>? actionOnBuilder = null) => AddChoice(_shapes, @class, actionOnBuilder);

    /// <summary>
    /// Adds one or more CSS classes to the shapes configuration for the control theme.
    /// </summary>
    /// <remarks>Use this method to apply additional CSS classes to the shapes within the control theme,
    /// allowing for customized styling. Multiple calls to this method will accumulate the specified classes.</remarks>
    /// <param name="classes">An array of <see cref="CssClass"/> objects representing the CSS classes to add to the shapes. This parameter
    /// cannot be null.</param>
    /// <returns>The current <see cref="ControlThemeBuilder"/> instance, enabling method chaining.</returns>
    public ControlThemeBuilder AddShapes(params CssClass[] classes) => AddChoices(_shapes, classes);

    #endregion

    #region Variants

    /// <summary>
    /// Adds a variant option to the control theme using the specified CSS class and optional metadata configuration.
    /// </summary>
    /// <param name="class">The CSS class that defines the visual style of the variant option. Cannot be null.</param>
    /// <param name="actionOnBuilder">An optional action to configure additional metadata for the variant option using the provided
    /// ChoiceMetadataBuilder.</param>
    /// <returns>The current ControlThemeBuilder instance, enabling method chaining.</returns>
    public ControlThemeBuilder AddVariant(CssClass @class, Action<ChoiceMetadataBuilder>? actionOnBuilder = null) => AddChoice(_variants, @class, actionOnBuilder);

    /// <summary>
    /// Adds one or more CSS class variants to the control theme builder.
    /// </summary>
    /// <remarks>Use this method to dynamically add CSS class variants that customize the appearance of the
    /// control theme. Variants can be used to define alternative visual states or styles for controls.</remarks>
    /// <param name="classes">An array of <see cref="CssClass"/> instances representing the CSS classes to add as variants. Cannot be null or
    /// contain null elements.</param>
    /// <returns>The current <see cref="ControlThemeBuilder"/> instance, enabling method chaining.</returns>
    public ControlThemeBuilder AddVariants(params CssClass[] classes) => AddChoices(_variants, classes);

    #endregion

    #region Sizes

    /// <summary>
    /// Adds a size option to the control theme using the specified CSS class and optional metadata configuration.
    /// </summary>
    /// <remarks>Use this method to define size options for controls in a theme, supporting responsive design
    /// and flexible styling.</remarks>
    /// <param name="class">The CSS class that defines the size styling to be applied to the control. Cannot be null.</param>
    /// <param name="actionOnBuilder">An optional action to configure additional metadata for the size option. This allows for further customization
    /// of the size behavior.</param>
    /// <returns>The current instance of the ControlThemeBuilder, enabling method chaining.</returns>
    public ControlThemeBuilder AddSize(CssClass @class, Action<ChoiceMetadataBuilder>? actionOnBuilder = null) => AddChoice(_sizes, @class, actionOnBuilder);

    /// <summary>
    /// Adds one or more CSS classes that define size options for the control theme.
    /// </summary>
    /// <remarks>Use this method to customize the sizing of controls by applying predefined or custom CSS
    /// classes. Ensure that the specified classes are defined in the relevant CSS files to achieve the desired visual
    /// effect.</remarks>
    /// <param name="classes">An array of <see cref="CssClass"/> objects representing the CSS classes to add. Each class specifies a
    /// size-related style to be applied to the control theme. Cannot be null.</param>
    /// <returns>The current <see cref="ControlThemeBuilder"/> instance, enabling method chaining.</returns>
    public ControlThemeBuilder AddSizes(params CssClass[] classes) => AddChoices(_sizes, classes);

    #endregion

    #region Roles

    /// <summary>
    /// Adds a new role to the control theme, enabling customization of the control's appearance and behavior for the
    /// specified role.
    /// </summary>
    /// <remarks>Use this method to dynamically extend the control theme with additional roles, which can be
    /// used to tailor the control's functionality and visual presentation according to different theme
    /// requirements.</remarks>
    /// <param name="role">The theme role to add. Specifies the style or behavior to be associated with the control.</param>
    /// <param name="actionOnBuilder">An optional action to configure additional metadata for the role being added. This allows further customization
    /// of the role's settings.</param>
    /// <returns>The current instance of the ControlThemeBuilder, allowing for method chaining.</returns>
    public ControlThemeBuilder AddRole(ThemeRole role, Action<ChoiceMetadataBuilder>? actionOnBuilder = null) => AddChoice(_roles, role, actionOnBuilder);

    /// <summary>
    /// Adds one or more theme roles to the control theme builder.
    /// </summary>
    /// <remarks>This method can be called multiple times to add different roles. Ensure that the roles
    /// provided are valid and applicable to the current theme context.</remarks>
    /// <param name="roles">An array of theme roles to add. Each role influences the styling and behavior of the control. Cannot be null or
    /// contain null elements.</param>
    /// <returns>The current instance of the ControlThemeBuilder, enabling method chaining.</returns>
    public ControlThemeBuilder AddRoles(params ThemeRole[] roles) => AddChoices(_roles, roles);

    /// <summary>
    /// Adds a theme role to the collection of item roles, optionally allowing customization of the role's metadata.
    /// </summary>
    /// <remarks>Use this method to associate additional roles with items in the theme and to customize their
    /// metadata as needed. This enhances the flexibility of theme configuration.</remarks>
    /// <param name="role">The theme role to add to the collection. Specifies the role that items will assume within the theme.</param>
    /// <param name="actionOnBuilder">An optional action that configures the metadata for the role being added. If provided, this action receives a
    /// builder for customizing the role's properties.</param>
    /// <returns>The current instance of the ControlThemeBuilder, enabling method chaining.</returns>
    public ControlThemeBuilder AddItemsRole(ThemeRole role, Action<ChoiceMetadataBuilder>? actionOnBuilder = null) => AddChoice(_itemsRoles, role, actionOnBuilder);

    /// <summary>
    /// Adds multiple theme roles to the collection of item roles, allowing for optional metadata configuration for each role.
    /// </summary>
    /// <param name="roles">An array of theme roles to add to the collection. Each role specifies the role that items will assume within the theme. Cannot be null or contain null elements.</param>
    /// <returns>The current instance of the ControlThemeBuilder, enabling method chaining.</returns>
    public ControlThemeBuilder AddItemsRoles(params ThemeRole[] roles) => AddChoices(_itemsRoles, roles);

    #endregion

    #region Choices

    /// <summary>
    /// Adds a single choice to the specified list and registers its metadata if provided. This method ensures that duplicate choices are not added to the list.
    /// </summary>
    /// <typeparam name="T">The type of the choice.</typeparam>
    /// <param name="list">The list to which the choice will be added.</param>
    /// <param name="choice">The choice to add.</param>
    /// <param name="configure">An optional action to configure the metadata for the choice.</param>
    /// <returns>The current instance of the ControlThemeBuilder, enabling method chaining.</returns>
    private ControlThemeBuilder AddChoice<T>(ICollection<T> list, T choice, Action<ChoiceMetadataBuilder>? configure = null) => AddChoices(list, [choice], configure is not null ? new Action<T, ChoiceMetadataBuilder>((_, builder) => configure(builder)) : null);

    /// <summary>
    /// Adds the specified choices to the provided collection, optionally registering associated metadata for each new
    /// choice.
    /// </summary>
    /// <remarks>If a choice already exists in the list, it is not added again and no metadata is registered
    /// for it. Use the configure parameter to customize metadata for each choice before registration.</remarks>
    /// <typeparam name="T">The type of elements contained in the collections.</typeparam>
    /// <param name="list">The collection to which choices will be added if they are not already present.</param>
    /// <param name="choices">The collection of choices to add to the list.</param>
    /// <param name="configure">An optional action that configures metadata for each choice as it is added. If provided, this action is invoked
    /// for each new choice.</param>
    /// <returns>The current instance of the ControlThemeBuilder, enabling method chaining.</returns>
    private ControlThemeBuilder AddChoices<T>(ICollection<T> list, ICollection<T> choices, Action<T, ChoiceMetadataBuilder>? configure = null)
    {
        foreach (var option in choices)
        {
            if (!list.Contains(option))
            {
                list.Add(option);
                if (configure is not null)
                {
                    var builder = new ChoiceMetadataBuilder();
                    configure(option, builder);

                    var metadata = builder.Build();
                    _choiceMetadataRegistry.Register(option, metadata);
                }
            }
        }

        return this;
    }

    #endregion

    #region Options

    /// <summary>
    /// Adds a CSS class option to the control theme, allowing for dynamic application of the class based on user interaction or other conditions.
    /// </summary>
    /// <param name="defaultValue">The default CSS class to apply. This parameter is optional and can be null.</param>
    /// <param name="configure">An optional action to configure the metadata for the CSS class option. This allows for further customization of the option's behavior.</param>
    /// <param name="onValueChanged">An optional action to execute when the value of the CSS class option changes. This allows for dynamic behavior based on the option's state.</param>
    /// <returns>The current instance of the ControlThemeBuilder, allowing for method chaining.</returns>
    public ControlThemeBuilder AddClass(CssClass? defaultValue = null, Action<OptionMetadataBuilder>? configure = null, Action<Control, object?>? onValueChanged = null)
        => AddOption(new ControlClassDefinition(defaultValue, onValueChanged), configure);

    /// <summary>
    /// Adds a toggle option for a specific CSS class to the control theme, allowing for dynamic application of the class based on user interaction or other conditions.
    /// </summary>
    /// <param name="class">The CSS class to toggle. This parameter cannot be null.</param>
    /// <param name="defaultValue">The default value indicating whether the CSS class is initially applied. Defaults to false.</param>
    /// <param name="configure">An optional action to configure the metadata for the toggle option. This allows for further customization of the option's behavior.</param>
    /// <param name="onValueChanged">An optional action to execute when the value of the toggle changes. This allows for dynamic behavior based on the toggle state.</param>
    /// <returns>The current instance of the ControlThemeBuilder, allowing for method chaining.</returns>
    public ControlThemeBuilder AddClassToggle(CssClass @class, bool defaultValue = false, Action<OptionMetadataBuilder>? configure = null, Action<Control, object?>? onValueChanged = null)
        => AddOption(new ControlClassToggleDefinition(@class, defaultValue, onValueChanged), configure);

    /// <summary>
    /// Adds a custom action to the control theme builder, enabling dynamic modification of control behavior at runtime.
    /// </summary>
    /// <remarks>Use this method to inject custom logic or behaviors into a control's theme. This is useful
    /// for scenarios where standard theme settings are insufficient and runtime customization is required.</remarks>
    /// <param name="action">The action to execute on the control. This delegate defines the behavior to apply when the theme is built.
    /// Cannot be null.</param>
    /// <param name="configure">An optional delegate to configure the metadata for the custom action. Use this to further customize how the
    /// action is described or applied.</param>
    /// <returns>The current instance of the ControlThemeBuilder, allowing for method chaining.</returns>
    public ControlThemeBuilder AddAction<T>(Action<T> action, Action<OptionMetadataBuilder>? configure = null)
        where T : Control
        => AddOption(new ControlActionDefinition(x => action((T)x)), configure);

    /// <summary>
    /// Adds an action to the control theme builder that is invoked when the value of the control changes.
    /// </summary>
    /// <param name="onValueChanged">The action to execute when the control's value changes. The action receives the control and the new value as
    /// parameters.</param>
    /// <param name="defaultValue">The default value to use when the action is executed without a specific value.</param>
    /// <param name="configure">An optional action to configure the metadata for the option associated with the value action.</param>
    /// <returns>The current instance of the ControlThemeBuilder, enabling method chaining.</returns>
    public ControlThemeBuilder AddValueAction(Action<Control, object?> onValueChanged, object? defaultValue = null, Action<OptionMetadataBuilder>? configure = null)
        => AddOption(new ControlValueActionDefinition(onValueChanged, defaultValue), configure);

    /// <summary>
    /// Adds a custom property definition to the control theme builder, allowing the theme to support additional
    /// properties for controls.
    /// </summary>
    /// <remarks>Use this method to extend a control theme with custom properties, enabling further
    /// customization of control appearance and behavior through the theme system.</remarks>
    /// <typeparam name="T">The type of the property being added to the control theme.</typeparam>
    /// <param name="property">The Avalonia property to associate with the custom property definition. Cannot be null.</param>
    /// <param name="defaultValue">The default value to use for the property if no value is provided. If omitted, the property's default is used.</param>
    /// <param name="configure">An optional action to configure the metadata for the custom property. This can be used to specify additional
    /// options or behaviors.</param>
    /// <param name="onValueChanged">An optional action to execute when the value of the toggle changes. This allows for dynamic behavior based on the toggle state.</param>
    /// <returns>The current instance of the ControlThemeBuilder, enabling method chaining.</returns>
    public ControlThemeBuilder AddProperty<T>(AvaloniaProperty<T> property, T? defaultValue = default, Action<OptionMetadataBuilder>? configure = null, Action<Control, object?>? onValueChanged = null)
        => AddOption(new ControlPropertyDefinition<T>(property, defaultValue, onValueChanged), configure);

    /// <summary>
    /// Adds a custom control option to the theme builder and optionally configures its associated metadata.
    /// </summary>
    /// <remarks>Use this method to extend the control theme with additional options and metadata, allowing
    /// for greater customization of control appearance and behavior.</remarks>
    /// <param name="option">The control option definition to add as a custom option. Cannot be null.</param>
    /// <param name="configure">An optional action that configures metadata for the custom option. If provided, receives an <see
    /// cref="OptionMetadataBuilder"/> instance to customize the option's metadata.</param>
    /// <returns>The current <see cref="ControlThemeBuilder"/> instance, enabling method chaining.</returns>
    public ControlThemeBuilder AddOption(IControlOptionDefinition option, Action<OptionMetadataBuilder>? configure = null)
    {
        _customOptions.Add(option);

        if (configure is not null)
        {
            var builder = new OptionMetadataBuilder();
            configure(builder);

            var metadata = builder.Build();
            _optionMetadataRegistry.Register(option, metadata);
            _choiceMetadataRegistry.Merge(builder.BuildChoiceMetadata());
        }

        return this;
    }

    #endregion

    #endregion

    #region Build

    /// <summary>
    /// Builds the control theme definition with all configured settings.
    /// </summary>
    /// <param name="controlName">The name of the control.</param>
    /// <returns>The constructed control theme definition.</returns>
    public ControlThemeDefinition Build(string controlName)
    {
        var fullKey = ResolveKey(controlName, themeKey);
        var theme = ResolveTheme(fullKey);

        return new(theme, fullKey)
        {
            Kind = _kind,
            ContentDefinition = _controlContentDefinition,
            IconDefinition = _iconContentDefinition,
            Variants = _variants,
            Roles = _roles,
            ItemsRoles = _itemsRoles,
            Sizes = _sizes,
            Shapes = _shapes,
            CustomSettings = _customOptions
        };
    }

    /// <summary>
    /// Builds the registry containing metadata for all options defined in this builder. This registry can be used to
    /// retrieve metadata for each option when needed.
    /// </summary>
    /// <returns>The current instance of the <see cref="OptionMetadataRegistry"/> that holds metadata for options.</returns>
    public OptionMetadataRegistry BuildOptionMetadata() => _optionMetadataRegistry;

    /// <summary>
    /// Gets the registry that contains metadata for available choices and their configuration options.
    /// </summary>
    /// <returns>The current instance of the <see cref="ChoiceMetadataRegistry"/> that holds metadata for choices.</returns>
    public ChoiceMetadataRegistry BuildChoiceMetadata() => _choiceMetadataRegistry;

    /// <summary>
    /// Retrieves the metadata associated with the specified control, applying the appropriate theme if available.
    /// </summary>
    /// <remarks>If the theme key is not set, the default theme resources are used. This method ensures that
    /// the correct theme is applied based on the control name and theme key.</remarks>
    /// <param name="controlName">The name of the control for which to retrieve metadata. This value determines which theming resources are
    /// applied.</param>
    /// <returns>A ChoiceMetadata instance containing the metadata for the specified control, with theming applied as
    /// appropriate.</returns>
    public ChoiceMetadata GetThemeDisplayName(string controlName) => new(new LocalizedString(!string.IsNullOrEmpty(themeKey) || _kind is not null ? $"Theme{controlName}{themeKey?.Replace(".", string.Empty, StringComparison.OrdinalIgnoreCase)}{_kind?.Name.ToTitleCase()}" : nameof(ControlThemeResources.ThemeDefault)));

    #endregion

    #region Theme resolution

    /// <summary>
    /// Resolves the theme resource key based on control name and theme name.
    /// </summary>
    /// <param name="control">The control name.</param>
    /// <param name="themeName">The theme name.</param>
    /// <returns>The resolved theme resource key, or null if no theme name is provided.</returns>
    private static string? ResolveKey(string control, string? themeName) => string.IsNullOrEmpty(themeName) ? null : ThemeResourceKeyFactory.Theme(control, themeName);

    /// <summary>
    /// Resolves and caches the control theme from the theme resource key.
    /// </summary>
    /// <param name="themeKey">The theme resource key.</param>
    /// <returns>The resolved control theme, or null if not found.</returns>
    private static ControlTheme? ResolveTheme(string? themeKey)
    {
        if (string.IsNullOrEmpty(themeKey))
            return null;

        EnsureThemeCacheInvalidation();

        if (ThemeCache.TryGetValue(themeKey, out var cached))
            return cached;

        if (Application.Current?.TryGetResource(themeKey, null, out var value) == true && value is ControlTheme theme)
        {
            ThemeCache[themeKey] = theme;
            return theme;
        }

        return null;
    }

    private static void EnsureThemeCacheInvalidation()
    {
        if (Interlocked.CompareExchange(ref _themeChangedSubscribed, 1, 0) != 0)
            return;

        if (Application.Current?.Styles.OfType<MyTheme>().FirstOrDefault() is not { } theme)
            return;

        theme.ThemeChanged += (_, _) => ThemeCache.Clear();
    }

    #endregion
}
