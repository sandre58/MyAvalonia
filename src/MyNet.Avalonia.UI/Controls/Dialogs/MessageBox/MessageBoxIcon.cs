// -----------------------------------------------------------------------
// <copyright file="MessageBoxIcon.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.UI.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public enum MessageBoxIcon
{
    Asterisk, // Same as Information
    Error,
    Exclamation, // Same as Warning
    Hand, // Same as Error
    Information,
    None,
    Question,
    Stop, // Same as Error
    Warning,
    Success,
}
