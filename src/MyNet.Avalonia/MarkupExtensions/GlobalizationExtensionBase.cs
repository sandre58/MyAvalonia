// -----------------------------------------------------------------------
// <copyright file="GlobalizationExtensionBase.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using MyNet.Utilities.Localization;

namespace MyNet.Avalonia.MarkupExtensions;

public abstract class GlobalizationExtensionBase<T> : MarkupExtension
    where T : BindingBase
{
    protected GlobalizationExtensionBase(bool updateOnCultureChanged, bool updateOnTimeZoneChanged)
    {
        ResourceLocator.Initialize();
        UpdateOnCultureChanged = updateOnCultureChanged;
        UpdateOnTimeZoneChanged = updateOnTimeZoneChanged;
    }

    protected EventHandler? UpdateTargetHandler { get; private set; }

    protected T Binding
    {
        get
        {
            field ??= CreateBinding();

            return field;
        }
    }

    protected abstract T CreateBinding();

    public object? TargetNullValue { get => Binding.TargetNullValue; set => Binding.TargetNullValue = value; }

    public object? FallbackValue { get => Binding.FallbackValue; set => Binding.FallbackValue = value; }

    public bool UpdateOnCultureChanged { get; set; }

    public bool UpdateOnTimeZoneChanged { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var targetProvider = (IProvideValueTarget?)serviceProvider.GetService(typeof(IProvideValueTarget));
        var targetElement = (AvaloniaObject?)targetProvider?.TargetObject;
        var targetProperty = (AvaloniaProperty?)targetProvider?.TargetProperty;

        if (targetElement is null || targetProperty is null) return Binding;

        UpdateTargetHandler = handler;

        if (UpdateOnCultureChanged)
            GlobalizationService.Current.CultureChanged += UpdateTargetHandler;

        if (UpdateOnTimeZoneChanged)
            GlobalizationService.Current.TimeZoneChanged += UpdateTargetHandler;

        return Binding;

        void handler(object? o, EventArgs e)
        {
            var expression = BindingOperations.GetBindingExpressionBase(targetElement, targetProperty);

            if (expression is null)
            {
                return;
            }

            var wr = new WeakReference<BindingExpressionBase>(expression);
            if (wr.TryGetTarget(out var target))
            {
                target.UpdateTarget();
            }
            else
            {
                GlobalizationService.Current.CultureChanged -= UpdateTargetHandler;
                GlobalizationService.Current.TimeZoneChanged -= UpdateTargetHandler;
            }
        }
    }
}
