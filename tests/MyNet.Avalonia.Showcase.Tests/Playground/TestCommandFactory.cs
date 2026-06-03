// -----------------------------------------------------------------------
// <copyright file="TestCommandFactory.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MyNet.Avalonia.Commands;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.Tests.Playground;

internal sealed class TestCommandFactory : ICommandFactory
{
    public ICommand Create(Action execute) => ActionCommand.Create(execute);

    public ICommand Create(Action execute, Func<bool> canExecute) => ActionCommand.Create(execute, canExecute);

    public ICommand Create<T>(Action<T?> execute) => ActionCommand.Create(execute);

    public ICommand Create<T>(Action<T?> execute, Func<T?, bool> canExecute) => ActionCommand.Create(execute, canExecute);

    public ICommand Create(Func<Task> execute) => ActionCommand.Create(execute);

    public ICommand Create(Func<Task> execute, Func<bool> canExecute) => ActionCommand.Create(execute, canExecute);

    public ICommand Create<T>(Func<T?, Task> execute) => ActionCommand.Create(execute);

    public ICommand Create<T>(Func<T?, Task> execute, Func<T?, bool> canExecute) => ActionCommand.Create(execute, canExecute);
}
