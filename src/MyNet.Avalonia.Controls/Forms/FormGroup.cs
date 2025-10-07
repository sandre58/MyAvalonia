// -----------------------------------------------------------------------
// <copyright file="FormGroup.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Avalonia.Controls.Primitives;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class FormGroup : HeaderedItemsControl
{
    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        recycleKey = null;
        return item is not FormItem;
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey) => item is not Control control
            ? new FormItem()
            : new FormItem
            {
                Content = control,
                [!FormItem.LabelProperty] = control[!FormItem.LabelProperty],
                [!FormItem.IsRequiredProperty] = control[!FormItem.IsRequiredProperty],
            };

    protected override void PrepareContainerForItemOverride(Control container, object? item, int index)
    {
        base.PrepareContainerForItemOverride(container, item, index);
        if (container is FormItem formItem && !formItem.IsSet(ContentControl.ContentTemplateProperty))
        {
            formItem.ContentTemplate = ItemTemplate;
        }
    }
}
