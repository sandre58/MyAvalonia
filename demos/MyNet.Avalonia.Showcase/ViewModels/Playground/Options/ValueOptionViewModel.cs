// -----------------------------------------------------------------------
// <copyright file="ValueOptionViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Specialized;
using System.Reactive.Subjects;
using DynamicData.Binding;
using Material.Icons;
using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;
using MyNet.Observable;
using MyNet.Utilities;

namespace MyNet.Avalonia.Showcase.ViewModels.Playground.Options;

/// <summary>
/// Provides a base view model for a setting, which includes a display name and a value. The generic version allows for strongly-typed values, while the non-generic version can be used for settings where the type is not known at compile time.
/// </summary>
/// <typeparam name="T">The type of the value for the setting.</typeparam>
/// <param name="definition">The control option definition associated with this setting.</param>
/// <param name="defaultValue">The initial value for the setting.</param>
/// <param name="displayNameFunc">A function that provides the display name for the setting.</param>
/// <param name="icon">An optional icon associated with the setting, which can be used for visual representation in the user interface. The icon can be of any type, such as a string representing a resource path, an image object, or any other relevant representation depending on the UI framework being used. This property allows for enhanced visual cues when displaying the setting in the UI, making it easier for users to identify and understand the purpose of the setting at a glance.</param>
internal abstract class ValueOptionViewModel<T>(IControlOptionDefinition definition, object? defaultValue, IProvideValue<string> displayNameFunc, MaterialIconKind? icon = null) : ValueOptionViewModel(definition, defaultValue, displayNameFunc, icon);

/// <summary>
/// Represents an abstract base class for editable settings that provides a display name for use in user interfaces.
/// </summary>
/// <remarks>Inherit from this class to implement view models for settings that can be edited and require a
/// user-friendly display name. The display name is intended for display purposes in UI components such as forms or
/// property grids.</remarks>
internal abstract class ValueOptionViewModel : OptionViewModel
{
    private readonly object? _defaultValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValueOptionViewModel"/> class with the specified control option definition, display name provider, and optional icon. The constructor sets up the necessary properties for the view model, including the control option definition that provides metadata and configuration for the setting, the display name provider that supplies a user-friendly name for the setting in the UI, and an optional icon that can be used for visual representation. The value of the setting is initialized to the default value defined in the control option definition, allowing for a consistent starting state when the view model is created.
    /// </summary>
    /// <param name="definition">The control option definition associated with this setting.</param>
    /// <param name="defaultValue">The initial value for the setting.</param>
    /// <param name="displayNameFunc">A provider that supplies the display name for the setting, used to present the setting in the UI. Cannot be null.</param>
    /// <param name="icon">An optional icon associated with the setting, which can be used for visual representation in the user interface. The icon can be of any type, such as a string representing a resource path, an image object, or any other relevant representation depending on the UI framework being used. This property allows for enhanced visual cues when displaying the setting in the UI, making it easier for users to identify and understand the purpose of the setting at a glance.</param>
    protected ValueOptionViewModel(IControlOptionDefinition definition, object? defaultValue, IProvideValue<string> displayNameFunc, MaterialIconKind? icon = null)
        : base(definition, displayNameFunc, icon)
    {
        _defaultValue = defaultValue;
        Value = defaultValue;

        Disposables.AddRange(
            [
                ValueChangedSubject,
                this.WhenPropertyChanged(x => x.Value).Subscribe(x => ValueChangedSubject.OnNext(x.Value))
            ]);

        if (Value is INotifyCollectionChanged observableCollection)
        {
            Disposables.Add(observableCollection.ObserveCollectionChanges().Subscribe(_ => ValueChangedSubject.OnNext(Value)));
        }
    }

    /// <summary>
    /// Gets or sets the value associated with the current instance.
    /// </summary>
    /// <remarks>This property can hold any object, and its value can be null. It is commonly used to store
    /// data that is dynamically determined at runtime.</remarks>
    public object? Value { get; set; }

    /// <summary>
    /// Gets the subject that represents a stream of boolean values.
    /// </summary>
    /// <remarks>This subject can be used to publish boolean values to subscribers, allowing for reactive
    /// programming patterns.</remarks>
    public Subject<object?> ValueChangedSubject { get; } = new();

    /// <summary>
    /// Resets the value to its default state.
    /// </summary>
    /// <remarks>This method sets the current value to the predefined default value, allowing for
    /// reinitialization of the state.</remarks>
    public void Reset() => Value = _defaultValue;
}
