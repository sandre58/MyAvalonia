// -----------------------------------------------------------------------
// <copyright file="DialogHostOptions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;

namespace MyNet.Avalonia.Extended.Dialogs;

/// <summary>
/// Host configuration for Avalonia dialog presenters.
/// </summary>
public sealed class DialogHostOptions(Func<TopLevel?> topLevelProvider)
{
    /// <summary>
    /// Gets the function that resolves the current top level.
    /// </summary>
    public Func<TopLevel?> TopLevelProvider { get; } = topLevelProvider ?? throw new ArgumentNullException(nameof(topLevelProvider));
}
