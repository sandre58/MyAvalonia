// -----------------------------------------------------------------------
// <copyright file="ActionCommand.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyNet.Avalonia.Commands;

/// <summary>
/// Lightweight <see cref="ICommand"/> factory for UI plumbing (controls, assists, theme defaults).
/// </summary>
public static class ActionCommand
{
    public static ICommand Create(Action execute) => new SyncCommand(execute);

    public static ICommand Create(Action execute, Func<bool> canExecute) => new SyncCommand(execute, canExecute);

    public static ICommand Create(Func<Task> execute) => new AsyncCommand(execute);

    public static ICommand Create(Func<Task> execute, Func<bool> canExecute) => new AsyncCommand(execute, canExecute);

    public static ICommand Create<T>(Action<T?> execute) => new SyncCommand<T>(execute);

    public static ICommand Create<T>(Action<T?> execute, Func<T?, bool> canExecute) => new SyncCommand<T>(execute, canExecute);

    public static ICommand Create<T>(Func<T?, Task> execute) => new AsyncCommand<T>(execute);

    public static ICommand Create<T>(Func<T?, Task> execute, Func<T?, bool> canExecute) => new AsyncCommand<T>(execute, canExecute);

    private abstract class CommandBase : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add { }
            remove { }
        }

        public abstract bool CanExecute(object? parameter);

        public abstract void Execute(object? parameter);
    }

    private sealed class SyncCommand(Action execute, Func<bool>? canExecute = null) : CommandBase
    {
        public override bool CanExecute(object? parameter) => canExecute?.Invoke() ?? true;

        public override void Execute(object? parameter) => execute();
    }

    private sealed class AsyncCommand(Func<Task> execute, Func<bool>? canExecute = null) : CommandBase
    {
        public override bool CanExecute(object? parameter) => canExecute?.Invoke() ?? true;

        public override void Execute(object? parameter) => _ = execute();
    }

    private sealed class SyncCommand<T>(Action<T?> execute, Func<T?, bool>? canExecute = null) : CommandBase
    {
        public override bool CanExecute(object? parameter) =>
            TryConvert(parameter, out var value) && (canExecute?.Invoke(value) ?? true);

        public override void Execute(object? parameter)
        {
            if (TryConvert(parameter, out var value))
                execute(value);
        }

        private static bool TryConvert(object? parameter, out T? value)
        {
            switch (parameter)
            {
                case T typed:
                    value = typed;
                    return true;
                case null when default(T) is null:
                    value = default;
                    return true;
                default:
                    value = default;
                    return false;
            }
        }
    }

    private sealed class AsyncCommand<T>(Func<T?, Task> execute, Func<T?, bool>? canExecute = null) : CommandBase
    {
        public override bool CanExecute(object? parameter) =>
            TryConvert(parameter, out var value) && (canExecute?.Invoke(value) ?? true);

        public override void Execute(object? parameter)
        {
            if (TryConvert(parameter, out var value))
                _ = execute(value);
        }

        private static bool TryConvert(object? parameter, out T? value)
        {
            switch (parameter)
            {
                case T typed:
                    value = typed;
                    return true;
                case null when default(T) is null:
                    value = default;
                    return true;
                default:
                    value = default;
                    return false;
            }
        }
    }
}
