// -----------------------------------------------------------------------
// <copyright file="DataGridHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;

namespace MyNet.Avalonia.Controls.Helpers;

internal static class DataGridHelper
{
    internal static void SynchronizeColumnProperty<T>(AvaloniaObject column, AvaloniaObject content, AvaloniaProperty<T> property) => SynchronizeColumnProperty(column, content, property, property);

    internal static void SynchronizeColumnProperty<T>(AvaloniaObject column, AvaloniaObject content, AvaloniaProperty<T> contentProperty, AvaloniaProperty<T> columnProperty)
    {
        if (!column.IsSet(columnProperty))
        {
            content.ClearValue(contentProperty);
        }
        else
        {
            _ = content.SetValue(contentProperty, column.GetValue(columnProperty));
        }
    }
}
