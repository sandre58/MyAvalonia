// -----------------------------------------------------------------------
// <copyright file="StyledElementExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Avalonia;
using MyNet.Avalonia.Theme.Classes;

namespace MyNet.Avalonia.Theme.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="StyledElement"/> class, allowing for convenient manipulation of CSS classes on Avalonia controls using the <see cref="CssClass"/> record type.
/// </summary>
[SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Extensions methods must be in a static class, and extension methods cannot be in a nested class.")]
public static class StyledElementExtensions
{
    extension(StyledElement obj)
    {
        /// <summary>
        /// Adds one or more CSS classes to the styled element's class list, converting each <see cref="CssClass"/> to its string representation before adding it to the list.
        /// </summary>
        /// <param name="classes">An array of <see cref="CssClass"/> instances to add to the styled element's class list.</param>
        public void AddClasses(params CssClass[] classes) => obj.Classes.AddRange(classes.Select(x => x.ToString()));

        /// <summary>
        /// Removes the specified CSS classes from the object's class collection.
        /// </summary>
        /// <remarks>This method modifies the internal collection of classes by removing all instances
        /// that match the provided class names. Ensure that the classes to be removed are valid and exist in the
        /// collection to avoid unnecessary operations.</remarks>
        /// <param name="classes">An array of <see cref="CssClass"/> instances representing the CSS classes to be removed from the collection.</param>
        public void RemoveClasses(params CssClass[] classes) => obj.Classes.RemoveAll(classes.Select(x => x.ToString()));
    }
}
