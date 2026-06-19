// -----------------------------------------------------------------------
// <copyright file="PassthroughViewFactory.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.Avalonia.Controls;
using MyNet.UI.Locators.Factories;

namespace MyNet.Avalonia.Extended.Headless.Tests.Dialogs;

internal sealed class PassthroughViewFactory(object? view = null) : IViewFactory
{
    public object CreateView(Type viewModelType) => view ?? new ContentDialog();

    public TView CreateView<TViewModel, TView>()
        where TView : class
        where TViewModel : class => (CreateView(typeof(TViewModel)) as TView)!;
}
