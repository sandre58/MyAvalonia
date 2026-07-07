// -----------------------------------------------------------------------
// <copyright file="ToolBarAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130

/// <summary>
/// Attached properties for per-item toolbar customization.
/// <see cref="IsSpacerProperty"/> is a Phase 2 feature — currently a stub
/// that reserves the API without affecting layout.
/// </summary>
public static class ToolBarAssist
{
    /// <summary>
    /// When set to <c>true</c> on a child of <see cref="ToolBar"/>, the item acts as a flexible spacer
    /// that fills remaining space between adjacent items (Phase 2 — not yet processed by the layout engine).
    /// </summary>
    public static readonly AttachedProperty<bool> IsSpacerProperty =
        AvaloniaProperty.RegisterAttached<Control, bool>("IsSpacer", typeof(ToolBarAssist));

    public static bool GetIsSpacer(Control element) => element.GetValue(IsSpacerProperty);

    public static void SetIsSpacer(Control element, bool value) => element.SetValue(IsSpacerProperty, value);
}
