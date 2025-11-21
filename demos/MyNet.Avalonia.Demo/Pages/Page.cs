// -----------------------------------------------------------------------
// <copyright file="Page.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;
using MyNet.Avalonia.Demo.Helpers;
using MyNet.UI.Navigation.Models;
using MyNet.Utilities;
using MyNet.Utilities.Logging;
using PropertyChanged;

namespace MyNet.Avalonia.Demo.Pages;

[DoNotNotify]
internal abstract class Page : UserControl, INavigationPage
{
    private IDisposable? _navigationTimer;

    #region IsActive

    /// <summary>
    /// Provides IsActive Property.
    /// </summary>
    public static readonly StyledProperty<bool> IsActiveProperty = AvaloniaProperty.Register<Page, bool>(nameof(IsActive), true);

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets the IsActive property.
    /// </summary>
    public bool IsActive
    {
        get => GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    #endregion

    #region Navigation

    public Type? GetParentPageType() => null;

    public void OnNavigated(NavigationContext navigationContext)
    {
        _navigationTimer?.Dispose();
        _navigationTimer = null;

        Equals(DataContext, this).IfFalse(() => (DataContext as INavigationPage)?.OnNavigated(navigationContext));
    }

    public void OnNavigatingFrom(NavigatingContext navigatingContext) => Equals(DataContext, this).IfFalse(() => (DataContext as INavigationPage)?.OnNavigatingFrom(navigatingContext));

    public void OnNavigatingTo(NavigatingContext navigatingContext)
    {
        _navigationTimer = PerformanceMonitor.Measure($"Page {GetType().Name} - Total Navigation");

        if (Equals(navigatingContext.OldPage, this))
        {
            navigatingContext.Cancel = true;
        }

        Equals(DataContext, this).IfFalse(() => (DataContext as INavigationPage)?.OnNavigatingTo(navigatingContext));
    }

    #endregion

    protected override void OnLoaded(RoutedEventArgs e)
    {
        using (PerformanceMonitor.Measure($"Page {GetType().Name} - OnLoaded"))
        {
            base.OnLoaded(e);
        }

        var controlCount = this.GetVisualDescendants().Count();
        LogManager.Debug($"FormsPage - Total controls instantiated: {controlCount}");
    }
}
