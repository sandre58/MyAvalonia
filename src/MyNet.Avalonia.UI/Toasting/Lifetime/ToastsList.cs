// -----------------------------------------------------------------------
// <copyright file="ToastsList.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;

namespace MyNet.Avalonia.UI.Toasting.Lifetime;

internal sealed class ToastsList : ConcurrentDictionary<int, ToastMetadata>
{
    internal ToastMetadata Add(Toast toast)
    {
        var metaData = new ToastMetadata(toast);
        this[toast.GetHashCode()] = metaData;
        return metaData;
    }
}

internal sealed class ToastMetadata
{
    public TimeSpan CreatedDate { get; }

    public Toast Toast { get; }

    internal ToastMetadata(Toast toast) => (Toast, CreatedDate) = (toast, DateTime.Now.TimeOfDay);
}
