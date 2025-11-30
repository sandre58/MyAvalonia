// -----------------------------------------------------------------------
// <copyright file="IComponentTimeSelector.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130 // Namespace does not match folder structure
using System;
using Avalonia.Input;

namespace MyNet.Avalonia.Controls.Primitives;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public interface IComponentTimeSelector : IInputElement
{
    event EventHandler<ValueChangedEventArgs<int>>? ValueChanged;

    int Minimum { get; }

    int Maximum { get; }

    int StepFrequency { get; }

    int? Value { get; set; }
}
