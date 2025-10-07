// -----------------------------------------------------------------------
// <copyright file="ClassesAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Avalonia;
using Avalonia.Collections;
using MyNet.Avalonia.Extensions;

namespace MyNet.Avalonia.Controls.Assists;

public static class ClassesAssist
{
    public static readonly AttachedProperty<string> ClassesProperty = AvaloniaProperty.RegisterAttached<StyledElement, string>("Classes", typeof(ClassesAssist));

    public static readonly AttachedProperty<StyledElement> ClassSourceProperty = AvaloniaProperty.RegisterAttached<StyledElement, StyledElement>("ClassSource", typeof(ClassesAssist));

    static ClassesAssist()
    {
        _ = ClassesProperty.Changed.AddClassHandler<StyledElement>(OnClassesChanged);
        _ = ClassSourceProperty.Changed.AddClassHandler<StyledElement>(OnClassSourceChanged);
        _ = AddClassesProperty.Changed.AddClassHandler<StyledElement>(OnAddClassesChanged);
        _ = RemoveClassesProperty.Changed.AddClassHandler<StyledElement>(OnRemoveClassesChanged);
    }

    private static void OnClassSourceChanged(StyledElement arg1, AvaloniaPropertyChangedEventArgs arg2)
    {
        if (arg2.NewValue is not StyledElement styledElement) return;
        arg1.Classes.Clear();
        var nonPseudoClasses = styledElement.Classes.Where(c => !c.StartsWith(':'));
        arg1.Classes.AddRange(nonPseudoClasses);
        _ = styledElement.Classes.WeakSubscribe((o, _) => OnSourceClassesChanged(o, arg1));
    }

    private static void OnSourceClassesChanged(object? sender, StyledElement target)
    {
        if (sender is not AvaloniaList<string> classes) return;
        target.Classes.Clear();
        var nonPseudoClasses = classes.Where(c => !c.StartsWith(':'));
        target.Classes.AddRange(nonPseudoClasses);
    }

    public static void SetClasses(AvaloniaObject obj, string value) => obj.SetValue(ClassesProperty, value);

    public static string GetClasses(AvaloniaObject obj) => obj.GetValue(ClassesProperty);

    private static void OnClassesChanged(StyledElement sender, AvaloniaPropertyChangedEventArgs value)
    {
        var @class = value.GetNewValue<string?>();
        if (@class is null) return;
        sender.Classes.Clear();
        var classes = @class.Split([' '], StringSplitOptions.RemoveEmptyEntries);
        sender.Classes.AddRange(classes);
    }

    public static void SetClassSource(StyledElement obj, StyledElement value) => obj.SetValue(ClassSourceProperty, value);

    public static StyledElement GetClassSource(StyledElement obj) => obj.GetValue(ClassSourceProperty);

    #region AddClasses

    /// <summary>
    /// Provides AddClasses Property for attached ClassesAssist element.
    /// </summary>
    public static readonly AttachedProperty<string?> AddClassesProperty = AvaloniaProperty.RegisterAttached<StyledElement, string?>("AddClasses", typeof(ClassesAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="AddClassesProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="AddClassesProperty"/>.</param>
    public static void SetAddClasses(StyledElement element, string? value) => element.SetValue(AddClassesProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="AddClassesProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static string? GetAddClasses(StyledElement element) => element.GetValue(AddClassesProperty);

    private static void OnAddClassesChanged(StyledElement sender, AvaloniaPropertyChangedEventArgs value)
    {
        var @class = value.GetNewValue<string?>();
        if (@class is null) return;
        sender.AddClasses(@class);
    }

    #endregion

    #region RemoveClasses

    /// <summary>
    /// Provides RemoveClasses Property for attached ClassesAssist element.
    /// </summary>
    public static readonly AttachedProperty<string?> RemoveClassesProperty = AvaloniaProperty.RegisterAttached<StyledElement, string?>("RemoveClasses", typeof(ClassesAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="RemoveClassesProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="RemoveClassesProperty"/>.</param>
    public static void SetRemoveClasses(StyledElement element, string? value) => element.SetValue(RemoveClassesProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="RemoveClassesProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static string? GetRemoveClasses(StyledElement element) => element.GetValue(RemoveClassesProperty);

    private static void OnRemoveClassesChanged(StyledElement sender, AvaloniaPropertyChangedEventArgs value)
    {
        var @class = value.GetNewValue<string?>();
        if (@class is null) return;
        sender.RemoveClasses(@class);
    }

    #endregion
}
