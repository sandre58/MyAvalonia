// -----------------------------------------------------------------------
// <copyright file="ClassHasher.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace MyNet.Avalonia.Theme.Classes.Engine;

/// <summary>
/// Defines a static utility class that provides a method to compute a hash value for a collection of class names. The hash is
/// computed using the FNV-1a algorithm, which is a fast, non-cryptographic hash function suitable for hash-based lookups.
/// </summary>
internal static class ClassHasher
{
    /// <summary>
    /// Generates a hash value for the specified collection of class names using the FNV-1a algorithm. The method processes each class name in a consistent order to ensure that the same set of classes produces the same hash value, regardless of their original order in the collection. This is achieved by sorting the class names before computing the hash. The resulting hash can be used as a key for caching compiled actions associated with specific combinations of classes.
    /// </summary>
    /// <param name="classes">A collection of class names for which to compute the hash. Cannot be null.</param>
    /// <returns>A 64-bit unsigned integer representing the hash value of the specified class names.</returns>
    public static ulong Hash(IEnumerable<string> classes)
    {
        const ulong offset = 14695981039346656037;
        const ulong prime = 1099511628211;

        var hash = offset;

        foreach (var c in classes.Order())
        {
            foreach (var ch in c)
            {
                hash ^= ch;
                hash *= prime;
            }

            hash ^= '|';
            hash *= prime;
        }

        return hash;
    }
}
