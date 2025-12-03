// -----------------------------------------------------------------------
// <copyright file="IAvaloniaTheme.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Media;

namespace MyNet.Avalonia.Theming;

public interface IAvaloniaTheme
{
    void SetPrimary(Color color, Color? foreground);

    void SetAccent(Color color, Color? foreground);

    void SetTheme(string? name);

    string? GetThemeName();

    ColorPair GetPrimaryPair();

    ColorPair GetAccentPair();
}
