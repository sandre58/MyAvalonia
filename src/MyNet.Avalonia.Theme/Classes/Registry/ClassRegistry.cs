// -----------------------------------------------------------------------
// <copyright file="ClassRegistry.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reactive.Disposables;
using Avalonia;
using MyNet.Avalonia.Theme.Diagnostics;
using MyNet.Text;

namespace MyNet.Avalonia.Theme.Classes.Registry;

/// <summary>
/// Defines a registry for utility actions that can be associated with controls in the MyNet.Avalonia.Theme framework.
/// </summary>
public static class ClassRegistry
{
    private static readonly Dictionary<string, Func<StyledElement, IDisposable>> Registry = [];
    private static readonly HashSet<string> RegisteredClassNames = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Registers a utility action with the specified name.
    /// </summary>
    /// <param name="name">The name of the utility action. Cannot be null.</param>
    /// <param name="action">The action to associate with the specified name. Cannot be null.</param>
    public static void Register<TControl>(string name, Func<TControl, IDisposable> action)
        where TControl : StyledElement
    {
        var @class = NormalizeClassName(name);
        Registry[@class] = control => control is TControl typed ? action(typed) : Disposable.Empty;
        RegisteredClassNames.Add(@class);

        PerformanceMonitor.Debug($"Registered utility action for class '{@class}'", PerformanceCategory.Utilities);
    }

    /// <summary>
    /// Registers a utility action with the specified class.
    /// </summary>
    /// <param name="class">The class of the utility action. Cannot be null.</param>
    /// <param name="action">The action to associate with the specified class. Cannot be null.</param>
    public static void Register<TControl>(CssClass @class, Func<TControl, IDisposable> action)
        where TControl : StyledElement => Register(@class.ToString(), action);

    /// <summary>
    /// Registers a control for each value of the specified enumeration, using a given prefix and applying a custom
    /// action to each control and enumeration value pair.
    /// </summary>
    /// <remarks>This method iterates over all values of the specified enumeration type and registers a
    /// control for each value. It enables dynamic registration and configuration of controls based on enumeration
    /// values, which can be useful for scenarios such as theming or feature toggling.</remarks>
    /// <typeparam name="TEnum">The enumeration type whose values will be used to register controls. Must be a value type that implements the
    /// Enum interface.</typeparam>
    /// <typeparam name="TControl">The type of control to associate with each enumeration value.</typeparam>
    /// <param name="prefix">The prefix to use when registering each control. The method combines this prefix with each enumeration value to
    /// create a unique registration key.</param>
    /// <param name="apply">An action to perform for each registered control and enumeration value pair. The action receives the control and
    /// the corresponding enumeration value as parameters.</param>
    /// <param name="noneManagement">Specifies how to handle the "None" value in the enumeration. This parameter allows for flexible management of the "None" value.</param>
    /// <param name="noneReplacement">Specifies the replacement value to use when the "None" value is encountered and the management strategy involves renaming. This parameter is optional and defaults to an empty string.</param>
    public static void RegisterMany<TEnum, TControl>(string prefix, Func<TControl, TEnum, IDisposable> apply, NoneManagement noneManagement = NoneManagement.Add, string? noneReplacement = "")
        where TEnum : struct, Enum
        where TControl : StyledElement
    {
        foreach (var item in Enum.GetValues<TEnum>())
        {
            var className = $"{prefix}-{item}";

            if (Convert.ToInt32(item, CultureInfo.InvariantCulture) == 0)
            {
                switch (noneManagement)
                {
                    case NoneManagement.Remove:
                        continue;
                    case NoneManagement.Add:
                        break;
                    case NoneManagement.RenameWithoutPrefix:
                        className = noneReplacement ?? string.Empty;
                        break;
                    case NoneManagement.RenameWithPrefix:
                        className = $"{prefix}-{noneReplacement}";
                        break;
                    case NoneManagement.RenameWithOnlyPrefix:
                        className = prefix;
                        break;
                }
            }

            Register<TControl>(className, c => apply(c, item));
        }
    }

    /// <summary>
    /// Retrieves the action associated with the specified class name, if one exists.
    /// </summary>
    /// <remarks>Use this method to obtain an action registered for a particular class name. If the class name
    /// is not present in the registry, the method returns <see langword="null"/>.</remarks>
    /// <param name="cls">The name of the class for which to resolve the associated action. Cannot be null.</param>
    /// <returns>An <see cref="Action{Control}"/> that can be executed for the specified class name, or <see langword="null"/> if
    /// no action is associated.</returns>
    public static Func<StyledElement, IDisposable>? Resolve(string cls) => Registry.GetValueOrDefault(cls);

    /// <summary>
    /// Returns whether a registered utility class exists for the given class name.
    /// </summary>
    public static bool ContainsRegisteredClass(string className)
        => RegisteredClassNames.Contains(NormalizeClassName(className));

    internal static int RegisteredClassCount => RegisteredClassNames.Count;

    private static string NormalizeClassName(string name) => name.ToLowerInvariant();
}

/// <summary>
/// Represents the management strategy for handling the "None" value in a set of CSS classes or control variants. This enumeration defines
/// the possible actions that can be taken when encountering a "None" value, allowing for flexible handling of such cases.
/// </summary>
public enum NoneManagement
{
    Remove,

    Add,

    RenameWithPrefix,

    RenameWithoutPrefix,

    RenameWithOnlyPrefix
}
