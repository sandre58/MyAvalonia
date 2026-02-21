// -----------------------------------------------------------------------
// <copyright file="VisualExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Avalonia;
using Avalonia.VisualTree;
using MyNet.Utilities;

namespace MyNet.Avalonia.Extensions;

public static class VisualExtensions
{
    /// <summary>
    /// Executes an action on all children of a visual of type <typeparamref name="T"/>.
    /// </summary>
    public static void ExecuteOnChildren<T>(this Visual visual, Action<T> action) => visual.GetVisualDescendants().OfType<T>().ForEach(action);
}
