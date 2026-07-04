// -----------------------------------------------------------------------
// <copyright file="MultiComboBox.Keyboard.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using MyNet.Avalonia.Controls.Behaviors;

namespace MyNet.Avalonia.Controls;

public partial class MultiComboBox
{
    /// <inheritdoc/>
    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        if (e.Handled)
            return;

        if (IsSearchEnabled && ItemsSearchBehavior.TryHandleKeyDown(this, e))
            return;

        if (!IsDropDownOpen)
        {
            switch (e.Key)
            {
                case Key.Enter:
                case Key.Down:
                case Key.Up:
                case Key.F4:
                    OpenPopup();

                    break;
            }
        }
        else
        {
            var hotkeys = Application.Current!.PlatformSettings?.HotkeyConfiguration;
            var ctrl = hotkeys is not null && e.KeyModifiers.HasFlag(hotkeys.CommandModifiers);

            if (e.Key is Key.Escape or Key.Tab or Key.F4)
            {
                if (IsSearchEnabled && e.Key == Key.Escape && !string.IsNullOrEmpty(SearchText))
                {
                    ItemsSearchBehavior.ClearSearchText(this);
                    ItemsSearchBehavior.FocusSearchBoxIfEnabled(this);
                    e.Handled = true;
                    return;
                }

                ClosePopup();
                e.Handled = true;
            }

            // This part of code is needed just to acquire initial focus, subsequent focus navigation will be done by ItemsControl.
            else if (SelectedIndex < 0 && ItemCount > 0 && e.Key is Key.Up or Key.Down && IsFocused)
            {
                var firstChild = Presenter?.Panel?.Children.FirstOrDefault(CanFocus);
                if (firstChild != null)
                {
                    e.Handled = firstChild.Focus(NavigationMethod.Directional);
                }
            }
            else if (!ctrl && e.Key.ToNavigationDirection() is { } direction && direction.IsDirectional())
            {
                e.Handled |= MoveSelection(direction, WrapSelection, e.KeyModifiers.HasFlag(KeyModifiers.Shift));
            }
            else if (SelectionMode.HasFlag(SelectionMode.Multiple) && hotkeys?.SelectAll.Any(x => x.Matches(e)) == true)
            {
                SelectAll();
                e.Handled = true;
            }
            else if (e.Key is Key.Space or Key.Enter)
            {
                UpdateSelectionFromEvent((Control)e.Source!, e);
            }
        }
    }
}
