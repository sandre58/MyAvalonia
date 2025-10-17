// -----------------------------------------------------------------------
// <copyright file="WindowDialogManager.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyNet.Avalonia.UI.Dialogs;
using MyNet.UI.Dialogs;
using MyNet.UI.Dialogs.ContentDialogs;
using MyNet.UI.Locators;
using MyNet.Utilities.Messaging;

namespace MyNet.Avalonia.Demo.Services;

/// <summary>
/// Provides methods and properties to display a window dialog.
/// </summary>
public static class WindowDialogManager
{
    #region Fields

    private static IViewResolver? _viewResolver;
    private static IViewLocator? _viewLocator;
    private static IViewModelLocator? _viewModelLocator;

    #endregion Fields

    #region Members

    public static IList<IDialogViewModel>? OpenedDialogs => DialogService?.OpenedDialogs;

    public static bool HasOpenedDialogs => OpenedDialogs?.Any() == true;

    public static WindowDialogService DialogService { get; } = new();

    #endregion Members

    public static void Initialize(
        IViewResolver viewResolver,
        IViewLocator viewLocator,
        IViewModelLocator viewModelLocator) => (_viewResolver, _viewLocator, _viewModelLocator) = (viewResolver, viewLocator, viewModelLocator);

    #region Show

    /// <summary>
    /// Displays a modal dialog.
    /// </summary>
    /// <typeparam name="T">The type of the dialog view model.</typeparam>
    /// <param name="closeAction">An optional action to execute when the dialog is closed.</param>
    public static async Task ShowAsync<T>(Action<T>? closeAction = null)
        where T : class, IDialogViewModel
    {
        var vm = GetViewModel<T>();

        if (vm != null)
            await ShowAsync(vm, closeAction).ConfigureAwait(false);
    }

    /// <summary>
    /// Displays a modal dialog.
    /// </summary>
    /// <param name="typeViewModel">The type of the dialog view model to display.</param>
    /// <param name="closeAction">An optional action to execute when the dialog is closed.</param>
    public static async Task ShowAsync(Type typeViewModel, Action<IDialogViewModel>? closeAction = null)
    {
        var vm = GetViewModel<IDialogViewModel>(typeViewModel);

        if (vm != null)
            await ShowAsync(vm, closeAction).ConfigureAwait(false);
    }

    /// <summary>
    /// Displays a message dialog.
    /// </summary>
    /// <typeparam name="T">The type of the dialog view model.</typeparam>
    /// <param name="viewModel">The dialog view model instance to display.</param>
    /// <param name="closeAction">An optional action to execute when the dialog is closed.</param>
    public static async Task ShowAsync<T>(T viewModel, Action<T>? closeAction = null)
        where T : IDialogViewModel
    {
        if (DialogService is null) return;

        var view = GetViewFromViewModel(viewModel.GetType());

        if (view != null)
        {
            if (closeAction is not null)
                viewModel.CloseRequest += (sender, e) => closeAction(viewModel);

            Messenger.Default?.Send(new OpenDialogMessage(DialogType.Dialog, viewModel));

            await DialogService.ShowAsync(view, viewModel).ConfigureAwait(false);
        }
    }

    #endregion Show

    #region ShowDialog

    /// <summary>
    /// Displays a modal dialog.
    /// </summary>
    public static async Task<bool?> ShowDialogAsync<TViewModel>()
        where TViewModel : class, IDialogViewModel
        => await ShowDialogAsync(typeof(TViewModel)).ConfigureAwait(false);

    /// <summary>
    /// Displays a modal dialog.
    /// </summary>
    /// <param name="typeViewModel">The view to include in workspace dialog.</param>
    public static async Task<bool?> ShowDialogAsync(Type typeViewModel) => GetViewModel<IDialogViewModel>(typeViewModel) is not IDialogViewModel vm ? false : await ShowDialogAsync(vm).ConfigureAwait(false);

    /// <summary>
    /// Displays a message dialog.
    /// </summary>
    /// <param name="viewModel">The view to include in workspace dialog.</param>
    public static async Task<bool?> ShowDialogAsync<T>(T viewModel)
        where T : IDialogViewModel
    {
        if (DialogService is null) return null;

        Messenger.Default?.Send(new OpenDialogMessage(DialogType.ModalDialog, viewModel));

        var view = GetViewFromViewModel(viewModel.GetType());

        return view is null ? false : await DialogService.ShowModalAsync(view, viewModel).ConfigureAwait(false);
    }

    #endregion ShowDialog

    private static T? GetViewModel<T>()
        where T : class
        => GetViewModel<T>(typeof(T));

    private static T? GetViewModel<T>(Type typeViewModel)
        where T : class
        => (T?)_viewModelLocator?.Get(typeViewModel);

    private static object? GetViewFromViewModel(Type viewModelType)
    {
        var viewType = _viewResolver?.Resolve(viewModelType);

        if (viewType == null) throw new InvalidOperationException($"{viewType} has not been resolved.");

        var view = _viewLocator?.Get(viewType);

        return view;
    }
}
