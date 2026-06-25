// -----------------------------------------------------------------------
// <copyright file="BusyServiceIndicator.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using MyNet.Avalonia.Controls;
using MyNet.UI.Loading;
using MyNet.UI.Loading.Models;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Extended.Controls;
#pragma warning restore IDE0130

/// <summary>
/// <see cref="BusyIndicator"/> that mirrors state from an application-wide <see cref="IBusyService"/>.
/// </summary>
public sealed class BusyServiceIndicator : BusyIndicator
{
    private static readonly Uri BusyContentTemplatesUri =
        new("avares://MyNet.Avalonia.Extended/Themes/Controls/BusyServiceIndicator.DataTemplates.axaml");

    private static IReadOnlyList<IDataTemplate>? _busyContentTemplates;

    private IBusyService? _subscribedService;
    private ContentPresenter? _busyContentPresenter;
    private bool _busyContentTemplatesAttached;

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

    /// <inheritdoc />
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _busyContentPresenter = e.NameScope.Find<ContentPresenter>("PART_BusyContent");
        AttachBusyContentTemplates();

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
    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        Unsubscribe(_subscribedService);
        _subscribedService = null;
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

    private void AttachBusyContentTemplates()
    {
        if (_busyContentTemplatesAttached || _busyContentPresenter is null)
            return;

        foreach (var template in GetBusyContentTemplates())
            _busyContentPresenter.DataTemplates.Add(template);

        _busyContentTemplatesAttached = true;
    }

    private static IReadOnlyList<IDataTemplate> GetBusyContentTemplates() =>
        _busyContentTemplates ??= LoadBusyContentTemplates();

    private static readonly string[] BusyContentTemplateKeys =
        [nameof(IndeterminateBusy), nameof(DeterminateBusy), nameof(ProgressionBusy)];

    private static List<IDataTemplate> LoadBusyContentTemplates()
    {
        if (AvaloniaXamlLoader.Load(BusyContentTemplatesUri) is not ResourceDictionary resources)
            return [];

        var templates = new List<IDataTemplate>(BusyContentTemplateKeys.Length);

        foreach (var key in BusyContentTemplateKeys)
        {
            if (resources.TryGetResource(key, null, out var value) && value is IDataTemplate template)
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
