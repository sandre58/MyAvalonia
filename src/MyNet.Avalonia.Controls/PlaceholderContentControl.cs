// -----------------------------------------------------------------------
// <copyright file="PlaceholderContentControl.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Media;

namespace MyNet.Avalonia.Controls;

public class PlaceholderContentControl : ContentControl
{
    #region Placeholder

    /// <summary>
    /// Provides Placeholder Property.
    /// </summary>
    public static readonly StyledProperty<object?> PlaceholderProperty = AvaloniaProperty.Register<PlaceholderContentControl, object?>(nameof(Placeholder));

    /// <summary>
    /// Gets or sets the Placeholder property.
    /// </summary>
    public object? Placeholder
    {
        get => GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    #endregion

    #region PlaceholderTemplate

    /// <summary>
    /// Provides PlaceholderTemplate Property.
    /// </summary>
    public static readonly StyledProperty<IDataTemplate?> PlaceholderTemplateProperty = AvaloniaProperty.Register<PlaceholderContentControl, IDataTemplate?>(nameof(PlaceholderTemplate));

    /// <summary>
    /// Gets or sets the PlaceholderTemplate property.
    /// </summary>
    public IDataTemplate? PlaceholderTemplate
    {
        get => GetValue(PlaceholderTemplateProperty);
        set => SetValue(PlaceholderTemplateProperty, value);
    }

    #endregion
}
