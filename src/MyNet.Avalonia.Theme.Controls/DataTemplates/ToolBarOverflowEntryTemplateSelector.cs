// -----------------------------------------------------------------------
// <copyright file="ToolBarOverflowEntryTemplateSelector.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Avalonia.Controls.Templates;
using MyNet.Avalonia.Controls.Primitives;

namespace MyNet.Avalonia.Theme.Controls.DataTemplates;

/// <summary>
/// Selects item or separator <see cref="IDataTemplate"/> for <see cref="ToolBarOverflowEntry"/> popup rows.
/// </summary>
public sealed class ToolBarOverflowEntryTemplateSelector : IDataTemplate
{
    /// <summary>
    /// Gets or sets the template for toolbar overflow action items.
    /// </summary>
    public IDataTemplate? Item { get; set; }

    /// <summary>
    /// Gets or sets the template for separator rows.
    /// </summary>
    public IDataTemplate? Separator { get; set; }

    /// <inheritdoc/>
    public bool Match(object? data) => data is ToolBarOverflowEntry;

    /// <inheritdoc/>
    public Control? Build(object? param)
    {
        if (param is not ToolBarOverflowEntry entry)
            return null;

        var template = entry.IsSeparator ? Separator : Item;
        return template?.Build(param);
    }
}
