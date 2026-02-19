// -----------------------------------------------------------------------
// <copyright file="PlaceholderContentControl.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace MyNet.Avalonia.Controls;

public class PlaceholderContentControl : ContentControl
{
    #region PlaceholderText

    /// <summary>
    /// Provides PlaceholderText Property.
    /// </summary>
    public static readonly StyledProperty<object?> PlaceholderTextProperty = AvaloniaProperty.Register<PlaceholderContentControl, object?>(nameof(PlaceholderText));

    /// <summary>
    /// Gets or sets the PlaceholderText property.
    /// </summary>
    public object? PlaceholderText
    {
        get => GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
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
