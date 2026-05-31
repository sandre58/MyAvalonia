// -----------------------------------------------------------------------
// <copyright file="VariantClassRegistry.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Theme.Assists;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.Avalonia.Theme.Classes.Registry.States;
using MyNet.Avalonia.Theme.MarkupExtensions;
using MyNet.Avalonia.Theme.Theming.Core;

namespace MyNet.Avalonia.Theme.Classes.Registry;

/// <summary>
/// Registers and applies "variant" CSS-like classes to Avalonia controls.
/// This registry maps ControlVariant values (such as "Outlined", "Light",
/// "Solid", "Text", etc.) to visual properties on controls, headers and
/// item containers (background, foreground, border, thickness, hover/active
/// states, ripple color). It contains resolution logic to choose the
/// appropriate resources or markup extensions depending on the variant,
/// the control category and theme role.
/// </summary>
public static class VariantClassRegistry
{
    #region State

    private static readonly object InheritForeground = new ForegroundExtension { AncestorType = typeof(Control) };
    private static readonly object Foreground = new ForegroundExtension();
    private static readonly object OverlayForeground = new ForegroundExtension { Opacity = Opacity.Overlay };
    private static readonly object HighForeground = new ForegroundExtension { Opacity = Opacity.High };
    private static readonly object HoverForeground = new ForegroundExtension { Opacity = Opacity.Hover };
    private static readonly object FocusForeground = new ForegroundExtension { Opacity = Opacity.Focus };
    private static readonly object HeaderForeground = Brush("(my:HeaderAssist.Foreground)");
    private static readonly object HeaderHighForeground = Brush("(my:HeaderAssist.Foreground)", Opacity.High);
    private static readonly object HeaderHoverForeground = Brush("(my:HeaderAssist.Foreground)", Opacity.Hover);
    private static readonly object HeaderFocusForeground = Brush("(my:HeaderAssist.Foreground)", Opacity.Focus);
    private static readonly object SurfaceContext = Context("Surface.Level3");
    private static readonly object ControlContext = Context("Surface.Level4");
    private static readonly object ControlBorderContext = Context("Control.Border");
    private static readonly object BackgroundRole = Role(VariantBrush.Background);
    private static readonly object OverlayBackgroundRole = Role(VariantBrush.Background, Opacity.Overlay);
    private static readonly object BorderBrushRole = Role(VariantBrush.BorderBrush);
    private static readonly object ForegroundRole = Role(VariantBrush.Foreground);
    private static readonly object ContrastedForegroundRole = Role(VariantBrush.Foreground, contrast: true);
    private static readonly object PrimaryItemsRole = Role(VariantBrush.Primary, role: "(my:ItemsAssist.Role)");
    private static readonly object FocusPrimaryItemsRole = Role(VariantBrush.Primary, Opacity.Focus, role: "(my:ItemsAssist.Role)");
    private static readonly object ContrastedActiveBackground = Brush("(my:ItemsAssist.ActiveBackground)", contrast: true);
    private static readonly object OverlayItemsForeground = Brush("(my:ItemsAssist.Foreground)", Opacity.Overlay);
    private static readonly object HoverItemsForeground = Brush("(my:ItemsAssist.Foreground)", Opacity.Hover);
    private static readonly object ItemsForeground = Brush("(my:ItemsAssist.Foreground)");

    /// <summary>
    /// Creates a new ThemeRoleExtension instance configured with the specified brush, opacity, contrast, and role
    /// settings.
    /// </summary>
    /// <remarks>Use this method to customize the appearance of a ThemeRoleExtension by specifying opacity and
    /// contrast options as needed.</remarks>
    /// <param name="variantBrush">The VariantBrush to apply to the theme role extension. Cannot be null.</param>
    /// <param name="opacity">An optional opacity value to apply to the theme role extension. If not specified, the default opacity is used.</param>
    /// <param name="contrast">A value indicating whether to enable contrast settings for the theme role extension. The default is <see
    /// langword="false"/>.</param>
    /// <param name="role">The role identifier to assign to the theme assist. The default is "(my:ThemeAssist.Role)".</param>
    /// <returns>A ThemeRoleExtension instance initialized with the provided parameters.</returns>
    private static ThemeRoleExtension Role(VariantBrush variantBrush, Opacity? opacity = null, bool contrast = false, string role = "(my:ThemeAssist.Role)") => new(variantBrush) { Opacity = opacity, Contrast = contrast, Role = role };

    /// <summary>
    /// Creates a new instance of the ThemeContextExtension class using the specified key.
    /// </summary>
    /// <param name="key">The key used to initialize the ThemeContextExtension. This parameter must not be null or empty.</param>
    /// <returns>A new ThemeContextExtension instance initialized with the provided key.</returns>
    private static ThemeContextExtension Context(string key) => new(key);

    /// <summary>
    /// Creates a new ThemeBrushExtension instance using the specified theme brush key, with optional opacity and
    /// contrast settings.
    /// </summary>
    /// <remarks>Use this method to customize the appearance of a theme brush by specifying opacity and
    /// contrast options as needed.</remarks>
    /// <param name="key">The key that identifies the theme brush to create. Cannot be null.</param>
    /// <param name="opacity">An optional opacity value to apply to the brush. If not specified, the default opacity is used.</param>
    /// <param name="contrast">A value indicating whether to apply contrast adjustments to the brush. The default is <see langword="false"/>.</param>
    /// <returns>A ThemeBrushExtension configured with the specified key, opacity, and contrast settings.</returns>
    private static ThemeBrushExtension Brush(string key, Opacity? opacity = null, bool contrast = false) => new(key) { Opacity = opacity, Contrast = contrast };

    /// <summary>
    /// Holds the currently applied variants for a single control.
    /// The registry allows multiple variant classes to be present at once
    /// (for example a control may have both a "Light" and an "Outlined"
    /// variant class). This internal state stores the sets of variants and
    /// exposes combined bitwise results via the computed properties.
    /// </summary>
    private sealed class ControlState
    {
        /// <summary>
        /// Gets the collection of control variants associated with this instance.
        /// </summary>
        /// <remarks>The collection is initialized as an empty set and can be modified to include various
        /// control variants as needed.</remarks>
        public HashSet<ControlVariant> Variants { get; } = [];

        /// <summary>
        /// Gets combined variant for the control body (bitwise OR of all variants).
        /// </summary>
        public ControlVariant Variant => Variants.Aggregate(ControlVariant.None, (a, b) => a | b);

        /// <summary>
        /// Gets the collection of control variants associated with the control header for this instance.
        /// </summary>
        public HashSet<ControlVariant> HeaderVariants { get; } = [];

        /// <summary>
        /// Gets combined variant applied to the control header.
        /// </summary>
        public ControlVariant HeaderVariant => HeaderVariants.Aggregate(ControlVariant.None, (a, b) => a | b);

        /// <summary>
        /// Gets the collection of control variants associated with this instance.
        /// </summary>
        /// <remarks>The collection is initialized as an empty set and can be modified by adding or
        /// removing control variants as needed.</remarks>
        public HashSet<ControlVariant> ItemsVariants { get; } = [];

        /// <summary>
        /// Gets combined variant applied to item containers (list items, menu items...).
        /// </summary>
        public ControlVariant ItemsVariant => ItemsVariants.Aggregate(ControlVariant.None, (a, b) => a | b);

        /// <summary>
        /// Gets tracks all SetProperty bindings for the variant section so they can be properly disposed on variant change.
        /// </summary>
        public BindingGroup VariantBindings { get; } = new();

        /// <summary>
        /// Gets tracks all SetProperty bindings for the header variant section.
        /// </summary>
        public BindingGroup HeaderBindings { get; } = new();

        /// <summary>
        /// Gets tracks all SetProperty bindings for the items variant section.
        /// </summary>
        public BindingGroup ItemsBindings { get; } = new();

        /// <summary>
        /// Gets tracks all SetProperty bindings for the fallback properties set in ApplyState.
        /// </summary>
        public BindingGroup FallbackBindings { get; } = new();
    }

    /// <summary>
    /// Applies the specified control state variants to the given control, modifying its appearance based on the
    /// provided state.
    /// </summary>
    /// <remarks>If no variant is set for the control body but a header variant exists, the method sets the
    /// control's background to transparent, removes the border, and inherits the foreground color to ensure the header
    /// styling is visible.</remarks>
    /// <param name="control">The control to which the state variants will be applied, influencing its visual representation.</param>
    /// <param name="state">The state object containing the variants to apply, including the control variant, header variant, and items
    /// variant.</param>
    private static void ApplyState(Control control, ControlState state)
    {
        ApplyVariant(control, state);
        ApplyHeaderVariant(control, state);
        ApplyItemsVariant(control, state);

        state.FallbackBindings.Reset();

        if (state.Variant == ControlVariant.None && state.HeaderVariant != ControlVariant.None)
        {
            state.FallbackBindings.Add(control.SetProperty(VariantAssist.BackgroundProperty, Brushes.Transparent));
            state.FallbackBindings.Add(control.SetProperty(VariantAssist.BorderThicknessProperty, new Thickness(0)));
            state.FallbackBindings.Add(control.SetProperty(VariantAssist.ForegroundProperty, InheritForeground));
        }
    }

    /// <summary>
    /// Applies the specified visual variant to the given control, updating its appearance according to the variant's
    /// characteristics.
    /// </summary>
    /// <remarks>When the default or no variant is specified, this method clears any previously set visual
    /// properties, allowing the control to use its default styling. For other variants, the method updates background,
    /// border, and foreground properties based on the control's category and theme role. If the transparent variant is
    /// applied, the header's appearance is also updated accordingly.</remarks>
    /// <param name="control">The control to which the visual variant will be applied. This parameter cannot be null.</param>
    /// <param name="state">The state object associated with the control, containing the current variant and bindings.</param>
    private static void ApplyVariant(Control control, ControlState state)
    {
        state.VariantBindings.Reset();

        var variant = state.Variant;

        if (variant is ControlVariant.None or ControlVariant.Default)
            return;

        var category = ThemeAssist.GetCategory(control);
        var hasRole = ThemeAssist.GetHasRole(control);
        var role = ThemeAssist.GetRole(control);

        var background = ResolveBackground(control, variant, category, role);
        var borderBrush = ResolveBorderBrush(variant, category, role);
        var borderThickness = ResolveBorderThickness(control, variant);
        var foreground = ResolveForeground(control, variant, category, hasRole, false);
        var headerForeground = ResolveForeground(control, variant, category, hasRole, true);

        state.VariantBindings.Add(control.SetProperty(VariantAssist.BackgroundProperty, background));
        state.VariantBindings.Add(control.SetProperty(VariantAssist.BorderBrushProperty, borderBrush));
        state.VariantBindings.Add(control.SetProperty(VariantAssist.BorderThicknessProperty, new Thickness(borderThickness)));
        state.VariantBindings.Add(control.SetProperty(VariantAssist.ForegroundProperty, foreground));

        if (variant == ControlVariant.Transparent)
            SetHeaderProperties(control, ControlVariant.Transparent, state.VariantBindings);
        else
            state.VariantBindings.Add(control.SetProperty(HeaderAssist.ForegroundProperty, headerForeground));

        // Specific controls
        if (category == ControlCategory.Input)
        {
            if (variant.HasFlag(ControlVariant.Outlined))
            {
                state.VariantBindings.Add(control.SetProperty(InteractionAssist.HoverBackgroundProperty, Brushes.Transparent));
                state.VariantBindings.Add(control.SetProperty(InteractionAssist.ActiveBackgroundProperty, Brushes.Transparent));
            }

            if (role == ThemeRole.Contrast)
                state.VariantBindings.Add(control.SetProperty(InteractionAssist.HoverBorderBrushProperty, HoverForeground));
        }
    }

    /// <summary>
    /// Applies the specified header variant to the given control, updating its visual properties accordingly.
    /// </summary>
    /// <remarks>This method resolves the appropriate background, foreground, border brush, and border
    /// thickness based on the provided header variant and the control's category and role. It is important to note that
    /// if the header variant is None, all header-specific properties will be cleared.</remarks>
    /// <param name="control">The control to which the header variant will be applied. This control's visual properties will be modified based
    /// on the specified variant.</param>
    /// <param name="state">The state object associated with the control, containing the current header variant and bindings.</param>
    private static void ApplyHeaderVariant(Control control, ControlState state)
    {
        state.HeaderBindings.Reset();

        var headerVariant = state.HeaderVariant;

        if (headerVariant == ControlVariant.None)
            return;

        SetHeaderProperties(control, headerVariant, state.HeaderBindings);
    }

    /// <summary>
    /// Sets header visual properties on the control for the given variant, adding the resulting disposables to the
    /// provided composite.
    /// </summary>
    private static void SetHeaderProperties(Control control, ControlVariant headerVariant, BindingGroup bindings)
    {
        var category = ThemeAssist.GetCategory(control);
        var hasRole = ThemeAssist.GetHasRole(control);
        var role = ThemeAssist.GetRole(control);

        var background = ResolveBackground(control, headerVariant, category, role);
        var foreground = ResolveForeground(control, headerVariant, category, hasRole, true);
        var borderBrush = ResolveBorderBrush(headerVariant, category, role);
        var borderThickness = ResolveBorderThickness(control, headerVariant);

        bindings.Add(control.SetProperty(HeaderAssist.BackgroundProperty, background));
        bindings.Add(control.SetProperty(HeaderAssist.BorderBrushProperty, borderBrush));
        bindings.Add(control.SetProperty(HeaderAssist.BorderThicknessProperty, new Thickness(borderThickness)));
        bindings.Add(control.SetProperty(HeaderAssist.ForegroundProperty, foreground));
    }

    /// <summary>
    /// Applies the specified visual variant to the given control, updating its appearance properties based on the
    /// variant.
    /// </summary>
    /// <remarks>This method modifies various properties of the control, including background, foreground,
    /// border thickness, and hover effects, based on the provided variant. It is important to ensure that the control
    /// is valid and properly initialized before applying a variant.</remarks>
    /// <param name="control">The control to which the visual variant will be applied. This control's appearance will be modified according to
    /// the specified variant.</param>
    /// <param name="state">The state object associated with the control, containing the current variant and bindings.</param>
    private static void ApplyItemsVariant(Control control, ControlState state)
    {
        state.ItemsBindings.Reset();

        var variant = state.ItemsVariant;

        if (variant == ControlVariant.None)
            return;

        var role = ItemsAssist.GetRole(control);

        var background = ResolveItemsBackground(variant);
        var foreground = ResolveItemsForeground(control, variant);
        var borderBrush = ResolveItemsBorderBrush();
        var borderThickness = ResolveItemsBorderThickness(variant);
        var hoverBackground = ResolveItemsHoverBackground(control, variant);
        var hoverForeground = ResolveItemsHoverForeground(control, variant);
        var activeBackground = ResolveItemsActiveBackground(control, variant, role);
        var activeForeground = ResolveItemsActiveForeground(control, variant, role);
        var activeBorderBrush = ResolveItemsActiveBorderBrush(control, variant, role);
        var rippleColor = ResolveItemsRippleColor(control, variant, role);

        state.ItemsBindings.Add(control.SetProperty(ItemsAssist.BackgroundProperty, background));
        state.ItemsBindings.Add(control.SetProperty(ItemsAssist.BorderBrushProperty, borderBrush));
        state.ItemsBindings.Add(control.SetProperty(ItemsAssist.BorderThicknessProperty, new Thickness(borderThickness)));
        state.ItemsBindings.Add(control.SetProperty(ItemsAssist.ForegroundProperty, foreground));
        state.ItemsBindings.Add(control.SetProperty(ItemsAssist.HoverBackgroundProperty, hoverBackground));
        state.ItemsBindings.Add(control.SetProperty(ItemsAssist.HoverForegroundProperty, hoverForeground));
        state.ItemsBindings.Add(control.SetProperty(ItemsAssist.ActiveBackgroundProperty, activeBackground));
        state.ItemsBindings.Add(control.SetProperty(ItemsAssist.ActiveForegroundProperty, activeForeground));
        state.ItemsBindings.Add(control.SetProperty(ItemsAssist.ActiveBorderBrushProperty, activeBorderBrush));
        state.ItemsBindings.Add(control.SetProperty(ItemsAssist.RippleColorProperty, rippleColor));
    }

    /// <summary>
    /// Resolves the appropriate background source for the control or header based on the provided variant, control category,
    /// and other contextual factors.
    /// </summary>
    /// <param name="control">The control for which to resolve the background.</param>
    /// <param name="variant">The visual variant of the control.</param>
    /// <param name="category">The category of the control.</param>
    /// <param name="role">The theme role of the control.</param>
    /// <returns>The resolved background source.</returns>
    private static object ResolveBackground(Control control, ControlVariant variant, ControlCategory category, ThemeRole role) =>
        variant.HasFlag(ControlVariant.Light)
            ? OverlayBackgroundRole
            : variant.HasFlag(ControlVariant.Solid) || variant.HasFlag(ControlVariant.Default)
                ? category == ControlCategory.Input && role == ThemeRole.Contrast ? OverlayForeground
                : category == ControlCategory.Input ? ControlContext
                : variant.HasFlag(ControlVariant.Outlined) || control is ProgressBar or Slider ? category is ControlCategory.Surface or ControlCategory.Navigation ? SurfaceContext : ControlContext
                : BackgroundRole
                : Brushes.Transparent;

    /// <summary>
    /// Determines the appropriate border brush to use based on the specified control variant.
    /// </summary>
    /// <remarks>Use this method to select a border brush that matches the visual style indicated by the
    /// control's variant. This is typically used when applying visual styles to controls that support multiple
    /// appearance options.</remarks>
    /// <param name="variant">A value that specifies the visual variant of the control. Must be a valid member of the ControlVariant
    /// enumeration.</param>
    /// <param name="category">The category of the control.</param>
    /// <param name="role">The theme role of the control.</param>
    /// <returns>Returns the border brush to apply. If the variant includes the Outlined flag, returns the BorderBrushRole;
    /// otherwise, returns Brushes.Transparent.</returns>
    private static object ResolveBorderBrush(ControlVariant variant, ControlCategory category, ThemeRole role)
        => variant.HasFlag(ControlVariant.Outlined) ? category == ControlCategory.Input && role == ThemeRole.Contrast ? FocusForeground
            : category == ControlCategory.Input ? ControlBorderContext
            : BorderBrushRole
            : Brushes.Transparent;

    /// <summary>
    /// Determines the appropriate border thickness for a control based on its visual variant.
    /// </summary>
    /// <param name="control">The control for which the border thickness is being resolved. The type of control may affect the resulting
    /// thickness.</param>
    /// <param name="variant">The visual variant that specifies the style of the control. If the variant includes the outlined flag, a nonzero
    /// border thickness is returned.</param>
    /// <returns>A double value representing the border thickness to apply. Returns 0.4 for outlined controls of type
    /// ExtendedIcon, 1 for other outlined controls, and 0 for controls that are not outlined.</returns>
    private static double ResolveBorderThickness(Control control, ControlVariant variant) => variant.HasFlag(ControlVariant.Outlined) ? control is ExtendedIcon ? 0.4 : 1 : 0;

    /// <summary>
    /// Resolves the appropriate foreground role for a control based on its visual variant, role association, and header
    /// status.
    /// </summary>
    /// <remarks>This method evaluates the control's variant and role to determine the most suitable
    /// foreground role. For controls with the Solid or Default variant, a contrasted foreground is used. If the control
    /// uses the Text variant and has an associated role, the foreground role is applied, with special handling for
    /// headered controls. In all other cases, the foreground is inherited.</remarks>
    /// <param name="control">The control for which the foreground role is being determined. Cannot be null.</param>
    /// <param name="variant">The visual variant of the control that influences the selection of the foreground role.</param>
    /// <param name="category">The category of the control.</param>
    /// <param name="hasRole">true if the control has an associated role that may affect foreground determination; otherwise, false.</param>
    /// <param name="isHeader">true if the control is a header or should be treated as one; otherwise, false.</param>
    /// <returns>An object representing the resolved foreground role for the specified control and conditions.</returns>
    private static object ResolveForeground(Control control, ControlVariant variant, ControlCategory category, bool hasRole, bool isHeader)
        => category == ControlCategory.Input ? InheritForeground
           : variant.HasFlag(ControlVariant.Text) && hasRole && (control is not HeaderedContentControl || isHeader) ? ForegroundRole
           : variant.HasFlag(ControlVariant.Light) || variant.HasFlag(ControlVariant.Outlined) ? InheritForeground
           : variant.HasFlag(ControlVariant.Solid) || variant.HasFlag(ControlVariant.Default) ? ContrastedForegroundRole
           : InheritForeground;

    /// <summary>
    /// Resolves the appropriate brush to use for overlay items based on the specified control variant.
    /// </summary>
    /// <remarks>Use this method to dynamically select the overlay item brush according to the control's
    /// visual state, enabling flexible UI rendering for different control variants.</remarks>
    /// <param name="variant">The control variant that determines which brush is selected. Must be a valid value from the ControlVariant
    /// enumeration.</param>
    /// <returns>An object representing the brush to use for overlay items. Returns OverlayItemsForeground if the variant
    /// includes the Solid flag; otherwise, returns Brushes.Transparent.</returns>
    private static object ResolveItemsBackground(ControlVariant variant)
        => variant.HasFlag(ControlVariant.Solid) ? OverlayItemsForeground : Brushes.Transparent;

    /// <summary>
    /// Determines the border thickness for items based on the specified control variant.
    /// </summary>
    /// <param name="variant">The control variant that influences the border thickness. If the variant includes the Outlined flag, a thickness
    /// of 1.4 is applied; otherwise, the thickness is 0.</param>
    /// <returns>A double representing the border thickness for the items, which is 1.4 if the Outlined variant is specified, and
    /// 0 otherwise.</returns>
    private static double ResolveItemsBorderThickness(ControlVariant variant)
        => variant.HasFlag(ControlVariant.Outlined) ? 1.4 : 0;

    /// <summary>
    /// Gets the border brush used for items, which is transparent by default.
    /// </summary>
    /// <returns>An object representing the border brush for items, specifically a transparent brush.</returns>
    private static object ResolveItemsBorderBrush() => Brushes.Transparent;

    /// <summary>
    /// Determines the appropriate foreground color based on the specified control variant.
    /// </summary>
    /// <remarks>This method evaluates the ControlVariant flags to decide which foreground color to use,
    /// ensuring that the correct visual representation is applied based on the variant's state.</remarks>
    /// <param name="control">The specified control.</param>
    /// <param name="variant">The control variant that influences the foreground color selection. Must be a valid value from the
    /// ControlVariant enumeration.</param>
    /// <returns>An object representing the selected foreground color. Returns HighForeground if the Text variant is included;
    /// otherwise, returns InheritForeground.</returns>
    private static object ResolveItemsForeground(Control control, ControlVariant variant)
        => variant.HasFlag(ControlVariant.Text) ? GetHighForeground(control) : GetForeground(control);

    /// <summary>
    /// Determines the background brush to use for hover items based on the specified control variant.
    /// </summary>
    /// <remarks>The returned brush depends on the flags set in the provided control variant. This method
    /// enables consistent theming for hover states across different control styles.</remarks>
    /// <param name="control">The specified control.</param>
    /// <param name="variant">The control variant that influences the selection of the hover background brush.</param>
    /// <returns>A brush object representing the background for hover items. Returns a specific foreground brush for 'Solid',
    /// 'Light', or 'Outlined' variants; otherwise, returns a transparent brush.</returns>
    private static object ResolveItemsHoverBackground(Control control, ControlVariant variant)
        => variant.HasFlag(ControlVariant.Solid) ? HoverItemsForeground
            : variant.HasFlag(ControlVariant.Light) || variant.HasFlag(ControlVariant.Outlined) ? GetHoverForeground(control)
            : Brushes.Transparent;

    /// <summary>
    /// Determines the foreground color to use when an item is hovered, based on the specified control variant.
    /// </summary>
    /// <remarks>If the specified variant includes the Solid flag, the method returns the ItemsForeground
    /// color; otherwise, it returns the default Foreground color.</remarks>
    /// <param name="control">The specified control.</param>
    /// <param name="variant">The control variant that influences the selection of the hover foreground color. Must be a valid value from the
    /// ControlVariant enumeration.</param>
    /// <returns>An object representing the foreground color to apply when an item is hovered, depending on the control variant.</returns>
    private static object ResolveItemsHoverForeground(Control control, ControlVariant variant)
        => variant.HasFlag(ControlVariant.Solid) ? ItemsForeground : GetForeground(control);

    /// <summary>
    /// Determines the appropriate background color for active items based on the specified control variant and theme
    /// role.
    /// </summary>
    /// <remarks>The method evaluates multiple flags of the ControlVariant enumeration and the ThemeRole to
    /// select the most suitable background color. Ensure that the variant and role parameters are set according to the
    /// desired visual effect.</remarks>
    /// <param name="control">The specified control.</param>
    /// <param name="variant">A set of flags that specifies the visual style of the control. Determines how the background color is selected.</param>
    /// <param name="role">The theme role that influences the background color selection, such as whether contrast styling should be
    /// applied.</param>
    /// <returns>An object representing the resolved background color for active items, based on the provided control variant and
    /// theme role.</returns>
    private static object ResolveItemsActiveBackground(Control control, ControlVariant variant, ThemeRole role)
        => (variant.HasFlag(ControlVariant.Light) && role == ThemeRole.Contrast) || (variant.HasFlag(ControlVariant.Solid) && variant.HasFlag(ControlVariant.Outlined)) ? GetFocusForeground(control)
            : variant.HasFlag(ControlVariant.Light) ? FocusPrimaryItemsRole
            : variant.HasFlag(ControlVariant.Text) || variant.HasFlag(ControlVariant.Outlined) ? Brushes.Transparent
            : variant.HasFlag(ControlVariant.Solid) && role == ThemeRole.Contrast ? GetForeground(control)
            : PrimaryItemsRole;

    /// <summary>
    /// Determines the appropriate foreground color for active items based on the specified control variant and theme
    /// role.
    /// </summary>
    /// <remarks>This method evaluates the combination of control variant and theme role to select the correct
    /// foreground color for active items. It supports different visual styles, including text, light, and outlined
    /// variants, and ensures that the color aligns with the intended theme context.</remarks>
    /// <param name="control">The specified control.</param>
    /// <param name="variant">The control variant that influences the selection of the foreground color. This parameter determines whether
    /// text, light, or outlined styles are applied.</param>
    /// <param name="role">The theme role that specifies the context in which the foreground color is used, such as contrast or primary
    /// roles.</param>
    /// <returns>An object representing the resolved foreground color for active items, selected according to the provided
    /// control variant and theme role.</returns>
    private static object ResolveItemsActiveForeground(Control control, ControlVariant variant, ThemeRole role)
        => variant.HasFlag(ControlVariant.Text) && role == ThemeRole.Contrast ? GetForeground(control)
            : variant.HasFlag(ControlVariant.Text) ? PrimaryItemsRole
            : variant.HasFlag(ControlVariant.Light) || variant.HasFlag(ControlVariant.Outlined) ? GetForeground(control)
            : ContrastedActiveBackground;

    /// <summary>
    /// Resolves the appropriate border brush for items based on the specified control variant and theme role.
    /// </summary>
    /// <remarks>Use this method to dynamically select a border brush that matches the visual state and theme
    /// context of a control. This ensures consistent styling across different control variants and theme
    /// roles.</remarks>
    /// <param name="control">The specified control.</param>
    /// <param name="variant">A value that specifies the control variant, which determines the visual style of the border. Must be a valid
    /// member of the ControlVariant enumeration.</param>
    /// <param name="role">A value that specifies the theme role, indicating the context in which the border brush is applied. Must be a
    /// valid member of the ThemeRole enumeration.</param>
    /// <returns>An object representing the resolved border brush. Returns the foreground brush if the variant is outlined and
    /// the role is contrast; otherwise, returns the primary items role brush or a transparent brush if neither
    /// condition is met.</returns>
    private static object ResolveItemsActiveBorderBrush(Control control, ControlVariant variant, ThemeRole role)
        => variant.HasFlag(ControlVariant.Outlined) && role == ThemeRole.Contrast ? GetForeground(control)
            : variant.HasFlag(ControlVariant.Outlined) ? PrimaryItemsRole
            : Brushes.Transparent;

    /// <summary>
    /// Determines the appropriate ripple color for items based on the specified control variant and theme role.
    /// </summary>
    /// <remarks>The method evaluates the combination of the control variant and theme role to ensure that the
    /// ripple color provides appropriate visual feedback for the user interface. When the variant includes text and the
    /// role is contrast, the foreground color is used to maximize visibility.</remarks>
    /// <param name="control">The specified control.</param>
    /// <param name="variant">The control variant that influences the ripple color selection. This parameter specifies the visual state or
    /// style of the control, such as text or other variants.</param>
    /// <param name="role">The theme role that defines the context in which the ripple color is applied. This parameter affects the
    /// contrast and visibility of the ripple effect.</param>
    /// <returns>An object representing the resolved ripple color. The return value may be a brush corresponding to the
    /// foreground or primary items role, or a transparent brush if no specific color is applicable.</returns>
    private static object ResolveItemsRippleColor(Control control, ControlVariant variant, ThemeRole role)
        => variant.HasFlag(ControlVariant.Text) && role == ThemeRole.Contrast ? GetForeground(control)
            : variant.HasFlag(ControlVariant.Text) ? PrimaryItemsRole
            : Brushes.Transparent;

    /// <summary>
    /// Determines the appropriate foreground color to use for the specified control.
    /// </summary>
    /// <remarks>If the control is a TabControl, the method returns a specific header foreground color;
    /// otherwise, it returns a general high foreground color.</remarks>
    /// <param name="control">The control for which to retrieve the foreground color. This can be a TabControl or any other Control type.</param>
    /// <returns>An object representing the foreground color to be used for the specified control.</returns>
    private static object GetHighForeground(Control control) => control is TabControl ? HeaderHighForeground : HighForeground;

    /// <summary>
    /// Determines the appropriate foreground color to use when a control has focus.
    /// </summary>
    /// <remarks>Use this method to ensure that controls display the correct foreground color when focused,
    /// providing a consistent visual experience across different control types.</remarks>
    /// <param name="control">The control for which to retrieve the focus foreground color. The type of control may affect the returned color.</param>
    /// <returns>An object representing the foreground color to use when the specified control is focused. Returns the header
    /// focus foreground color if the control is a tab control; otherwise, returns the standard focus foreground color.</returns>
    private static object GetFocusForeground(Control control) => control is TabControl ? HeaderFocusForeground : FocusForeground;

    /// <summary>
    /// Gets the foreground color to use when a control is hovered over.
    /// </summary>
    /// <param name="control">The control for which to retrieve the hover foreground color. The returned color depends on the type of this
    /// control.</param>
    /// <returns>An object representing the hover foreground color. Returns the header hover foreground color if the control is a
    /// TabControl; otherwise, returns the standard hover foreground color.</returns>
    private static object GetHoverForeground(Control control) => control is TabControl ? HeaderHoverForeground : HoverForeground;

    /// <summary>
    /// Retrieves the appropriate foreground color for the specified control.
    /// </summary>
    /// <param name="control">The control for which to obtain the foreground color. The type of control determines which color is returned.</param>
    /// <returns>An object representing the foreground color. Returns the header foreground color if the control is a TabControl;
    /// otherwise, returns the standard foreground color.</returns>
    private static object GetForeground(Control control) => control is TabControl ? HeaderForeground : Foreground;

    #endregion

    /// <summary>
    /// Registers the variant-related classes in the ClassRegistry, enabling the application of ControlVariant values to controls
    /// based on CSS-like class names.
    /// </summary>
    public static void Register()
    {
        ClassRegistry.RegisterMany<ControlVariant, Control>(CssPrefix.Variant, setVariant, NoneManagement.Remove);
        ClassRegistry.RegisterMany<ControlVariant, Control>(CssPrefix.HeaderVariant, setHeaderVariant, NoneManagement.Remove);
        ClassRegistry.Register<Control>(CssClass.VariantHeader, x => setHeaderVariant(x, ControlVariant.Default));
        ClassRegistry.RegisterMany<ControlVariant, Control>(CssPrefix.ItemsVariant, setItemsVariant, NoneManagement.Remove);

        static IDisposable setVariant(Control control, ControlVariant variant) => apply(control, x => x.Variants, variant);
        static IDisposable setHeaderVariant(Control control, ControlVariant variant) => apply(control, x => x.HeaderVariants, variant);
        static IDisposable setItemsVariant(Control control, ControlVariant variant) => apply(control, x => x.ItemsVariants, variant);

        static IDisposable apply(Control control, Func<ControlState, HashSet<ControlVariant>> provideVariants, ControlVariant currentSingleVariant)
        {
            var context = ClassContext.Create<Control, ControlState>(control);

            provideVariants(context.State).Add(currentSingleVariant);

            ApplyState(control, context.State);

            return Disposable.Create(() =>
            {
                provideVariants(context.State).Remove(currentSingleVariant);

                ApplyState(control, context.State);
            });
        }
    }
}
