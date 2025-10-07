// -----------------------------------------------------------------------
// <copyright file="PaginationButton.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[PseudoClasses(PseudoClassName.Left, PseudoClassName.Right, PseudoClassName.Selected)]
public class PaginationButton : RepeatButton
{
    public static readonly StyledProperty<int> PageProperty = AvaloniaProperty.Register<PaginationButton, int>(
        nameof(Page));

    public int Page
    {
        get => GetValue(PageProperty);
        set => SetValue(PageProperty, value);
    }

    internal bool IsFastForward { get; private set; }

    internal bool IsFastBackward { get; private set; }

    internal void SetStatus(int page, bool isSelected, bool isLeft, bool isRight)
    {
        PseudoClasses.Set(PseudoClassName.Selected, isSelected);
        PseudoClasses.Set(PseudoClassName.Left, isLeft);
        PseudoClasses.Set(PseudoClassName.Right, isRight);
        IsFastForward = isLeft;
        IsFastBackward = isRight;
        Page = page;
    }

    internal void SetSelected(bool isSelected) => PseudoClasses.Set(PseudoClassName.Selected, isSelected);
}
