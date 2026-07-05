// -----------------------------------------------------------------------
// <copyright file="ClipboardAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Windows.Input;
using Avalonia;
using MyNet.Avalonia.Clipboard;
using MyNet.Avalonia.Commands;

namespace MyNet.Avalonia.Theme.Assists;

public static class ClipboardAssist
{
    /// <summary>
    /// Gets a command that copies text to the clipboard.
    /// Kept on this assist because ControlThemes bind clipboard buttons directly to it;
    /// it is not attached-property behavior on the host control.
    /// </summary>
    public static ICommand? CopyTextCommand { get; } = ActionCommand.Create<string>(async x => await ClipboardManager.CopyTextAsync(x).ConfigureAwait(false));

    #region Content

    /// <summary>
    /// Provides Content Property for attached ClipboardAssist element.
    /// </summary>
    public static readonly AttachedProperty<object?> ContentProperty = AvaloniaProperty.RegisterAttached<StyledElement, object?>("Content", typeof(ClipboardAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="ContentProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ContentProperty"/>.</param>
    public static void SetContent(StyledElement element, object? value) => element.SetValue(ContentProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ContentProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static object? GetContent(StyledElement element) => element.GetValue(ContentProperty);

    #endregion
}
