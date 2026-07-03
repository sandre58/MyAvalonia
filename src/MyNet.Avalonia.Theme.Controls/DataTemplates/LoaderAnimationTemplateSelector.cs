// -----------------------------------------------------------------------
// <copyright file="LoaderAnimationTemplateSelector.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;
using MyNet.Avalonia.Controls.Enums;

namespace MyNet.Avalonia.Theme.Controls.DataTemplates;

/// <summary>
/// Selects a <see cref="IDataTemplate"/> for each <see cref="LoaderAnimation"/> value.
/// </summary>
public sealed class LoaderAnimationTemplateSelector : IDataTemplate
{
    /// <summary>
    /// Gets templates keyed by <see cref="LoaderAnimation"/> member name.
    /// </summary>
    [Content]
    public Dictionary<string, IDataTemplate> Templates { get; } = [];

    /// <inheritdoc/>
    public bool Match(object? data) => data is LoaderAnimation animation && Templates.ContainsKey(animation.ToString());

    /// <inheritdoc/>
    public Control? Build(object? param) => Templates[param!.ToString()!].Build(param);
}
