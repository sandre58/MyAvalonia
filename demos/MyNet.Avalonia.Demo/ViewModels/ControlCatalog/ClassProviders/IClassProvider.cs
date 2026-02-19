// -----------------------------------------------------------------------
// <copyright file="IClassProvider.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;

namespace MyNet.Avalonia.Demo.ViewModels.ControlCatalog.ClassProviders;

internal interface IClassProvider : INotifyPropertyChanged
{
    string? SelectedClass { get; }
}
