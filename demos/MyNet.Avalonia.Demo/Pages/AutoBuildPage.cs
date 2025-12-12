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
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.VisualTree;
using MyNet.Avalonia.Demo.Helpers;
using MyNet.Avalonia.Extensions;
using MyNet.Avalonia.Helpers;
using MyNet.Avalonia.Theme;
using MyNet.Avalonia.Theme.Assists;
using MyNet.Utilities.Logging;
using PropertyChanged;

namespace MyNet.Avalonia.Demo.Pages;

/// <summary>
/// Base page for automatic construction of themed demo sections with lazy loading and performance logging.
/// </summary>
[DoNotNotify]
internal abstract class AutoBuildPage : Page, IDisposable
{
    /// <summary>
    /// Number of sections to expand and build immediately on page load.
    /// </summary>
    private const int InitialExpandedSectionCount = 1;

    /// <summary>
    /// Spacing between section containers.
    /// </summary>
    private const double SectionSpacing = 12d;

    private CancellationTokenSource? _cancellationTokenSource;
    private bool _disposedValue;

    /// <summary>
    /// Called when the control template is applied. Triggers page construction.
    /// </summary>
    /// <param name="e">Template event args.</param>
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

    /// <summary>
    /// Provides the theme sections to build for this page.
    /// </summary>
    /// <returns>Theme section data.</returns>
    protected abstract IEnumerable<ControlThemeData> ProvideThemes();

    /// <summary>
    /// Factory for creating a demo control for a given theme data.
    /// </summary>
    /// <param name="data">Theme data for the control.</param>
    /// <returns>The created control.</returns>
    protected abstract Control CreateControl(ControlData data);

    /// <summary>
    /// Prepares the root panel for section containers.
    /// </summary>
    /// <param name="root">The root panel.</param>
    /// <returns>A cleared stack panel for section containers.</returns>
    private static StackPanel PrepareRootPanel(Panel root)
    {
        if (root is StackPanel stackPanel)
        {
            stackPanel.Children.Clear();
            return stackPanel;
        }

        root.Children.Clear();
        var host = new StackPanel();
        root.Children.Add(host);
        return host;
    }

    /// <summary>
    /// Gets the display name for a section.
    /// </summary>
    /// <param name="theme">Theme data.</param>
    /// <returns>Display name for the section.</returns>
    private static string GetSectionDisplayName(ControlThemeData theme)
        => string.IsNullOrWhiteSpace(theme.Name) ? string.Empty : theme.Name;

    /// <summary>
    /// Creates the header panel for a section, including title and summary.
    /// </summary>
    /// <param name="theme">Theme data.</param>
    /// <param name="displayName">Section display name.</param>
    /// <returns>StackPanel containing header elements.</returns>
    private static StackPanel CreateSectionHeader(ControlThemeData theme, string displayName)
    {
        var subtitleParts = new List<string>
        {
            $"{Math.Max(theme.Layouts.Count, 1)} layout(s)"
        };

        var styleCount = theme.Styles.Count(x => x.Length == 1);
        if (styleCount > 0)
            subtitleParts.Add($"{styleCount} style(s)");

        if (theme.Colors.Count > 0)
            subtitleParts.Add($"{theme.Colors.Count} role(s)");

        if (theme.Sizes.Count > 0)
            subtitleParts.Add($"{theme.Sizes.Count} size(s)");

        if (theme.CustomControls.Count > 0)
            subtitleParts.Add($"{theme.CustomControls.Count} custom control(s)");

        var headerPanel = new StackPanel();

        var title = new SelectableTextBlock
        {
            Text = displayName,
            Margin = new Thickness(0, 0, 0, 2)
        };
        title.Classes.Add("H3");
        headerPanel.Children.Add(title);

        if (subtitleParts.Count > 0)
        {
            var subtitle = new TextBlock
            {
                Text = string.Join(" · ", subtitleParts)
            };
            subtitle.AddClasses("Caption Secondary");
            headerPanel.Children.Add(subtitle);
        }

        return headerPanel;
    }

    /// <summary>
    /// Creates a border container for section content, with a loading placeholder.
    /// </summary>
    /// <param name="displayName">Section display name.</param>
    /// <returns>Border containing loading placeholder.</returns>
    private static Border CreateSectionContentHost(string displayName) => new()
    {
        CornerRadius = new CornerRadius(8),
        Padding = new Thickness(12),
        Background = Brushes.Transparent,
        Child = CreateLoadingPlaceholder(displayName)
    };

    /// <summary>
    /// Creates a loading placeholder panel for section content.
    /// </summary>
    /// <param name="displayName">Section display name.</param>
    /// <returns>StackPanel with loading indicator.</returns>
    private static StackPanel CreateLoadingPlaceholder(string displayName)
    {
        var panel = new StackPanel();

        var label = new TextBlock
        {
            Text = $"Building \"{displayName}\"...",
            Margin = new Thickness(0, 0, 0, 4),
            Opacity = 0.7
        };
        label.Classes.Add("Caption");
        panel.Children.Add(label);

        var progress = new ProgressBar
        {
            IsIndeterminate = true,
            Height = 3
        };
        panel.Children.Add(progress);

        return panel;
    }

    /// <summary>
    /// Asynchronously builds all theme sections and attaches them to the root panel.
    /// </summary>
    /// <param name="root">Root panel for section containers.</param>
    private async Task BuildAsync(Panel root)
    {
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

            LogManager.Debug($"[PERF] AutoBuildPage ({GetType().Name}) - Preparing {themes.Count} theme sections");

            var container = PrepareRootPanel(root);
            var eagerBuilds = new List<Task>();

            for (var i = 0; i < themes.Count; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    LogManager.Debug($"[PERF] AutoBuildPage ({GetType().Name}) - Build cancelled before section {i}");
                    break;
                }

                var buildImmediately = i < InitialExpandedSectionCount;
                var section = CreateSectionContainer(themes[i], i, buildImmediately, eagerBuilds, cancellationToken);
                container.Children.Add(section);
            }

            if (eagerBuilds.Count > 0)
            {
                try
                {
                    await Task.WhenAll(eagerBuilds).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    LogManager.Debug($"[PERF] AutoBuildPage ({GetType().Name}) - Eager builds cancelled");
                }
            }

            var totalControls = root.GetVisualDescendants().Count();
            LogManager.Debug($"[PERF] AutoBuildPage ({GetType().Name}) - Total visual elements instantiated (headers + built sections): {totalControls}");
        }
    }

    /// <summary>
    /// Creates an expander container for a theme section, with lazy build logic.
    /// </summary>
    /// <param name="theme">Theme data.</param>
    /// <param name="index">Section index.</param>
    /// <param name="buildImmediately">Whether to build immediately or on expand.</param>
    /// <param name="eagerBuilds">List to track immediate build tasks.</param>
    /// <param name="cancellationToken">Cancellation token for build.</param>
    /// <returns>Expander for the section.</returns>
    private Expander CreateSectionContainer(ControlThemeData theme, int index, bool buildImmediately, List<Task> eagerBuilds, CancellationToken cancellationToken)
    {
        var displayName = GetSectionDisplayName(theme);
        var contentHost = CreateSectionContentHost(displayName);
        var expander = new Expander
        {
            Header = CreateSectionHeader(theme, displayName),
            Content = new ScrollViewer
            {
                Content = contentHost,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                VerticalScrollBarVisibility = ScrollBarVisibility.Disabled
            },
            Margin = new Thickness(0, 0, 0, SectionSpacing),
            HorizontalAlignment = global::Avalonia.Layout.HorizontalAlignment.Stretch,
            IsExpanded = buildImmediately
        };
        expander.AddClasses("AutoBuildSection Outlined Headered");
        ThemeAssist.SetRole(expander, Avalonia.Theme.Palettes.ThemeRole.Dark);

        var state = new SectionBuildState(theme, contentHost, displayName, index, cancellationToken);
        expander.Tag = state;

        if (buildImmediately)
        {
            eagerBuilds.Add(ScheduleSectionBuildAsync(expander, state));
        }
        else
        {
            expander.Expanded += SectionExpandedAsync;
        }

        return expander;
    }

    /// <summary>
    /// Handles section expansion to trigger lazy build.
    /// </summary>
    private async void SectionExpandedAsync(object? sender, RoutedEventArgs e)
    {
        if (sender is not Expander expander || expander.Tag is not SectionBuildState state)
            return;

        try
        {
            await ScheduleSectionBuildAsync(expander, state).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            LogManager.Debug($"[PERF] AutoBuildPage ({GetType().Name}) - Build cancelled for '{state.DisplayName}' on expand");
        }
    }

    /// <summary>
    /// Schedules the build of a section on the UI thread.
    /// </summary>
    private async Task ScheduleSectionBuildAsync(Expander owner, SectionBuildState state)
    {
        if (state.IsBuilt || state.IsBuilding || state.CancellationToken.IsCancellationRequested)
            return;

        state.IsBuilding = true;
        LogManager.Debug($"[PERF] AutoBuildPage ({GetType().Name}) - Queue build for section '{state.DisplayName}' (index {state.Index})");

        try
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                if (state.IsBuilt || state.CancellationToken.IsCancellationRequested)
                    return;

                var grid = BuildThemeSectionContent(state.Theme, state.DisplayName, state.CancellationToken);
                state.ContentHost.Child = grid;
                state.IsBuilt = true;
                LogManager.Debug($"[PERF] AutoBuildPage ({GetType().Name}) - Section '{state.DisplayName}' ready with {grid.Children.Count} child containers");
            },
            DispatcherPriority.Background);
        }
        finally
        {
            state.IsBuilding = false;
            if (state.IsBuilt)
                owner.Expanded -= SectionExpandedAsync;
        }
    }

    /// <summary>
    /// Builds the content grid for a theme section.
    /// </summary>
    /// <param name="item">Theme data.</param>
    /// <param name="displayName">Section display name.</param>
    /// <param name="cancellationToken">Cancellation token for build.</param>
    /// <returns>Grid containing built controls.</returns>
    private Grid BuildThemeSectionContent(ControlThemeData item, string displayName, CancellationToken cancellationToken)
    {
        var grid = new Grid
        {
            [!IsEnabledProperty] = this[!IsActiveProperty]
        };

        int controlCount;
        using (PerformanceMonitor.Measure($"BuildHelper.Build for '{displayName}'"))
        {
            BuildHelper.Build(grid, item, CreateControl, cancellationToken);
            controlCount = grid.Children.Count;
        }

        if (cancellationToken.IsCancellationRequested)
        {
            LogManager.Debug($"[PERF] Section '{displayName}' - Build aborted early");
            return grid;
        }

        LogManager.Debug($"[PERF] Section '{displayName}' - Created {controlCount} controls");

        return grid;
    }

    /// <summary>
    /// Disposes resources used by the page.
    /// </summary>
    /// <param name="disposing">True if called from Dispose().</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _cancellationTokenSource?.Dispose();
            }

            _disposedValue = true;
        }
    }

    /// <summary>
    /// Disposes the page and suppresses finalization.
    /// </summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// State for a section's build process.
    /// </summary>
    /// <remarks>
    /// Creates a new build state for a section.
    /// </remarks>
    /// <param name="theme">Theme data.</param>
    /// <param name="contentHost">Content host border.</param>
    /// <param name="displayName">Section display name.</param>
    /// <param name="index">Section index.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    private sealed class SectionBuildState(ControlThemeData theme, Border contentHost, string displayName, int index, CancellationToken cancellationToken)
    {
        /// <summary>
        /// Gets theme data for the section.
        /// </summary>
        public ControlThemeData Theme { get; } = theme;

        /// <summary>
        /// Gets content host border for the section.
        /// </summary>
        public Border ContentHost { get; } = contentHost;

        /// <summary>
        /// Gets display name for the section.
        /// </summary>
        public string DisplayName { get; } = displayName;

        /// <summary>
        /// Gets index of the section.
        /// </summary>
        public int Index { get; } = index;

        /// <summary>
        /// Gets cancellation token for the build.
        /// </summary>
        public CancellationToken CancellationToken { get; } = cancellationToken;

        /// <summary>
        /// Gets or sets a value indicating whether true if the section has been built.
        /// </summary>
        public bool IsBuilt { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether true if the section is currently building.
        /// </summary>
        public bool IsBuilding { get; set; }
    }
}
