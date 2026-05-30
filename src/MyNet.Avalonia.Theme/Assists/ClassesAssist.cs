// -----------------------------------------------------------------------
// <copyright file="ClassesAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using MyNet.Avalonia.Theme.Classes.Engine;

namespace MyNet.Avalonia.Theme.Assists;

/// <summary>
/// Defines attached properties for managing CSS-like classes on Avalonia controls with support for replacing, adding, and removing classes in a layered manner.
/// </summary>
public static class ClassesAssist
{
    #region Layer class

    /// <summary>
    /// Represents a layer that contains a collection of unique class names.
    /// </summary>
    /// <remarks>The collection of class names is stored as a hash set to ensure that each class name is
    /// unique within the layer and to provide efficient lookup operations.</remarks>
    private sealed class Layer
    {
        /// <summary>
        /// Gets the collection of unique class names associated with the current instance.
        /// </summary>
        /// <remarks>The returned collection can be used to add or remove class names as needed. Modifying
        /// this collection affects the set of classes applied to the instance. The collection does not allow duplicate
        /// class names.</remarks>
        public HashSet<string> Classes { get; } = [];
    }

    /// <summary>
    /// Identifies an attached property that stores a dictionary of layers associated with a styled element.
    /// </summary>
    /// <remarks>This property enables the association of multiple visual layers with a single styled element,
    /// allowing for advanced visual composition scenarios. The dictionary uses string keys to identify individual
    /// layers. Before accessing or modifying the dictionary, ensure it is initialized to avoid null reference
    /// exceptions.</remarks>
    private static readonly AttachedProperty<Dictionary<string, Layer>?> LayersProperty = AvaloniaProperty.RegisterAttached<StyledElement, Dictionary<string, Layer>?>("Layers", typeof(ClassesAssist));

    /// <summary>
    /// Gets the dictionary of layers associated with the specified styled element. If the dictionary does not exist, it initializes a new one and associates it with the element.
    /// </summary>
    /// <param name="element">The styled element for which to retrieve the layers.</param>
    /// <returns>A dictionary of layers associated with the specified styled element.</returns>
    private static Dictionary<string, Layer> GetLayers(StyledElement element)
    {
        var layers = element.GetValue(LayersProperty);
        if (layers == null)
        {
            layers = [];
            element.SetValue(LayersProperty, layers);
        }

        return layers;
    }

    /// <summary>
    /// Identifies an attached property that stores a set of managed class names for a styled element.
    /// </summary>
    /// <remarks>This property enables associating multiple class names with a styled element, which can be
    /// used to apply dynamic or conditional styling. When using this property, ensure that the set of class names is
    /// managed consistently to maintain the intended visual appearance. The property is typically accessed through
    /// static methods that get or set its value on a target element.</remarks>
    private static readonly AttachedProperty<HashSet<string>?> ManagedClassesProperty = AvaloniaProperty.RegisterAttached<StyledElement, HashSet<string>?>("ManagedClasses", typeof(ClassesAssist));

    /// <summary>
    /// Gets the set of managed class names associated with the specified styled element.
    /// </summary>
    /// <remarks>If the managed classes have not been previously set, an empty HashSet is created and
    /// associated with the element.</remarks>
    /// <param name="element">The styled element for which to retrieve the managed class names. This parameter cannot be null.</param>
    /// <returns>A HashSet of strings containing the names of managed classes. If no managed classes are set, an empty HashSet is
    /// returned.</returns>
    private static HashSet<string> GetManagedClasses(StyledElement element)
    {
        var set = element.GetValue(ManagedClassesProperty);
        if (set == null)
        {
            set = [];
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
        UseRegisteredClassesProperty.Changed.AddClassHandler<AvaloniaObject, bool>(OnUseRegisteredClassesChanged);
    }

    #region Classes (replace layer)

    /// <summary>
    /// Identifies the attached property that enables dynamic assignment of CSS classes to a styled element.
    /// </summary>
    /// <remarks>This property allows developers to assign one or more CSS class names to an Avalonia
    /// StyledElement at runtime. Classes assigned through this property can be used to apply conditional styling,
    /// similar to the class attribute in HTML and CSS. The property is typically used in conjunction with style
    /// selectors that target specific class names.</remarks>
    public static readonly AttachedProperty<object?> ClassesProperty = AvaloniaProperty.RegisterAttached<StyledElement, object?>("Classes", typeof(ClassesAssist));

    /// <summary>
    /// Sets the CSS-style classes for the specified Avalonia object, enabling dynamic styling based on class
    /// membership.
    /// </summary>
    /// <remarks>This method updates the ClassesProperty of the specified element, which may affect the visual
    /// appearance and behavior of the control according to the applied styles. Changing the classes can trigger style
    /// updates and re-evaluation of style selectors.</remarks>
    /// <param name="element">The AvaloniaObject to which the classes will be applied. This parameter cannot be null.</param>
    /// <param name="value">An object representing the classes to assign to the element. This can be a string containing one or more class
    /// names, or an array of class names.</param>
    public static void SetClasses(AvaloniaObject element, object? value) => element.SetValue(ClassesProperty, value);

    /// <summary>
    /// Gets the value of the Classes property for the specified AvaloniaObject.
    /// </summary>
    /// <remarks>This method retrieves the current value of the Classes property, which is used to manage CSS
    /// classes for styling the element.</remarks>
    /// <param name="element">The AvaloniaObject from which to retrieve the Classes property value. This parameter cannot be null.</param>
    /// <returns>An object representing the value of the Classes property. This may be null if the property has not been set.</returns>
    public static object? GetClasses(AvaloniaObject element) => element.GetValue(ClassesProperty);

    /// <summary>
    /// Handles changes to the classes property of a styled element and updates the associated visual layer accordingly.
    /// </summary>
    /// <remarks>This method is typically used to respond to dynamic changes in the styling classes of UI
    /// elements, ensuring that the visual representation remains consistent with the current set of classes.</remarks>
    /// <param name="sender">The source object of the property change event. Must be a StyledElement for the update to occur.</param>
    /// <param name="args">An AvaloniaPropertyChangedEventArgs instance containing details about the property change, including the new
    /// value.</param>
    private static void OnClassesChanged(AvaloniaObject sender, AvaloniaPropertyChangedEventArgs args)
    {
        if (sender is not StyledElement element) return;

        var value = args.GetNewValue<object?>();
        SetLayer(element, "Replace", Extract(value));
    }

    #endregion

    #region AddClasses

    /// <summary>
    /// Identifies the attached property that enables associating additional CSS class names with a styled element for
    /// dynamic styling.
    /// </summary>
    /// <remarks>This property allows developers to add or remove CSS class names on a styled element at
    /// runtime, facilitating conditional or state-based styling in Avalonia applications. The value assigned should be
    /// a collection or string representing one or more class names. This property is typically used in XAML or
    /// code-behind to modify the visual appearance of controls without altering their templates.</remarks>
    public static readonly AttachedProperty<object?> AddClassesProperty = AvaloniaProperty.RegisterAttached<StyledElement, object?>("AddClasses", typeof(ClassesAssist));

    /// <summary>
    /// Sets the additional CSS-like classes for the specified Avalonia object, enabling dynamic styling or behavior
    /// changes at runtime.
    /// </summary>
    /// <remarks>The specified classes must be defined in the application's styles to have any visual effect.
    /// This method is typically used to modify the appearance or behavior of controls dynamically in response to
    /// application logic.</remarks>
    /// <param name="element">The AvaloniaObject to which the additional classes will be applied. Cannot be null.</param>
    /// <param name="value">An object representing the additional classes to add. This can be a string containing class names separated by
    /// spaces, or an array of class names. Can be null to clear previously set classes.</param>
    public static void SetAddClasses(AvaloniaObject element, object? value) => element.SetValue(AddClassesProperty, value);

    /// <summary>
    /// Gets the value of the AddClasses attached property for the specified AvaloniaObject.
    /// </summary>
    /// <remarks>Use this method to retrieve additional CSS class information applied to an Avalonia element
    /// via the AddClasses attached property.</remarks>
    /// <param name="element">The AvaloniaObject from which to retrieve the AddClasses property value. This parameter cannot be null.</param>
    /// <returns>The current value of the AddClasses property, or null if the property has not been set.</returns>
    public static object? GetAddClasses(AvaloniaObject element) => element.GetValue(AddClassesProperty);

    /// <summary>
    /// Handles changes to the 'Add' classes of a styled element and updates the associated styling layer accordingly.
    /// </summary>
    /// <remarks>This method is typically invoked when the 'Add' classes property of a StyledElement changes,
    /// allowing dynamic updates to the element's styling. It ensures that the styling layer reflects the current set of
    /// classes applied to the element.</remarks>
    /// <param name="sender">The source object of the property change event. Must be a StyledElement to apply class changes.</param>
    /// <param name="args">An AvaloniaPropertyChangedEventArgs instance containing information about the property change, including the new
    /// class values.</param>
    private static void OnAddClassesChanged(AvaloniaObject sender, AvaloniaPropertyChangedEventArgs args)
    {
        if (sender is not StyledElement element) return;

        var value = args.GetNewValue<object?>();
        SetLayer(element, "Add", Extract(value));
    }

    #endregion

    #region RemoveClasses

    /// <summary>
    /// Identifies the attached property that specifies the classes to be removed from a styled element.
    /// </summary>
    /// <remarks>This attached property enables dynamic modification of an element's styling by allowing
    /// specific classes to be removed at runtime. It is useful in scenarios where application logic determines which
    /// visual states or styles should no longer apply to a control. The value assigned should represent one or more
    /// class names to be removed from the target element's class list.</remarks>
    public static readonly AttachedProperty<object?> RemoveClassesProperty = AvaloniaProperty.RegisterAttached<StyledElement, object?>("RemoveClasses", typeof(ClassesAssist));

    /// <summary>
    /// Sets the value of the RemoveClasses attached property on the specified AvaloniaObject.
    /// </summary>
    /// <remarks>Setting this property may affect the visual appearance or behavior of the specified element,
    /// depending on how the RemoveClasses property is used within the application.</remarks>
    /// <param name="element">The AvaloniaObject on which to set the RemoveClasses property.</param>
    /// <param name="value">The value to assign to the RemoveClasses property. This value can be null.</param>
    public static void SetRemoveClasses(AvaloniaObject element, object? value) => element.SetValue(RemoveClassesProperty, value);

    /// <summary>
    /// Gets the value of the RemoveClasses attached property for the specified AvaloniaObject.
    /// </summary>
    /// <remarks>Use this method to query which CSS-like classes are marked for removal from the given
    /// element. This is typically used in styling scenarios to dynamically manage class lists.</remarks>
    /// <param name="element">The AvaloniaObject from which to retrieve the RemoveClasses property value. This parameter cannot be null.</param>
    /// <returns>The value of the RemoveClasses property, or null if the property is not set on the specified element.</returns>
    public static object? GetRemoveClasses(AvaloniaObject element) => element.GetValue(RemoveClassesProperty);

    /// <summary>
    /// Handles changes to the 'RemoveClasses' property on a styled element and updates the element's layer accordingly.
    /// </summary>
    /// <remarks>This method is typically used as a property changed callback to synchronize the visual layer
    /// of a styled element when its 'RemoveClasses' property changes.</remarks>
    /// <param name="sender">The object on which the property change occurred. Must be a StyledElement.</param>
    /// <param name="args">The event data containing information about the property change, including the new value of the 'RemoveClasses'
    /// property.</param>
    private static void OnRemoveClassesChanged(AvaloniaObject sender, AvaloniaPropertyChangedEventArgs args)
    {
        if (sender is not StyledElement element) return;

        var value = args.GetNewValue<object?>();
        SetLayer(element, "Remove", Extract(value));
    }

    #endregion

    #region UseRegisteredClasses

    /// <summary>
    /// Gets a collection of subscriptions for controls that have registered classes enabled. This collection is used to manage event subscriptions for controls that utilize the UseRegisteredClasses attached property, allowing for proper cleanup when the property is disabled.
    /// </summary>
    private static readonly ConditionalWeakTable<Control, IDisposable> Subscriptions = [];

    /// <summary>
    /// Identifies an attached property that stores the runtime state of the associated control's classes.
    /// </summary>
    /// <remarks>This property enables tracking and managing the runtime state of control classes within the
    /// Avalonia framework. Access to this property should be performed in a thread-safe manner, as it may be used
    /// across multiple threads.</remarks>
    private static readonly AttachedProperty<ClassesRuntimeState?> RuntimeStateProperty = AvaloniaProperty.RegisterAttached<Control, ClassesRuntimeState?>("ClassesRuntimeState", typeof(ClassesAssist));

    /// <summary>
    /// Provides UseRegisteredClasses Property for attached ClassesAssist element.
    /// </summary>
    public static readonly AttachedProperty<bool> UseRegisteredClassesProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("UseRegisteredClasses", typeof(ClassesAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="UseRegisteredClassesProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="UseRegisteredClassesProperty"/>.</param>
    public static void SetUseRegisteredClasses(StyledElement element, bool value) => element.SetValue(UseRegisteredClassesProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="UseRegisteredClassesProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetUseRegisteredClasses(StyledElement element) => element.GetValue(UseRegisteredClassesProperty);

    /// <summary>
    /// Handles changes to the enabled state of utility features for a control.
    /// </summary>
    /// <remarks>When utilities are enabled, this method subscribes to the control's class collection changes
    /// and triggers recompilation to ensure the control's appearance is updated accordingly.</remarks>
    /// <param name="sender">The object on which the property change occurred. Must be a StyledElement.</param>
    /// <param name="e">An event argument that provides information about the change in the enabled state, including the control and the
    /// new value.</param>
    private static void OnUseRegisteredClassesChanged(AvaloniaObject sender, AvaloniaPropertyChangedEventArgs<bool> e)
    {
        if (e.Sender is Control control)
        {
            if (e.GetNewValue<bool>())
            {
                if (!Subscriptions.TryGetValue(control, out _))
                {
                    // Subscribe to collection changed events instead of enumerating the
                    // classes collection (ToObservable would enumerate and emit items
                    // synchronously, which could cause Recompile to modify the collection
                    // while it is being iterated). Using FromEventPattern avoids that.
                    var sub = System.Reactive.Linq.Observable.FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                        h => control.Classes.CollectionChanged += h,
                        h => control.Classes.CollectionChanged -= h)
                        .Subscribe(_ => Recompile(control));

                    Subscriptions.Add(control, sub);
                }

                Recompile(control);
            }
            else
            {
                if (Subscriptions.TryGetValue(control, out var sub))
                {
                    sub.Dispose();
                    Subscriptions.Remove(control);
                }
            }
        }
    }

    /// <summary>
    /// Recompiles the classes for the specified control by comparing the current set of classes with the previously stored state and applying any necessary changes to ensure that the control's visual representation is up to date.
    /// </summary>
    /// <param name="control">The control whose classes are to be recompiled.</param>
    private static void Recompile(StyledElement control)
    {
        if (control is not Control ctrl)
            return;

        var state = ctrl.GetValue(RuntimeStateProperty);

        if (state is null)
        {
            state = new();
            ctrl.SetValue(RuntimeStateProperty, state);
        }

        var classes = control.Classes;

        var hash = ClassHasher.Hash(classes);

        if (hash == state.Hash)
            return;

        ClassDiffEngine.ApplyDiff(control, state, classes);

        state.Hash = hash;
    }

    #endregion

    #region Build Classes

    /// <summary>
    /// Sets the specified layer for a styled element and updates its associated CSS classes.
    /// </summary>
    /// <param name="element">The styled element to which the layer will be applied. Cannot be null.</param>
    /// <param name="name">The name of the layer to set for the specified element. If the layer does not exist, it will be created.</param>
    /// <param name="classes">An enumerable collection of CSS class names to associate with the layer. Existing classes for the layer will be
    /// cleared before adding the new classes.</param>
    private static void SetLayer(StyledElement element, string name, IEnumerable<string> classes)
    {
        var layers = GetLayers(element);

        if (!layers.TryGetValue(name, out var layer))
        {
            layer = new();
            layers[name] = layer;
        }

        layer.Classes.Clear();

        foreach (var c in classes)
            layer.Classes.Add(c);

        Rebuild(element);
    }

    /// <summary>
    /// Updates the set of CSS classes applied to the specified styled element based on the current configuration of
    /// class layers.
    /// </summary>
    /// <remarks>This method synchronizes the element's managed classes by applying the 'Replace', 'Add', and
    /// 'Remove' layers in order of precedence. Only the classes present in the resulting set are retained on the
    /// element, ensuring that its visual state reflects the latest layer configuration.</remarks>
    /// <param name="element">The styled element whose CSS classes are to be updated according to the defined replacement, addition, and
    /// removal layers. Cannot be null.</param>
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

        // Remove old managed classes
        foreach (var old in managed.ToList().Where(old => !newManaged.Contains(old)))
        {
            element.Classes.Remove(old);
            managed.Remove(old);
        }

        // Add new managed classes
        foreach (var c in newManaged.Where(c => !managed.Contains(c)))
        {
            element.Classes.Add(c);
            managed.Add(c);
        }
    }

    /// <summary>
    /// Extracts a collection of non-empty strings from the specified input value.
    /// </summary>
    /// <remarks>This method supports both single string inputs and collections of strings. For string inputs,
    /// it splits the string by spaces and removes empty entries. For collections, it filters out any entries that are
    /// null, empty, or consist only of whitespace. This is useful for normalizing input data before further
    /// processing.</remarks>
    /// <param name="value">The input object to extract strings from. This can be a string or an enumerable collection of strings. If null,
    /// an empty collection is returned.</param>
    /// <returns>An enumerable collection of non-empty strings extracted from the input value. Returns an empty collection if the
    /// input is null or contains only whitespace strings.</returns>
    private static IEnumerable<string> Extract(object? value) => value switch
    {
        null => [],
        string s => s.Split(' ', StringSplitOptions.RemoveEmptyEntries),
        IEnumerable<string> enumerable => enumerable.Where(x => !string.IsNullOrWhiteSpace(x)),
        _ => []
    };

    #endregion
}
