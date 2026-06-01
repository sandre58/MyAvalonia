// -----------------------------------------------------------------------
// <copyright file="OverlayDialogHostKey.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls.Internals;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal readonly record struct OverlayDialogHostKey(string? Id, int? TopLevelKey);
