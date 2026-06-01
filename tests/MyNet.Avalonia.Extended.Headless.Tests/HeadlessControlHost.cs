// -----------------------------------------------------------------------
// <copyright file="HeadlessControlHost.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;

namespace MyNet.Avalonia.Extended.Headless.Tests;

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
}
