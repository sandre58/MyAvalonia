// -----------------------------------------------------------------------
// <copyright file="ClassesAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Collections;
using MyNet.Utilities;

namespace MyNet.Avalonia.Theme.Assists;

public static class ClassesAssist
{
    public static readonly AttachedProperty<object> ClassesProperty = AvaloniaProperty.RegisterAttached<StyledElement, object>("Classes", typeof(ClassesAssist));

    public static readonly AttachedProperty<StyledElement> ClassSourceProperty = AvaloniaProperty.RegisterAttached<StyledElement, StyledElement>("ClassSource", typeof(ClassesAssist));

    static ClassesAssist()
    {
        _ = ClassesProperty.Changed.AddClassHandler<StyledElement>(OnClassesChanged);
        _ = ClassSourceProperty.Changed.AddClassHandler<StyledElement>(OnClassSourceChanged);
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

    public static void SetClasses(AvaloniaObject obj, object value) => obj.SetValue(ClassesProperty, value);

    public static object GetClasses(AvaloniaObject obj) => obj.GetValue(ClassesProperty);

    private static void OnClassesChanged(StyledElement sender, AvaloniaPropertyChangedEventArgs value)
    {
        var classes = value.NewValue is IEnumerable<string> classesEnumerable ? classesEnumerable.ToArray() : (value.NewValue as string)?.Split([' '], StringSplitOptions.RemoveEmptyEntries);
        if (classes is null) return;
        classes.ForEach(x => sender.Classes.Set(x, true));
    }

    public static void SetClassSource(StyledElement obj, StyledElement value) => obj.SetValue(ClassSourceProperty, value);

    public static StyledElement GetClassSource(StyledElement obj) => obj.GetValue(ClassSourceProperty);
}
