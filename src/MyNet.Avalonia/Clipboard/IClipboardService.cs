// -----------------------------------------------------------------------
// <copyright file="IClipboardService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Threading.Tasks;
using Avalonia.Input;

namespace MyNet.Avalonia.Clipboard;

public interface IClipboardService
{
    Task CopyAsync(IDataObject content);

    Task CopyTextAsync(string text);
}
