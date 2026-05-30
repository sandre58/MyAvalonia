// -----------------------------------------------------------------------
// <copyright file="ToastClipboardFeedback.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Clipboard;
using MyNet.UI.Resources;
using MyNet.UI.Toasting;

namespace MyNet.Avalonia.Extended.Clipboard;

/// <summary>
/// Shows toast notifications after clipboard operations.
/// </summary>
public sealed class ToastClipboardFeedback : IClipboardFeedback
{
    /// <inheritdoc />
    public void NotifySuccess() => ToasterManager.ShowInformation(MessageResources.CopyInClipBoardSuccess);

    /// <inheritdoc />
    public void NotifyError() => ToasterManager.ShowError(MessageResources.CopyInClipBoardError);
}
