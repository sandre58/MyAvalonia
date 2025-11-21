// -----------------------------------------------------------------------
// <copyright file="AutoBuildPage.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Threading;
using MyNet.Avalonia.Controls.Assists;
using MyNet.Avalonia.Demo.Helpers;
using MyNet.Utilities.Logging;
using PropertyChanged;

namespace MyNet.Avalonia.Demo.Pages;

[DoNotNotify]
internal abstract class AutoBuildPage : Page, IDisposable
{
    private CancellationTokenSource? _cancellationTokenSource;
    private bool _disposedValue;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        using (PerformanceMonitor.Measure($"AutoBuildPage ({GetType().Name}) - OnApplyTemplate"))
        {
            base.OnApplyTemplate(e);

            var panel = this.FindControl<Panel>("Root");

            if (panel is not null)
                _ = BuildAsync(panel);
        }
    }

    protected abstract IEnumerable<ControlThemeData> ProvideThemes();

    protected abstract Control CreateControl(ControlData data);

    private async Task BuildAsync(Panel root)
    {
        // Cancel any previous build operation
        if (_cancellationTokenSource is not null)
            await _cancellationTokenSource.CancelAsync().ConfigureAwait(false);
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = _cancellationTokenSource.Token;

        using (PerformanceMonitor.Measure($"AutoBuildPage ({GetType().Name}) - Total Build"))
        {
            List<ControlThemeData> themes;
            using (PerformanceMonitor.Measure($"AutoBuildPage ({GetType().Name}) - ProvideThemes"))
            {
                themes = [.. ProvideThemes()];
            }

            LogManager.Debug($"[PERF] AutoBuildPage ({GetType().Name}) - Building {themes.Count} theme sections");

            // Build controls progressively
            var sectionIndex = 0;
            foreach (var item in themes)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    LogManager.Debug($"[PERF] AutoBuildPage ({GetType().Name}) - Build cancelled at section {sectionIndex}");
                    return;
                }

                sectionIndex++;

                // Build each theme section on the UI thread with low priority
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;

                    using (PerformanceMonitor.Measure($"AutoBuildPage ({GetType().Name}) - Build section '{item.Name}'"))
                    {
                        var container = BuildThemeSection(item);
                        root.Children.Add(container);
                    }
                },
                DispatcherPriority.Background);

                // Small delay to allow UI to update and remain responsive
                await Task.Delay(10, cancellationToken).ConfigureAwait(false);
            }
        }
    }

    private HeaderedContentControl BuildThemeSection(ControlThemeData item)
    {
        // Controls
        var grid = new Grid
        {
            [!IsEnabledProperty] = this[!IsActiveProperty]
        };

        int controlCount;
        using (PerformanceMonitor.Measure($"BuildHelper.Build for '{item.Name}'"))
        {
            BuildHelper.Build(grid, item, CreateControl);
            controlCount = grid.Children.Count;
        }

        LogManager.Debug($"[PERF] Section '{item.Name}' - Created {controlCount} controls");

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

        return container;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _cancellationTokenSource?.Dispose();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
