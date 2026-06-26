// -----------------------------------------------------------------------
// <copyright file="BusyServiceIndicator.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Threading;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Resources;
using MyNet.UI.Loading;
using MyNet.UI.Loading.Models;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Extended.Controls;
#pragma warning restore IDE0130

/// <summary>
/// <see cref="BusyIndicator"/> that mirrors state from an application-wide <see cref="IBusyService"/>.
/// </summary>
/// <remarks>
/// The control ships with templates for the built-in busy models (<see cref="IndeterminateBusy"/>,
/// <see cref="DeterminateBusy"/> and <see cref="ProgressionBusy"/>). To render a custom
/// <see cref="IBusy"/> implementation, add a <see cref="DataTemplate"/> to <see cref="BusyContentTemplates"/>:
/// <code>
/// <![CDATA[
/// <ext:BusyServiceIndicator BusyService="{Binding MyService}">
///     <ext:BusyServiceIndicator.BusyContentTemplates>
///         <DataTemplate DataType="local:DownloadBusy">
///             <!-- custom presentation -->
///         </DataTemplate>
///     </ext:BusyServiceIndicator.BusyContentTemplates>
/// </ext:BusyServiceIndicator>
/// ]]>
/// </code>
/// User templates are evaluated before the built-in ones, so they can also override a default model.
/// </remarks>
public sealed class BusyServiceIndicator : BusyIndicator
{
    private static IReadOnlyList<IDataTemplate>? _defaultBusyContentTemplates;

    private IBusyService? _subscribedService;
    private ContentPresenter? _busyContentPresenter;

    /// <summary>
    /// Initializes a new instance of the <see cref="BusyServiceIndicator"/> class.
    /// </summary>
    public BusyServiceIndicator() => BusyContentTemplates.CollectionChanged += OnBusyContentTemplatesChanged;

    /// <summary>
    /// Defines the <see cref="BusyService"/> property.
    /// </summary>
    public static readonly StyledProperty<IBusyService?> BusyServiceProperty =
        AvaloniaProperty.Register<BusyServiceIndicator, IBusyService?>(nameof(BusyService));

    /// <summary>
    /// Gets or sets the application-wide busy service to observe.
    /// </summary>
    public IBusyService? BusyService
    {
        get => GetValue(BusyServiceProperty);
        set => SetValue(BusyServiceProperty, value);
    }

    /// <summary>
    /// Gets the data templates used to present the active <see cref="IBusy"/> model.
    /// Populate it in XAML to support custom <see cref="IBusy"/> implementations; entries are
    /// matched before the built-in templates, so they can also override a default presentation.
    /// </summary>
    public DataTemplates BusyContentTemplates { get; } = [];

    /// <inheritdoc />
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _busyContentPresenter = e.NameScope.Find<ContentPresenter>("PART_BusyContent");
        RebuildBusyContentTemplates();

        if (!ReferenceEquals(_subscribedService, BusyService))
        {
            Unsubscribe(_subscribedService);
            _subscribedService = BusyService;
            Subscribe(_subscribedService);
        }

        SyncFromService(BusyService);
    }

    /// <inheritdoc />
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property != BusyServiceProperty)
            return;

        Unsubscribe(_subscribedService);
        _subscribedService = change.GetNewValue<IBusyService?>();
        Subscribe(_subscribedService);
        SyncFromService(_subscribedService);
    }

    /// <inheritdoc />
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        if (!ReferenceEquals(_subscribedService, BusyService))
        {
            Unsubscribe(_subscribedService);
            _subscribedService = BusyService;
            Subscribe(_subscribedService);
        }

        SyncFromService(BusyService);
    }

    /// <inheritdoc />
    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        Unsubscribe(_subscribedService);
        _subscribedService = null;
        _busyContentPresenter = null;
        base.OnDetachedFromVisualTree(e);
    }

    private void Subscribe(IBusyService? service)
    {
        if (service is null)
            return;

        service.PropertyChanged += OnBusyServicePropertyChanged;
    }

    private void Unsubscribe(IBusyService? service)
    {
        if (service is null)
            return;

        service.PropertyChanged -= OnBusyServicePropertyChanged;
    }

    private void OnBusyServicePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not (nameof(IBusyService.IsBusy) or nameof(IBusyService.CurrentBusy)))
            return;

        Post(() => SyncFromServiceCore(_subscribedService));
    }

    private void SyncFromService(IBusyService? service) => Post(() => SyncFromServiceCore(service));

    private void SyncFromServiceCore(IBusyService? service)
    {
        var isBusy = service?.IsBusy ?? false;
        IsOpen = isBusy;

        // The model raises its own INotifyPropertyChanged events, so data-bound templates update on their own.
        // Assigning the same reference is a no-op for the presenter, which avoids rebuilding the visual tree.
        BusyContent = isBusy ? service?.CurrentBusy : null;
    }

    private void OnBusyContentTemplatesChanged(object? sender, NotifyCollectionChangedEventArgs e) =>
        Post(RebuildBusyContentTemplates);

    private void RebuildBusyContentTemplates()
    {
        if (_busyContentPresenter is null)
            return;

        _busyContentPresenter.DataTemplates.Clear();

        // User templates first so they can override (or extend) the built-in defaults.
        foreach (var template in BusyContentTemplates)
            _busyContentPresenter.DataTemplates.Add(template);

        foreach (var template in GetDefaultBusyContentTemplates())
            _busyContentPresenter.DataTemplates.Add(template);

        RefreshBusyContentPresentation();
    }

    private void RefreshBusyContentPresentation()
    {
        var content = BusyContent;
        if (content is null)
            return;

        BusyContent = null;
        BusyContent = content;
    }

    private static IReadOnlyList<IDataTemplate> GetDefaultBusyContentTemplates() =>
        _defaultBusyContentTemplates ??= LoadDefaultBusyContentTemplates();

    private static readonly string[] DefaultBusyContentTemplateKeys =
        [nameof(IndeterminateBusy), nameof(DeterminateBusy), nameof(ProgressionBusy)];

    private static List<IDataTemplate> LoadDefaultBusyContentTemplates()
    {
        var templates = new List<IDataTemplate>(DefaultBusyContentTemplateKeys.Length);

        foreach (var key in DefaultBusyContentTemplateKeys)
        {
            if (ApplicationResources.TryGetResource<IDataTemplate>(key) is { } template)
                templates.Add(template);
        }

        return templates;
    }

    private static void Post(Action action)
    {
        if (Dispatcher.UIThread.CheckAccess())
            action();
        else
            Dispatcher.UIThread.Post(action);
    }
}
