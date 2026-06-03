// -----------------------------------------------------------------------
// <copyright file="CulturePicker.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace MyNet.Avalonia.Geography.Controls;

/// <summary>
/// Compact culture selector (flag button + menu). Does not depend on shell view models.
/// </summary>
public class CulturePicker : DropDownButton
{
    public static readonly StyledProperty<IEnumerable<CultureInfo>?> CulturesProperty = AvaloniaProperty.Register<CulturePicker, IEnumerable<CultureInfo>?>(nameof(Cultures));

    public static readonly StyledProperty<CultureInfo?> SelectedCultureProperty = AvaloniaProperty.Register<CulturePicker, CultureInfo?>(nameof(SelectedCulture));

    public static readonly StyledProperty<ICommand?> SelectCultureCommandProperty = AvaloniaProperty.Register<CulturePicker, ICommand?>(nameof(SelectCultureCommand));

    static CulturePicker()
    {
        CulturesProperty.Changed.AddClassHandler<CulturePicker>((picker, _) => picker.RebuildFlyoutItems());
        SelectedCultureProperty.Changed.AddClassHandler<CulturePicker>((picker, _) => picker.UpdateCheckedStates());
        SelectCultureCommandProperty.Changed.AddClassHandler<CulturePicker>((picker, _) => picker.RebuildFlyoutItems());
    }

    public IEnumerable<CultureInfo>? Cultures
    {
        get => GetValue(CulturesProperty);
        set => SetValue(CulturesProperty, value);
    }

    public CultureInfo? SelectedCulture
    {
        get => GetValue(SelectedCultureProperty);
        set => SetValue(SelectedCultureProperty, value);
    }

    public ICommand? SelectCultureCommand
    {
        get => GetValue(SelectCultureCommandProperty);
        set => SetValue(SelectCultureCommandProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        RebuildFlyoutItems();
    }

    private void RebuildFlyoutItems()
    {
        if (Flyout is not MenuFlyout menu)
            return;

        menu.Items.Clear();

        if (Cultures is null)
            return;

        foreach (var culture in Cultures)
        {
            menu.Items.Add(new MenuItem
            {
                Header = culture,
                Command = SelectCultureCommand,
                CommandParameter = culture,
                ToggleType = MenuItemToggleType.CheckBox,
                IsChecked = CultureEquals(culture, SelectedCulture),
            });
        }
    }

    private void UpdateCheckedStates()
    {
        if (Flyout is not MenuFlyout menu)
            return;

        foreach (var item in menu.Items)
        {
            if (item is MenuItem { CommandParameter: CultureInfo culture } menuItem)
                menuItem.IsChecked = CultureEquals(culture, SelectedCulture);
        }
    }

    private static bool CultureEquals(CultureInfo? a, CultureInfo? b) =>
        ReferenceEquals(a, b)
        || (a is not null && b is not null && string.Equals(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));
}
