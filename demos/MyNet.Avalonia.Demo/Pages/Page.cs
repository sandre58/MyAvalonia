// -----------------------------------------------------------------------
// <copyright file="Page.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using MyNet.Avalonia.Helpers;
using PropertyChanged;

namespace MyNet.Avalonia.Demo.Pages;

[DoNotNotify]
internal abstract class Page : UserControl
{
    protected override void OnLoaded(RoutedEventArgs e)
    {
        using (PerformanceMonitor.Measure($"Page {GetType().Name} - OnLoaded"))
        {
            base.OnLoaded(e);
        }

        var controlCount = this.GetVisualDescendants().Count();
        PerformanceMonitor.Debug($"FormsPage - Total controls instantiated: {controlCount}");
    }
}
