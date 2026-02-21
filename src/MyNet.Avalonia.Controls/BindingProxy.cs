// -----------------------------------------------------------------------
// <copyright file="BindingProxy.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;

namespace MyNet.Avalonia.Controls;

public class BindingProxy : AvaloniaObject
{
    public static readonly StyledProperty<object?> DataProperty = AvaloniaProperty.Register<BindingProxy, object?>(nameof(Data));

    public object? Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }
}
