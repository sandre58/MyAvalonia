// -----------------------------------------------------------------------
// <copyright file="MultiComboBoxItem.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Automation;
using Avalonia.Automation.Peers;
using Avalonia.Controls;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class MultiComboBoxItem : ListBoxItem
{
    static MultiComboBoxItem() => AutomationProperties.ControlTypeOverrideProperty.OverrideDefaultValue<MultiComboBoxItem>(AutomationControlType.ComboBoxItem);

    public MultiComboBoxItem() => this.GetObservable(IsFocusedProperty)
                .Subscribe(focused =>
                {
                    if (focused)
                    {
                        (Parent as MultiComboBox)?.ItemFocused(this);
                    }
                });
}
