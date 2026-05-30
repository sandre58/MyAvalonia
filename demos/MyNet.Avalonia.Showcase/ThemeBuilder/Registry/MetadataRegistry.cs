// -----------------------------------------------------------------------
// <copyright file="MetadataRegistry.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Registry;

/// <summary>
/// Provides a base class for associating metadata objects with keys, enabling registration and retrieval of metadata by
/// key.
/// </summary>
/// <remarks>This abstract class maintains a registry of key-metadata pairs, supporting efficient registration and
/// lookup operations. Derived classes can use this registry to manage metadata for various scenarios, such as type
/// associations or configuration extensions. Thread safety is not guaranteed; callers should ensure appropriate
/// synchronization if used concurrently.</remarks>
/// <typeparam name="TKey">The type of the key used to identify metadata entries. Must be a non-nullable type.</typeparam>
/// <typeparam name="TMetadata">The type of the metadata associated with each key. Must be a reference type.</typeparam>
internal abstract class MetadataRegistry<TKey, TMetadata>
    where TKey : notnull
    where TMetadata : class
{
    private readonly Dictionary<TKey, TMetadata> _map = [];

    /// <summary>
    /// Associates the specified key with the provided metadata in the registry. If the key already exists, its metadata
    /// is updated.
    /// </summary>
    /// <remarks>If the specified key already exists in the registry, its associated metadata is replaced with
    /// the new value.</remarks>
    /// <param name="key">The key to associate with the metadata. Must be unique within the registry.</param>
    /// <param name="metadata">The metadata to associate with the specified key.</param>
    public void Register(TKey key, TMetadata metadata) => _map[key] = metadata;

    /// <summary>
    /// Merges the specified metadata registry into the current registry, updating existing entries and adding new ones.
    /// </summary>
    /// <remarks>If a key in the provided registry already exists in the current registry, its value will be
    /// replaced. This method can be used to combine or update metadata from multiple sources.</remarks>
    /// <param name="registry">The metadata registry to merge with the current registry. Entries from this registry will overwrite existing
    /// entries with the same key in the current registry.</param>
    public void Merge(MetadataRegistry<TKey, TMetadata> registry)
    {
        foreach (var kvp in registry._map)
        {
            _map[kvp.Key] = kvp.Value;
        }
    }

    /// <summary>
    /// Retrieves the metadata associated with the specified key, or null if the key does not exist.
    /// </summary>
    /// <remarks>This method uses a try-get pattern to efficiently check for the existence of the key before
    /// attempting to retrieve the associated metadata.</remarks>
    /// <param name="key">The key associated with the metadata to retrieve. Must not be null.</param>
    /// <returns>The metadata associated with the specified key, or null if the key is not found.</returns>
    public TMetadata? Get(TKey key) => TryGet(key, out var meta) ? meta : null;

    /// <summary>
    /// Retrieves the metadata associated with the specified key, cast to the specified type, or null if the key does not exist or cannot be cast.
    /// </summary>
    /// <typeparam name="T">The type to which the metadata should be cast. Must be a subclass of TMetadata.</typeparam>
    /// <param name="key">The key associated with the metadata to retrieve. Must not be null.</param>
    /// <returns>The metadata associated with the specified key, cast to the specified type, or null if the key is not found or the cast fails.</returns>
    public T? Get<T>(TKey key)
        where T : TMetadata => (T?)Get(key);

    /// <summary>
    /// Attempts to retrieve the metadata associated with the specified key.
    /// </summary>
    /// <remarks>This method enables safe retrieval of metadata without throwing an exception if the key does
    /// not exist.</remarks>
    /// <param name="key">The key whose associated metadata is to be retrieved.</param>
    /// <param name="metadata">When this method returns, contains the metadata associated with the specified key, if the key is found;
    /// otherwise, the default value for the metadata type.</param>
    /// <returns>true if the metadata was found for the specified key; otherwise, false.</returns>
    public bool TryGet(TKey key, out TMetadata? metadata) => _map.TryGetValue(key, out metadata);
}
