// -----------------------------------------------------------------------
// <copyright file="DialogKeyboardHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using MyNet.UI.Dialogs.MessageBox;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Extended.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Shared keyboard handling for dialog shells and content.
/// </summary>
internal static class DialogKeyboardHelper
{
    public static bool TryHandleMessageBoxKey(KeyEventArgs e, MessageBoxResultOption buttons, Action<MessageBoxResult> close)
    {
        if (e.Handled)
            return false;

        if (e.Key == Key.Escape)
        {
            close(DialogButtonHelper.GetDefaultCloseResult(buttons));
            e.Handled = true;
            return true;
        }

        if (e.Key is not Key.Enter || e.KeyModifiers != KeyModifiers.None || ShouldIgnoreEnter(e.Source))
            return false;

        var affirmative = DialogButtonHelper.GetAffirmativeResult(buttons);
        if (affirmative == MessageBoxResult.None)
            return false;

        close(affirmative);
        e.Handled = true;
        return true;
    }

    public static bool TryHandleContentDialogKey(Control scope, KeyEventArgs e)
    {
        if (e.Handled)
            return false;

        if (e.Key == Key.Escape)
        {
            if (TryExecuteButton(FindDismissButton(scope)))
            {
                e.Handled = true;
                return true;
            }

            return false;
        }

        if (e.Key is not Key.Enter || e.KeyModifiers != KeyModifiers.None || ShouldIgnoreEnter(e.Source))
            return false;

        if (!TryExecuteButton(FindAffirmativeButton(scope)))
            return false;

        e.Handled = true;
        return true;
    }

    private static bool ShouldIgnoreEnter(object? source)
        => source is TextBox textBox && (textBox.AcceptsReturn || textBox.AcceptsTab);

    private static Button? FindAffirmativeButton(Control scope)
        => FindButton(scope, static button => button.IsDefault)
           ?? FindButton(scope, static button => HasClass(button, "kind-filled") || HasClass(button, "variant-filled"));

    private static Button? FindDismissButton(Control scope)
        => FindButton(scope, static button => HasClass(button, "kind-text") || HasClass(button, "variant-text") || HasClass(button, "variant-outlined"));

    private static Button? FindButton(Control scope, Func<Button, bool> predicate)
    {
        foreach (var button in scope.GetVisualDescendants())
        {
            if (button is not Button { IsVisible: true, IsEffectivelyEnabled: true } candidate)
                continue;

            if (predicate(candidate))
                return candidate;
        }

        return null;
    }

    private static bool HasClass(Button button, string className)
        => button.Classes.Contains(className);

    private static bool TryExecuteButton(Button? button)
    {
        if (button is not { IsVisible: true, IsEffectivelyEnabled: true })
            return false;

        if (button.Command?.CanExecute(button.CommandParameter) == true)
        {
            button.Command.Execute(button.CommandParameter);
            return true;
        }

        button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        return true;
    }
}
