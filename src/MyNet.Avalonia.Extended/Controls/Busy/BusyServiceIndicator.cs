// -----------------------------------------------------------------------
// <copyright file="BusyServiceIndicator.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using Avalonia;
using MyNet.Avalonia.Controls;
using MyNet.UI.Loading;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Extended.Controls;
#pragma warning restore IDE0130

/// <summary>
/// <see cref="BusyIndicator"/> that mirrors state from an application-wide <see cref="IBusyService"/>.
/// </summary>
public sealed class BusyServiceIndicator : BusyIndicator
{
    private IBusyService? _subscribedService;

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

        SyncFromService(_subscribedService);
    }

    private void SyncFromService(IBusyService? service)
    {
        var isBusy = service?.IsBusy ?? false;
        IsOpen = isBusy;
        BusyContent = isBusy ? service?.CurrentBusy : null;
    }
}
