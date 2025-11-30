// -----------------------------------------------------------------------
// <copyright file="IPopupControl.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Input;

namespace MyNet.Avalonia.Controls.Primitives;

public interface IPopupControl : IInputElement
{
    bool IsDropDownOpen { get; set; }

    void OpenPopup();

    void ClosePopup();

    void TogglePopup();
}
