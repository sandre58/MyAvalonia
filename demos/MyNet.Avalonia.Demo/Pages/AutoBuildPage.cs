// -----------------------------------------------------------------------
// <copyright file="AutoBuildPage.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using MyNet.Avalonia.Controls.Assists;
using MyNet.Avalonia.Demo.Helpers;
using MyNet.Utilities.Logging;
using PropertyChanged;

namespace MyNet.Avalonia.Demo.Pages;

[DoNotNotify]
internal abstract class AutoBuildPage : Page
{
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        var panel = this.FindControl<Panel>("Root");

        if (panel is not null)
            Build(panel);
    }

    protected abstract IEnumerable<ControlThemeData> ProvideThemes();

    protected abstract Control CreateControl(ControlData data);

    private void Build(Panel root)
    {
        using (LogManager.MeasureTime())
        {
            foreach (var item in ProvideThemes())
            {
                // Controls
                var grid = new Grid
                {
                    [!IsEnabledProperty] = this[!IsActiveProperty]
                };

                BuildHelper.Build(grid, item, CreateControl);

                var container = new HeaderedContentControl
                {
                    Header = item.Name,
                    Content = grid,
                    ClipToBounds = false,
                    Background = Brushes.Transparent,
                    HorizontalContentAlignment = global::Avalonia.Layout.HorizontalAlignment.Stretch
                };
                HeaderAssist.SetHorizontalAlignment(container, global::Avalonia.Layout.HorizontalAlignment.Stretch);
                container.Classes.AddRange(["H2"]);

                root.Children.Add(container);
            }
        }
    }
}
