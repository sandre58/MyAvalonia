// -----------------------------------------------------------------------
// <copyright file="IClipboardFeedback.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Clipboard;

/// <summary>
/// Optional user feedback after clipboard operations.
/// </summary>
public interface IClipboardFeedback
{
    /// <summary>
    /// Notifies that content was copied successfully.
    /// </summary>
    void NotifySuccess();

    /// <summary>
    /// Notifies that copying failed.
    /// </summary>
    void NotifyError();
}
