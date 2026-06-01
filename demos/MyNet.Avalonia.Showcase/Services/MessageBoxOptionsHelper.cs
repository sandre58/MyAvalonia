// -----------------------------------------------------------------------
// <copyright file="MessageBoxOptionsHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Reflection;
using MyNet.UI.Dialogs.MessageBox;

namespace MyNet.Avalonia.Showcase.Services;

/// <summary>
/// Creates <see cref="MessageBoxOptions"/> instances for showcase demos (options use internal setters).
/// </summary>
internal static class MessageBoxOptionsHelper
{
    public static MessageBoxOptions Create(
        string message,
        string? title,
        MessageSeverity severity,
        MessageBoxResultOption buttons,
        MessageBoxResult defaultResult = MessageBoxResult.Ok)
    {
        var options = new MessageBoxOptions();
        Set(options, nameof(MessageBoxOptions.Message), message);
        Set(options, nameof(MessageBoxOptions.Title), title);
        Set(options, nameof(MessageBoxOptions.Severity), severity);
        Set(options, nameof(MessageBoxOptions.Buttons), buttons);
        Set(options, nameof(MessageBoxOptions.DefaultResult), defaultResult);
        return options;
    }

    private static void Set(MessageBoxOptions target, string propertyName, object? value)
        => typeof(MessageBoxOptions).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public)!
            .SetValue(target, value);
}
