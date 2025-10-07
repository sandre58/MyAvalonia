// -----------------------------------------------------------------------
// <copyright file="IAvaloniaTheme.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Media;

namespace MyNet.Avalonia.Theming;

public interface IAvaloniaTheme
{
    Color PrimaryColor { get; set; }

    Color? PrimaryForegroundColor { get; set; }

    Color AccentColor { get; set; }

    Color? AccentForegroundColor { get; set; }
}
