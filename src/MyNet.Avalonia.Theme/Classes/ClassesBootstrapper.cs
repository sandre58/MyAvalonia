// -----------------------------------------------------------------------
// <copyright file="ClassesBootstrapper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Theme.Classes.Registry;

namespace MyNet.Avalonia.Theme.Classes;

/// <summary>
/// Provides a bootstrapper for initializing and registering all utility classes required by the application.
/// </summary>
/// <remarks>Call this class's initialization method once during application startup to ensure that all utility
/// classes are properly registered and available for use. This class is intended to centralize the setup of utility
/// components, helping to prevent duplicate registrations and ensuring consistent application behavior.</remarks>
public static class ClassesBootstrapper
{
    private static bool _initialized;

    /// <summary>
    /// Initializes the utility classes by calling their respective registration methods. This method should be called once at the application startup to ensure that all utilities are properly registered and ready for use throughout the application.
    /// </summary>
    public static void Initialize()
    {
        if (_initialized)
            return;

        _initialized = true;

        AlignClassRegistry.Register();
        AnimationClassRegistry.Register();
        BorderClassRegistry.RegisterBorderThickness();
        BorderClassRegistry.RegisterCornerRadius();
        FocusClassRegistry.Register();
        IconClassRegistry.Register();
        LayoutClassRegistry.Register();
        OpacityClassRegistry.Register();
        ShapeClassRegistry.Register();
        ShadowClassRegistry.Register();
        SpacingClassRegistry.RegisterMargins();
        SpacingClassRegistry.RegisterSpacings();
        SpacingClassRegistry.RegisterPaddings();
        StateClassRegistry.Register();
        TypographyClassRegistry.RegisterTexts();
        TypographyClassRegistry.RegisterFonts();
        TypographyClassRegistry.RegisterDecorations();
        VariantClassRegistry.Register();
    }
}
