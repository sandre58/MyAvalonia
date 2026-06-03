// -----------------------------------------------------------------------
// <copyright file="AvaloniaCommandFactory.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MyNet.Avalonia.Extended.Schedulers;
using MyNet.UI.Commands;
using MyNet.UI.Threading;

namespace MyNet.Avalonia.Extended.Commands;

/// <summary>
/// Avalonia implementation of <see cref="ICommandFactory"/> that raises <see cref="ICommand.CanExecuteChanged"/>
/// on the UI dispatcher via an injected <see cref="ISchedulerProvider"/>.
/// </summary>
/// <param name="schedulerProvider">The scheduler provider used for UI-thread notifications.</param>
public sealed class AvaloniaCommandFactory(ISchedulerProvider schedulerProvider) : ICommandFactory
{
    private readonly RelayCommandFactory _inner = new(schedulerProvider ?? throw new ArgumentNullException(nameof(schedulerProvider)));

    /// <inheritdoc />
    public ICommand Create(Action execute) => _inner.Create(execute);

    /// <inheritdoc />
    public ICommand Create(Action execute, Func<bool> canExecute) => _inner.Create(execute, canExecute);

    /// <inheritdoc />
    public ICommand Create<T>(Action<T?> execute) => _inner.Create(execute);

    /// <inheritdoc />
    public ICommand Create<T>(Action<T?> execute, Func<T?, bool> canExecute) => _inner.Create(execute, canExecute);

    /// <inheritdoc />
    public ICommand Create(Func<Task> execute) => _inner.Create(execute);

    /// <inheritdoc />
    public ICommand Create(Func<Task> execute, Func<bool> canExecute) => _inner.Create(execute, canExecute);

    /// <inheritdoc />
    public ICommand Create<T>(Func<T?, Task> execute) => _inner.Create(execute);

    /// <inheritdoc />
    public ICommand Create<T>(Func<T?, Task> execute, Func<T?, bool> canExecute) => _inner.Create(execute, canExecute);
}
