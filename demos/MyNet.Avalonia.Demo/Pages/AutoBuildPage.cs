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
using Avalonia.VisualTree;
using MyNet.Avalonia.Demo.Helpers;
using MyNet.Avalonia.Theme.Assists;
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

            // Build controls progressively with small batches to keep UI responsive
            // Larger batches = faster but may freeze UI
            // Smaller batches = slower but smoother UI
            var batchSize = 2; // Process 2 sections at a time (good compromise)

            for (var i = 0; i < themes.Count; i += batchSize)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    LogManager.Debug($"[PERF] AutoBuildPage ({GetType().Name}) - Build cancelled at section {i}");
                    return;
                }

                var batch = themes.Skip(i).Take(batchSize);

                // Build batch on the UI thread with normal priority
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;

                    foreach (var item in batch)
                    {
                        using (PerformanceMonitor.Measure($"AutoBuildPage ({GetType().Name}) - Build section '{item.Name}'"))
                        {
                            var container = BuildThemeSection(item);
                            root.Children.Add(container);
                        }
                    }
                },
                DispatcherPriority.Normal);

                // Small delay to allow UI to update between batches
                await Task.Delay(1, cancellationToken).ConfigureAwait(false);
            }

            // Debug total visual elements created
            var totalControls = root.GetVisualDescendants().Count();
            LogManager.Debug($"[PERF] AutoBuildPage ({GetType().Name}) - Total visual elements created: {totalControls}");
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
