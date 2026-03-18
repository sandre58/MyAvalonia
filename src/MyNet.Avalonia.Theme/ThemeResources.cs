// -----------------------------------------------------------------------
// <copyright file="ThemeResources.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading;
using Avalonia;
using Avalonia.Media;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.Avalonia.Theme.Theming;
using MyNet.Utilities;

namespace MyNet.Avalonia.Theme;

/// <summary>
/// Represents a static class that provides access to theme-related resources, such as animation durations, colors, brushes, and other theming elements. This class serves as a centralized point for retrieving theme resources in a type-safe manner, ensuring that the correct resource keys are used and that resources are accessed efficiently through lazy loading.
/// </summary>
public static class ThemeResources
{
    /// <summary>
    /// Provides static properties that represent animation-related resources for use in UI development.
    /// </summary>
    /// <remarks>The Animation class exposes tokens that can be used to reference animation properties, such
    /// as opacity, in a consistent manner throughout the application. This approach simplifies access to animation
    /// resources and promotes reuse across different UI components.</remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "It simplifies access to animation-related resources.")]
    public static class Animation
    {
        /// <summary>
        /// Gets the token that represents the duration of the opacity animation as a <see cref="TimeSpan"/> value.
        /// </summary>
        /// <remarks>This property can be used to retrieve the animation duration for opacity transitions
        /// in UI elements, allowing for consistent timing across the application.</remarks>
        public static Token<TimeSpan> Opacity { get; } = new Token<TimeSpan>(ThemeResourceKeyFactory.Animation(nameof(Opacity)));
    }

    /// <summary>
    /// Represents a nested static class that provides access to shadow-related resources for use in UI development. This class contains tokens that can be used to reference shadow properties, such as shadow depth, in a consistent manner throughout the application. By centralizing access to shadow resources, this class promotes reuse and simplifies the management of shadow settings across different UI components.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "It simplifies access to animation-related resources.")]
    public static class Shadow
    {
        /// <summary>
        /// Gets the token that identifies the shadow depth setting for controls in the current theme.
        /// </summary>
        /// <remarks>Use this token to retrieve or assign the shadow depth applied to controls, ensuring
        /// consistent visual theming across the application's user interface.</remarks>
        public static Token<ShadowDepth> Control { get; } = new Token<ShadowDepth>(ThemeResourceKeyFactory.Shadow(nameof(Control)));

        /// <summary>
        /// Gets the token that identifies the shadow depth setting for surfaces in the current theme.
        /// </summary>
        /// <remarks>Use this token to retrieve or assign the shadow depth applied to surfaces, ensuring
        /// consistent visual theming across the application's user interface.</remarks>
        public static Token<ShadowDepth> Surface { get; } = new Token<ShadowDepth>(ThemeResourceKeyFactory.Shadow(nameof(Surface)));
    }

    /// <summary>
    /// Represents a nested static class that provides access to corner radius-related resources for use in UI development. This class contains tokens that can be used to reference corner radius properties, such as control and surface corner radii, in a consistent manner throughout the application. By centralizing access to corner radius resources, this class promotes reuse and simplifies the management of corner radius settings across different UI components.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "It simplifies access to animation-related resources.")]
    public static class Corners
    {
        private static readonly Dictionary<CornerSize, Token<CornerRadius>> Tokens = [];

        /// <summary>
        /// Gets the token that identifies the corner radius setting for controls in the current theme.
        /// </summary>
        /// <remarks>Use this token to retrieve or assign the corner radius applied to controls, ensuring
        /// consistent visual theming across the application's user interface.</remarks>
        public static Token<CornerRadius> Control { get; } = new Token<CornerRadius>(ThemeResourceKeyFactory.Corners(nameof(Control)));

        /// <summary>
        /// Gets the token that identifies the corner radius setting for surfaces in the current theme.
        /// </summary>
        /// <remarks>Use this token to retrieve or assign the corner radius applied to surfaces, ensuring
        /// consistent visual theming across the application's user interface.</remarks>
        public static Token<CornerRadius> Surface { get; } = new Token<CornerRadius>(ThemeResourceKeyFactory.Corners(nameof(Surface)));

        /// <summary>
        /// Retrieves a token that represents the corner radius associated with the specified corner size.
        /// </summary>
        /// <remarks>This method uses a caching mechanism to optimize performance. If a token for the
        /// specified size has already been created, it is returned from the cache; otherwise, a new token is generated
        /// and stored for future use.</remarks>
        /// <param name="size">The size of the corner for which to retrieve the token. This value determines the corner radius that will be
        /// encapsulated by the returned token.</param>
        /// <returns>A token containing the corner radius corresponding to the specified corner size.</returns>
        public static Token<CornerRadius> Get(CornerSize size) => Tokens.GetOrAdd(size, new Token<CornerRadius>(ThemeResourceKeyFactory.Corners(size.ToString())));
    }

    /// <summary>
    /// Represents a nested static class that provides access to spacing-related resources for use in UI development. This class contains tokens that can be used to reference spacing properties, such as padding and margin, in a consistent manner throughout the application. By centralizing access to spacing resources, this class promotes reuse and simplifies the management of spacing settings across different UI components.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "It simplifies access to animation-related resources.")]
    public static class Spacing
    {
        private static readonly Dictionary<SpacingSize, Token<double>> Tokens = [];

        /// <summary>
        /// Retrieves a token that represents the spacing associated with the specified spacing size.
        /// </summary>
        /// <remarks>This method uses a caching mechanism to optimize performance. If a token for the
        /// specified size has already been created, it is returned from the cache; otherwise, a new token is generated
        /// and stored for future use.</remarks>
        /// <param name="size">The size of the spacing for which to retrieve the token. This value determines the spacing that will be
        /// encapsulated by the returned token.</param>
        /// <returns>A token containing the spacing corresponding to the specified spacing size.</returns>
        public static Token<double> Get(SpacingSize size) => Tokens.GetOrAdd(size, new Token<double>(ThemeResourceKeyFactory.Spacing(size.ToString())));
    }

    /// <summary>
    /// Represents a nested static class that provides access to font-related resources for use in UI development. This class contains tokens that can be used to reference font properties, such as size and weight, in a consistent manner throughout the application. By centralizing access to font resources, this class promotes reuse and simplifies the management of font settings across different UI components.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "It simplifies access to animation-related resources.")]
    public static class Font
    {
        /// <summary>
        /// Represents a nested static class that provides access to font size-related resources for use in UI development. This class contains tokens that can be used to reference font size properties, such as header font size, in a consistent manner throughout the application. By centralizing access to font size resources, this class promotes reuse and simplifies the management of font size settings across different UI components.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "It simplifies access to animation-related resources.")]
        public static class Size
        {
            private static readonly Dictionary<FontSize, Token<double>> Tokens = [];

            /// <summary>
            /// Retrieves a token that represents the font size associated with the specified font size.
            /// </summary>
            /// <remarks>This method uses a caching mechanism to optimize performance. If a token for the
            /// specified size has already been created, it is returned from the cache; otherwise, a new token is generated
            /// and stored for future use.</remarks>
            /// <param name="size">The size of the font for which to retrieve the token. This value determines the font size that will be
            /// encapsulated by the returned token.</param>
            /// <returns>A token containing the font size corresponding to the specified font size.</returns>
            public static Token<double> Get(FontSize size) => Tokens.GetOrAdd(size, new Token<double>(ThemeResourceKeyFactory.FontSize(size.ToString())));
        }

        /// <summary>
        /// Represents a nested static class that provides access to font weight-related resources for use in UI development. This class contains tokens that can be used to reference font weight properties, such as header font weight, in a consistent manner throughout the application. By centralizing access to font weight resources, this class promotes reuse and simplifies the management of font weight settings across different UI components.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "It simplifies access to animation-related resources.")]
        public static class Weight
        {
            /// <summary>
            /// Gets the token that identifies the font weight setting for headers in the current theme.
            /// </summary>
            public static Token<FontWeight> Header { get; } = new Token<FontWeight>(ThemeResourceKeyFactory.FontWeight(nameof(Header)));
        }
    }

    /// <summary>
    /// Provides access to predefined icon geometries used in animations.
    /// </summary>
    /// <remarks>This class simplifies access to animation-related resources by providing a method to retrieve
    /// geometry tokens based on a specified key. The geometries are cached for performance optimization.</remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "It simplifies access to animation-related resources.")]
    public static class Icons
    {
        private static readonly Dictionary<string, Token<StreamGeometry>> GeometryTokens = [];

        /// <summary>
        /// Retrieves a token that represents the StreamGeometry resource associated with the specified key. If the
        /// resource does not exist, a new token is created and added to the collection.
        /// </summary>
        /// <param name="key">The unique key that identifies the StreamGeometry resource to retrieve or create.</param>
        /// <returns>A token that represents the StreamGeometry resource associated with the specified key.</returns>
        public static Token<StreamGeometry> Get(string key) => GeometryTokens.GetOrAdd(key, new Token<StreamGeometry>(ThemeResourceKeyFactory.Geometry(key)));
    }

    /// <summary>
    /// Represents a token that provides lazy access to a resource of the specified type, identified by a unique key.
    /// </summary>
    /// <remarks>The resource is not retrieved until the first time the Value property is accessed. This can
    /// improve performance by deferring resource loading until it is actually needed.</remarks>
    /// <typeparam name="T">The type of the resource to be retrieved.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "It simplifies access to theme-related resources.")]
    public record Token<T>
    {
        private readonly Lazy<T> _lazyValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="Token{T}"/> class with the specified key. The key is used to identify the resource that will be retrieved when the Value property is accessed for the first time.
        /// </summary>
        /// <param name="key">The unique key that identifies the resource to retrieve.</param>
        public Token(string key)
        {
            Key = key;
            var capturedKey = key; // avoid capturing `this` inside the factory
            _lazyValue = new Lazy<T>(() => Avalonia.ThemeResources.GetResource<T>(capturedKey), LazyThreadSafetyMode.PublicationOnly);
        }

        /// <summary>
        /// Gets the unique key that identifies the resource associated with this token. This key is used to retrieve the resource from the theme resources when the Value property is accessed for the first time.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets the lazily initialized value of the current instance.
        /// </summary>
        /// <remarks>The value is computed on first access and cached for subsequent accesses. This
        /// approach can improve performance by delaying the computation until it is actually needed.</remarks>
        public T Value => _lazyValue.Value;
    }
}
