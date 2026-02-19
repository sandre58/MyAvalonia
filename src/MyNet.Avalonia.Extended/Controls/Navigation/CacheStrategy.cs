// -----------------------------------------------------------------------
// <copyright file="CacheStrategy.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Extended.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Cache strategy for view reuse.
/// </summary>
public enum CacheStrategy
{
    /// <summary>No caching - views are recreated on each navigation.</summary>
    None,

    /// <summary>Cache by page instance - each page instance gets its own cached view.</summary>
    ByInstance,

    /// <summary>Cache by page type - one cached view per page type (all instances share the same view).</summary>
    ByType
}
