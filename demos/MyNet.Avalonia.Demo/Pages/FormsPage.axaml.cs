// -----------------------------------------------------------------------
// <copyright file="FormsPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Demo.Helpers;
using MyNet.Avalonia.Helpers;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class FormsPage : Page
{
    public FormsPage()
    {
        using (PerformanceMonitor.Measure("FormsPage - InitializeComponent"))
        {
            InitializeComponent();
        }
    }
}
