// -----------------------------------------------------------------------
// <copyright file="SpinnerBehavior.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Reflection;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace MyNet.Avalonia.Controls.Behaviors;

/// <summary>
/// Provides attached properties for <see cref="Spinner"/> controls to bind Increase/Decrease actions
/// to <see cref="ICommand"/> instances or ViewModel methods.
/// Mouse wheel and keyboard navigation are handled separately by <see cref="InputBehavior"/>,
/// which raises the <see cref="Spinner.SpinEvent"/> that this behavior listens to.
/// </summary>
public static class SpinnerBehavior
{
    private static readonly AttachedProperty<bool> IsSpinHookedProperty =
        AvaloniaProperty.RegisterAttached<Spinner, bool>("IsSpinHooked", typeof(SpinnerBehavior));

    static SpinnerBehavior()
    {
        IncreaseCommandProperty.Changed.AddClassHandler<Spinner>((s, _) => EnsureSpinHook(s));
        DecreaseCommandProperty.Changed.AddClassHandler<Spinner>((s, _) => EnsureSpinHook(s));
        IncreaseMethodProperty.Changed.AddClassHandler<Spinner>((s, _) => EnsureSpinHook(s));
        DecreaseMethodProperty.Changed.AddClassHandler<Spinner>((s, _) => EnsureSpinHook(s));

        // Re-resolve method-based commands when the DataContext changes.
        StyledElement.DataContextProperty.Changed.AddClassHandler<Spinner>((s, _) =>
        {
            RebuildMethodCommand(s, IncreaseMethodProperty, IncreaseCommandProperty);
            RebuildMethodCommand(s, DecreaseMethodProperty, DecreaseCommandProperty);
        });
    }

    #region IncreaseCommand

    /// <summary>
    /// Provides IncreaseCommand Property for attached SpinnerBehavior element.
    /// </summary>
    public static readonly AttachedProperty<ICommand?> IncreaseCommandProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, ICommand?>("IncreaseCommand", typeof(SpinnerBehavior));

    /// <summary>
    /// Accessor for Attached <see cref="IncreaseCommandProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set <see cref="IncreaseCommandProperty"/>.</param>
    public static void SetIncreaseCommand(StyledElement element, ICommand? value) => element.SetValue(IncreaseCommandProperty, value);

    /// <summary>
    /// Accessor for Attached <see cref="IncreaseCommandProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static ICommand? GetIncreaseCommand(StyledElement element) => element.GetValue(IncreaseCommandProperty);

    #endregion

    #region DecreaseCommand

    /// <summary>
    /// Provides DecreaseCommand Property for attached SpinnerBehavior element.
    /// </summary>
    public static readonly AttachedProperty<ICommand?> DecreaseCommandProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, ICommand?>("DecreaseCommand", typeof(SpinnerBehavior));

    /// <summary>
    /// Accessor for Attached <see cref="DecreaseCommandProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set <see cref="DecreaseCommandProperty"/>.</param>
    public static void SetDecreaseCommand(StyledElement element, ICommand? value) => element.SetValue(DecreaseCommandProperty, value);

    /// <summary>
    /// Accessor for Attached <see cref="DecreaseCommandProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static ICommand? GetDecreaseCommand(StyledElement element) => element.GetValue(DecreaseCommandProperty);

    #endregion

    #region IncreaseCommandParameter

    /// <summary>
    /// Provides IncreaseCommandParameter Property for attached SpinnerBehavior element.
    /// </summary>
    public static readonly AttachedProperty<object?> IncreaseCommandParameterProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, object?>("IncreaseCommandParameter", typeof(SpinnerBehavior));

    /// <summary>
    /// Accessor for Attached <see cref="IncreaseCommandParameterProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set <see cref="IncreaseCommandParameterProperty"/>.</param>
    public static void SetIncreaseCommandParameter(StyledElement element, object? value) => element.SetValue(IncreaseCommandParameterProperty, value);

    /// <summary>
    /// Accessor for Attached <see cref="IncreaseCommandParameterProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static object? GetIncreaseCommandParameter(StyledElement element) => element.GetValue(IncreaseCommandParameterProperty);

    #endregion

    #region DecreaseCommandParameter

    /// <summary>
    /// Provides DecreaseCommandParameter Property for attached SpinnerBehavior element.
    /// </summary>
    public static readonly AttachedProperty<object?> DecreaseCommandParameterProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, object?>("DecreaseCommandParameter", typeof(SpinnerBehavior));

    /// <summary>
    /// Accessor for Attached <see cref="DecreaseCommandParameterProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set <see cref="DecreaseCommandParameterProperty"/>.</param>
    public static void SetDecreaseCommandParameter(StyledElement element, object? value) => element.SetValue(DecreaseCommandParameterProperty, value);

    /// <summary>
    /// Accessor for Attached <see cref="DecreaseCommandParameterProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static object? GetDecreaseCommandParameter(StyledElement element) => element.GetValue(DecreaseCommandParameterProperty);

    #endregion

    #region IncreaseMethod

    /// <summary>
    /// Provides IncreaseMethod Property for attached SpinnerBehavior element.
    /// Name of the method to invoke on the DataContext when spinning up.
    /// Supports parameterless methods, methods with a typed parameter, and methods with an <see cref="object"/> parameter.
    /// </summary>
    public static readonly AttachedProperty<string?> IncreaseMethodProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, string?>("IncreaseMethod", typeof(SpinnerBehavior));

    /// <summary>
    /// Accessor for Attached <see cref="IncreaseMethodProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set <see cref="IncreaseMethodProperty"/>.</param>
    public static void SetIncreaseMethod(StyledElement element, string? value) => element.SetValue(IncreaseMethodProperty, value);

    /// <summary>
    /// Accessor for Attached <see cref="IncreaseMethodProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static string? GetIncreaseMethod(StyledElement element) => element.GetValue(IncreaseMethodProperty);

    #endregion

    #region DecreaseMethod

    /// <summary>
    /// Provides DecreaseMethod Property for attached SpinnerBehavior element.
    /// Name of the method to invoke on the DataContext when spinning down.
    /// Supports parameterless methods, methods with a typed parameter, and methods with an <see cref="object"/> parameter.
    /// </summary>
    public static readonly AttachedProperty<string?> DecreaseMethodProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, string?>("DecreaseMethod", typeof(SpinnerBehavior));

    /// <summary>
    /// Accessor for Attached <see cref="DecreaseMethodProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set <see cref="DecreaseMethodProperty"/>.</param>
    public static void SetDecreaseMethod(StyledElement element, string? value) => element.SetValue(DecreaseMethodProperty, value);

    /// <summary>
    /// Accessor for Attached <see cref="DecreaseMethodProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static string? GetDecreaseMethod(StyledElement element) => element.GetValue(DecreaseMethodProperty);

    #endregion

    #region Spin Event Wiring

    private static void EnsureSpinHook(Spinner spinner)
    {
        if (spinner.GetValue(IsSpinHookedProperty))
            return;

        spinner.AddHandler(Spinner.SpinEvent, OnSpin, RoutingStrategies.Bubble);
        spinner.SetValue(IsSpinHookedProperty, true);

        RebuildMethodCommand(spinner, IncreaseMethodProperty, IncreaseCommandProperty);
        RebuildMethodCommand(spinner, DecreaseMethodProperty, DecreaseCommandProperty);
    }

    private static void OnSpin(object? sender, SpinEventArgs e)
    {
        if (sender is not Spinner spinner)
            return;

        var command = e.Direction == SpinDirection.Increase
            ? GetIncreaseCommand(spinner)
            : GetDecreaseCommand(spinner);

        var parameter = e.Direction == SpinDirection.Increase
            ? GetIncreaseCommandParameter(spinner)
            : GetDecreaseCommandParameter(spinner);

        if (command?.CanExecute(parameter) == true)
        {
            command.Execute(parameter);
            e.Handled = true;
        }
    }

    #endregion

    #region Method Command Resolution

    private static void RebuildMethodCommand(Spinner spinner, AttachedProperty<string?> methodProperty, AttachedProperty<ICommand?> commandProperty)
    {
        var methodName = spinner.GetValue(methodProperty);
        if (string.IsNullOrWhiteSpace(methodName))
            return;

        // Do not overwrite an explicitly provided ICommand (only replace our own SpinnerMethodCommand).
        var existing = spinner.GetValue(commandProperty);
        if (existing is not null and not SpinnerMethodCommand)
            return;

        spinner.SetValue(commandProperty, new SpinnerMethodCommand(spinner, methodName));
    }

    private sealed class SpinnerMethodCommand(Spinner spinner, string methodName) : ICommand
    {
        private readonly WeakReference<Spinner> _spinnerRef = new(spinner);

        public event EventHandler? CanExecuteChanged
        {
            add { }
            remove { }
        }

        public bool CanExecute(object? parameter) => ResolveMethod(parameter) is not null;

        public void Execute(object? parameter)
        {
            var method = ResolveMethod(parameter);
            if (method is null || !_spinnerRef.TryGetTarget(out var target) || target.DataContext is null)
                return;

            _ = method.GetParameters().Length == 0
                ? method.Invoke(target.DataContext, null)
                : method.Invoke(target.DataContext, [parameter]);
        }

        private MethodInfo? ResolveMethod(object? parameter)
        {
            if (!_spinnerRef.TryGetTarget(out var target) || target.DataContext is null)
                return null;

            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var vmType = target.DataContext.GetType();

            // 1. Prefer parameterless.
            var parameterless = vmType.GetMethod(methodName, flags, null, [], null);
            if (parameterless is not null)
                return parameterless;

            // 2. Try exact parameter type, then fall back to object.
            var paramType = parameter?.GetType() ?? typeof(object);
            return vmType.GetMethod(methodName, flags, null, [paramType], null)
                ?? vmType.GetMethod(methodName, flags, null, [typeof(object)], null);
        }
    }

    #endregion
}
