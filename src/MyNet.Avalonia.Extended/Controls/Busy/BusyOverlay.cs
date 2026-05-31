// -----------------------------------------------------------------------
// <copyright file="BusyOverlay.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using MyNet.UI.Loading;
using MyNet.UI.Loading.Models;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Extended.Controls;
#pragma warning restore IDE0130

/// <summary>
/// Full-surface overlay bound to an application-wide <see cref="IBusyService"/>.
/// </summary>
public sealed class BusyOverlay : TemplatedControl
{
    private IBusyService? _subscribedService;

    /// <summary>
    /// Defines the <see cref="BusyService"/> property.
    /// </summary>
    public static readonly StyledProperty<IBusyService?> BusyServiceProperty =
        AvaloniaProperty.Register<BusyOverlay, IBusyService?>(nameof(BusyService));

    /// <summary>
    /// Defines the <see cref="IsOpen"/> property.
    /// </summary>
    public static readonly DirectProperty<BusyOverlay, bool> IsOpenProperty =
        AvaloniaProperty.RegisterDirect<BusyOverlay, bool>(
            nameof(IsOpen),
            o => o.IsOpen,
            (o, v) => o.IsOpen = v);

    /// <summary>
    /// Defines the <see cref="ActiveBusy"/> property.
    /// </summary>
    public static readonly DirectProperty<BusyOverlay, IBusy?> ActiveBusyProperty =
        AvaloniaProperty.RegisterDirect<BusyOverlay, IBusy?>(
            nameof(ActiveBusy),
            o => o.ActiveBusy,
            (o, v) => o.ActiveBusy = v);

    static BusyOverlay()
    {
        IsHitTestVisibleProperty.OverrideDefaultValue<BusyOverlay>(false);
        IsVisibleProperty.OverrideDefaultValue<BusyOverlay>(false);
    }

    /// <summary>
    /// Gets or sets the application-wide busy service to observe.
    /// </summary>
    public IBusyService? BusyService
    {
        get => GetValue(BusyServiceProperty);
        set => SetValue(BusyServiceProperty, value);
    }

    /// <summary>
    /// Gets a value indicating whether the overlay is visible.
    /// </summary>
    public bool IsOpen
    {
        get;
        private set
        {
            if (SetAndRaise(IsOpenProperty, ref field, value))
            {
                IsVisible = value;
                IsHitTestVisible = value;
                PseudoClasses.Set(":open", value);
            }
        }
    }

    /// <summary>
    /// Gets the top-most busy indicator currently displayed.
    /// </summary>
    public IBusy? ActiveBusy
    {
        get;
        private set => SetAndRaise(ActiveBusyProperty, ref field, value);
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
        ActiveBusy = isBusy ? service?.CurrentBusy : null;
    }
}
