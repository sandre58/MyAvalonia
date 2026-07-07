// -----------------------------------------------------------------------
// <copyright file="ArrangedItem.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Controls.Primitives;
#pragma warning restore IDE0130
/// <summary>
/// Associates a toolbar child control with its explicit computed <see cref="Avalonia.Rect"/>.
/// No implicit layout is allowed — every visible item must have an explicit Rect.
/// </summary>
public sealed record ArrangedItem(Control Element, Rect Rect);
