// -----------------------------------------------------------------------
// <copyright file="ToolBarItem.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Templates;
using MyNet.Avalonia.Controls.Primitives;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130

/// <summary>
/// Container item for a <see cref="ToolBar"/>. Extends <see cref="Button"/> to inherit
/// built-in command execution and accessibility semantics, while adding toolbar-specific
/// properties: <see cref="Header"/>, <see cref="Icon"/>,
/// and <see cref="OverflowPriority"/>.
/// </summary>
[TemplatePart(PartRoot, typeof(Border))]
public class ToolBarItem : Button
{
    public const string PartRoot = "PART_Root";

    public static readonly StyledProperty<object?> HeaderProperty =
        AvaloniaProperty.Register<ToolBarItem, object?>(nameof(Header));

    public static readonly StyledProperty<IDataTemplate?> HeaderTemplateProperty =
        AvaloniaProperty.Register<ToolBarItem, IDataTemplate?>(nameof(HeaderTemplate));

    public static readonly StyledProperty<object?> IconProperty =
        AvaloniaProperty.Register<ToolBarItem, object?>(nameof(Icon));

    public static readonly StyledProperty<ToolBarOverflowPriority> OverflowPriorityProperty =
        AvaloniaProperty.Register<ToolBarItem, ToolBarOverflowPriority>(nameof(OverflowPriority));

    public object? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public IDataTemplate? HeaderTemplate
    {
        get => GetValue(HeaderTemplateProperty);
        set => SetValue(HeaderTemplateProperty, value);
    }

    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public ToolBarOverflowPriority OverflowPriority
    {
        get => GetValue(OverflowPriorityProperty);
        set => SetValue(OverflowPriorityProperty, value);
    }
}
