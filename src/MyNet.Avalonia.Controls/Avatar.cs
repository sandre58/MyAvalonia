// -----------------------------------------------------------------------
// <copyright file="Avatar.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace MyNet.Avalonia.Controls;

public class Avatar : ContentControl
{
    #region Source

    /// <summary>
    /// Provides Source Property.
    /// </summary>
    public static readonly StyledProperty<IImage?> SourceProperty = AvaloniaProperty.Register<Avatar, IImage?>(nameof(Source));

    /// <summary>
    /// Gets or sets the Source property.
    /// </summary>
    public IImage? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    #endregion

    #region Stretch

    /// <summary>
    /// Provides Stretch Property.
    /// </summary>
    public static readonly StyledProperty<Stretch> StretchProperty = AvaloniaProperty.Register<Avatar, Stretch>(nameof(Stretch), Stretch.UniformToFill);

    /// <summary>
    /// Gets or sets the Stretch property.
    /// </summary>
    public Stretch Stretch
    {
        get => GetValue(StretchProperty);
        set => SetValue(StretchProperty, value);
    }

    #endregion

    #region ShowBackground

    /// <summary>
    /// Provides ShowBackground Property.
    /// </summary>
    public static readonly StyledProperty<bool> ShowBackgroundProperty = AvaloniaProperty.Register<Avatar, bool>(nameof(ShowBackground), true);

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets the ShowBackground property.
    /// </summary>
    public bool ShowBackground
    {
        get => GetValue(ShowBackgroundProperty);
        set => SetValue(ShowBackgroundProperty, value);
    }

    #endregion
}
