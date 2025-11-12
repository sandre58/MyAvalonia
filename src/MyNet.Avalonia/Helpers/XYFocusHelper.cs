// -----------------------------------------------------------------------
// <copyright file="XYFocusHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Input;
using Avalonia.VisualTree;

namespace MyNet.Avalonia.Helpers;

public static class XYFocusHelper
{
    public static bool IsAllowedXYNavigationMode(this InputElement visual, KeyDeviceType? keyDeviceType) => IsAllowedXYNavigationMode(XYFocus.GetNavigationModes(visual), keyDeviceType);

    private static bool IsAllowedXYNavigationMode(XYFocusNavigationModes modes, KeyDeviceType? keyDeviceType) => keyDeviceType switch
    {
        null => !modes.Equals(XYFocusNavigationModes.Disabled), // programmatic input, allow any subtree except Disabled.
        KeyDeviceType.Keyboard => modes.HasFlag(XYFocusNavigationModes.Keyboard),
        KeyDeviceType.Gamepad => modes.HasFlag(XYFocusNavigationModes.Gamepad),
        KeyDeviceType.Remote => modes.HasFlag(XYFocusNavigationModes.Remote),
        _ => throw new ArgumentOutOfRangeException(nameof(keyDeviceType), keyDeviceType, null)
    };

    public static InputElement? FindXYSearchRoot(this InputElement visual, KeyDeviceType? keyDeviceType)
    {
        var candidate = visual;
        var candidateParent = visual.FindAncestorOfType<InputElement>();

        while (candidateParent?.IsAllowedXYNavigationMode(keyDeviceType) == true)
        {
            candidate = candidateParent;
            candidateParent = candidate.FindAncestorOfType<InputElement>();
        }

        return candidate;
    }
}
