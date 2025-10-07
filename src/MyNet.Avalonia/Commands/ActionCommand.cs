// -----------------------------------------------------------------------
// <copyright file="ActionCommand.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyNet.Avalonia.Commands;

public sealed class ActionCommand<T> : ActionCommand, ICommand
{
    private readonly Action<T>? _cb;
    private readonly Func<T, Task>? _acb;

    public ActionCommand(Action<T> cb) => _cb = cb;

    public ActionCommand(Func<T, Task> cb) => _acb = cb;

    private bool Busy
    {
        get;
        set
        {
            field = value;
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public override event EventHandler? CanExecuteChanged;

    public override bool CanExecute(object? parameter) => !Busy;

    public override async void Execute(object? parameter)
    {
        if (Busy)
            return;
        try
        {
            Busy = true;
            if (_cb != null)
                _cb((T)parameter!);
            else
                await _acb!((T)parameter!).ConfigureAwait(false);
        }
        finally
        {
            Busy = false;
        }
    }
}

public abstract class ActionCommand : ICommand
{
    public static ActionCommand Create(Action cb) => new ActionCommand<object>(_ => cb());

    public static ActionCommand Create<TArg>(Action<TArg> cb) => new ActionCommand<TArg>(cb);

    public static ActionCommand CreateFromTask(Func<Task> cb) => new ActionCommand<object>(_ => cb());

    public abstract bool CanExecute(object? parameter);

    public abstract void Execute(object? parameter);

    public abstract event EventHandler? CanExecuteChanged;
}
