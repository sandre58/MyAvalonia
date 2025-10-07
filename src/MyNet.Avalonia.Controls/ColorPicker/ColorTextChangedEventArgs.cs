// -----------------------------------------------------------------------
// <copyright file="ColorTextChangedEventArgs.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class ColorTextChangedEventArgs(string? text) : System.EventArgs
{
    public string? Text { get; } = text;
}
