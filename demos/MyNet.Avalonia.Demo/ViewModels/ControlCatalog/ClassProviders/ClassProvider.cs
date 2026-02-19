// -----------------------------------------------------------------------
// <copyright file="ClassProvider.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Observable;

namespace MyNet.Avalonia.Demo.ViewModels.ControlCatalog.ClassProviders;

internal sealed class ClassProvider(string defaultClass) : ObservableObject, IClassProvider
{
    public string? SelectedClass { get; set; } = defaultClass;
}
