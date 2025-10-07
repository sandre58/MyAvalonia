// -----------------------------------------------------------------------
// <copyright file="MultiComboBoxSelectedItemList.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class MultiComboBoxSelectedItemList : ItemsControl
{
    public static readonly StyledProperty<ICommand?> RemoveCommandProperty = AvaloniaProperty.Register<MultiComboBoxSelectedItemList, ICommand?>(nameof(RemoveCommand));

    public ICommand? RemoveCommand
    {
        get => GetValue(RemoveCommandProperty);
        set => SetValue(RemoveCommandProperty, value);
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey) => NeedsContainer<Tag>(item, out recycleKey);

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey) => new Tag();

    protected override void PrepareContainerForItemOverride(Control container, object? item, int index)
    {
        base.PrepareContainerForItemOverride(container, item, index);
        if (container is Tag tag)
            tag.CloseCommand = RemoveCommand;
        if (container is ContentControl contentControl)
            contentControl.ContentTemplate = ItemTemplate;
    }
}
