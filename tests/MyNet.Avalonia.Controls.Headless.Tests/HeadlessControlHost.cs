// -----------------------------------------------------------------------
// <copyright file="HeadlessControlHost.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;

namespace MyNet.Avalonia.Controls.Headless.Tests;

internal static class HeadlessControlHost
{
    public static Window Show(Control control, Size? size = null)
    {
        size ??= new(800, 600);
        control.Width = size.Value.Width;
        control.Height = size.Value.Height;

        var window = new Window
        {
            Content = control,
            Width = size.Value.Width + 40,
            Height = size.Value.Height + 40
        };

        window.Show();
        return window;
    }

    public static T? FindByName<T>(Control root, string name)
        where T : Control =>
        root.GetVisualDescendants().OfType<T>().FirstOrDefault(x => x.Name == name);

    public static void Click(Button button) =>
        button.RaiseEvent(new(Button.ClickEvent));

    public static void PointerPress(Button button, KeyModifiers modifiers = KeyModifiers.None) =>
        button.RaiseEvent(new PointerPressedEventArgs(
            button,
            new Pointer(1, PointerType.Mouse, true),
            button,
            new(4, 4),
            0,
            new PointerPointProperties(RawInputModifiers.None, PointerUpdateKind.LeftButtonPressed),
            modifiers,
            0));

    public static void PointerRelease(Button button, KeyModifiers modifiers = KeyModifiers.None) =>
        button.RaiseEvent(new PointerReleasedEventArgs(
            InputElement.PointerReleasedEvent,
            new Pointer(1, PointerType.Mouse, true),
            button,
            new(4, 4),
            0,
            new PointerPointProperties(RawInputModifiers.None, PointerUpdateKind.LeftButtonReleased),
            modifiers,
            MouseButton.Left));

    public static void PointerEnter(Control control, KeyModifiers modifiers = KeyModifiers.None) =>
        control.RaiseEvent(new PointerEventArgs(InputElement.PointerEnteredEvent, control, null!, control, new(4, 4), 0, new PointerPointProperties(RawInputModifiers.None, PointerUpdateKind.Other), modifiers));

    public static void PointerMove(Control control, KeyModifiers modifiers = KeyModifiers.None) =>
        control.RaiseEvent(new PointerEventArgs(InputElement.PointerMovedEvent, control, null!, control, new(4, 4), 0, new PointerPointProperties(RawInputModifiers.None, PointerUpdateKind.Other), modifiers));

    public static void KeyDown(InputElement element, Key key, KeyModifiers modifiers = KeyModifiers.None) =>
        element.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyDownEvent,
            Key = key,
            KeyModifiers = modifiers
        });

    public static void KeyUp(InputElement element, Key key, KeyModifiers modifiers = KeyModifiers.None) =>
        element.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyUpEvent,
            Key = key,
            KeyModifiers = modifiers
        });
}
