// -----------------------------------------------------------------------
// <copyright file="ClassesAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;

namespace MyNet.Avalonia.Theme.Assists;

public static class ClassesAssist
{
    #region Internal storage

    private sealed class Layer
    {
        public HashSet<string> Classes { get; } = new();
    }

    private static readonly AttachedProperty<Dictionary<string, Layer>> LayersProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, Dictionary<string, Layer>>(
            "Layers",
            typeof(ClassesAssist));

    private static Dictionary<string, Layer> GetLayers(StyledElement element)
    {
        var layers = element.GetValue(LayersProperty);
        if (layers == null)
        {
            layers = new Dictionary<string, Layer>();
            element.SetValue(LayersProperty, layers);
        }

        return layers;
    }

    private static readonly AttachedProperty<HashSet<string>> ManagedClassesProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, HashSet<string>>(
            "ManagedClasses",
            typeof(ClassesAssist));

    private static HashSet<string> GetManagedClasses(StyledElement element)
    {
        var set = element.GetValue(ManagedClassesProperty);
        if (set == null)
        {
            set = new HashSet<string>();
            element.SetValue(ManagedClassesProperty, set);
        }

        return set;
    }

    #endregion

    static ClassesAssist()
    {
        ClassesProperty.Changed.AddClassHandler<AvaloniaObject>(OnClassesChanged);
        AddClassesProperty.Changed.AddClassHandler<AvaloniaObject>(OnAddClassesChanged);
        RemoveClassesProperty.Changed.AddClassHandler<AvaloniaObject>(OnRemoveClassesChanged);
    }

    #region Classes (replace layer)

    public static readonly AttachedProperty<object?> ClassesProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, object?>(
            "Classes",
            typeof(ClassesAssist));

    public static void SetClasses(AvaloniaObject element, object? value)
        => element.SetValue(ClassesProperty, value);

    public static object? GetClasses(AvaloniaObject element)
        => element.GetValue(ClassesProperty);

    private static void OnClassesChanged(AvaloniaObject sender, AvaloniaPropertyChangedEventArgs args)
    {
        if (sender is not StyledElement element) return;

        var value = args.GetNewValue<object?>();
        SetLayer(element, "Replace", Extract(value));
    }

    #endregion

    #region AddClasses

    public static readonly AttachedProperty<object?> AddClassesProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, object?>(
            "AddClasses",
            typeof(ClassesAssist));

    public static void SetAddClasses(AvaloniaObject element, object? value)
        => element.SetValue(AddClassesProperty, value);

    public static object? GetAddClasses(AvaloniaObject element)
        => element.GetValue(AddClassesProperty);

    private static void OnAddClassesChanged(AvaloniaObject sender, AvaloniaPropertyChangedEventArgs args)
    {
        if (sender is not StyledElement element) return;

        var value = args.GetNewValue<object?>();
        SetLayer(element, "Add", Extract(value));
    }

    #endregion

    #region RemoveClasses

    public static readonly AttachedProperty<object?> RemoveClassesProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, object?>(
            "RemoveClasses",
            typeof(ClassesAssist));

    public static void SetRemoveClasses(AvaloniaObject element, object? value)
        => element.SetValue(RemoveClassesProperty, value);

    public static object? GetRemoveClasses(AvaloniaObject element)
        => element.GetValue(RemoveClassesProperty);

    private static void OnRemoveClassesChanged(AvaloniaObject sender, AvaloniaPropertyChangedEventArgs args)
    {
        if (sender is not StyledElement element) return;

        var value = args.GetNewValue<object?>();
        SetLayer(element, "Remove", Extract(value));
    }

    #endregion

    #region Core logic

    private static void SetLayer(StyledElement element, string name, IEnumerable<string> classes)
    {
        var layers = GetLayers(element);

        if (!layers.TryGetValue(name, out var layer))
        {
            layer = new Layer();
            layers[name] = layer;
        }

        layer.Classes.Clear();

        foreach (var c in classes)
            layer.Classes.Add(c);

        Rebuild(element);
    }

    private static void Rebuild(StyledElement element)
    {
        var layers = GetLayers(element);
        var managed = GetManagedClasses(element);

        var newManaged = new HashSet<string>();

        // Replace layer (prioritaire)
        if (layers.TryGetValue("Replace", out var replaceLayer))
        {
            foreach (var c in replaceLayer.Classes)
                newManaged.Add(c);
        }

        // Add layer
        if (layers.TryGetValue("Add", out var addLayer))
        {
            foreach (var c in addLayer.Classes)
                newManaged.Add(c);
        }

        // Remove layer
        if (layers.TryGetValue("Remove", out var removeLayer))
        {
            foreach (var c in removeLayer.Classes)
                newManaged.Remove(c);
        }

        // 🔥 Diff propre

        // Supprimer anciennes classes gérées
        foreach (var old in managed.ToList())
        {
            if (!newManaged.Contains(old))
            {
                element.Classes.Remove(old);
                managed.Remove(old);
            }
        }

        // Ajouter nouvelles
        foreach (var c in newManaged)
        {
            if (!managed.Contains(c))
            {
                element.Classes.Add(c);
                managed.Add(c);
            }
        }
    }

    private static IEnumerable<string> Extract(object? value)
    {
        if (value == null)
            return [];

        if (value is string s)
            return s.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (value is IEnumerable<string> enumerable)
            return enumerable.Where(x => !string.IsNullOrWhiteSpace(x));

        return [];
    }

    #endregion
}
