// -----------------------------------------------------------------------
// <copyright file="StyledElementExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Avalonia;

namespace MyNet.Avalonia.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="StyledElement"/> class, allowing for convenient manipulation of CSS classes on Avalonia controls.
/// </summary>
[SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Extensions methods must be in a static class, and extension methods cannot be in a nested class.")]
public static class StyledElementExtensions
{
    extension(StyledElement obj)
    {
        /// <summary>
        /// Adds one or more class names to the object's class collection. Each input string may contain multiple class
        /// names separated by whitespace.
        /// </summary>
        /// <remarks>This method splits each provided string by whitespace and adds all resulting class
        /// names to the collection. Only non-empty class names are included.</remarks>
        /// <param name="classes">An array of class name strings to add. Each string can include one or more class names separated by spaces.
        /// Empty or whitespace-only entries are ignored.</param>
        public void AddClasses(params string[] classes) => obj.Classes.AddRange(classes.SelectMany(x => x.Split(" ", System.StringSplitOptions.RemoveEmptyEntries)));

        /// <summary>
        /// Removes the specified class names from the object's class collection.
        /// </summary>
        /// <remarks>This method removes all occurrences of the provided class names from the collection.
        /// If a specified class name does not exist in the collection, it is ignored.</remarks>
        /// <param name="classes">An array of class names to remove. Each class name must be a non-empty string.</param>
        public void RemoveClasses(params string[] classes) => obj.Classes.RemoveAll(classes);
    }
}
