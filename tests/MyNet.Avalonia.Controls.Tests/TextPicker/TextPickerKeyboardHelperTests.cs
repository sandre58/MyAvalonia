// -----------------------------------------------------------------------
// <copyright file="TextPickerKeyboardHelperTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Input;
using FluentAssertions;
using MyNet.Avalonia.Controls.Primitives.Internal;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.TextPicker;

public class TextPickerKeyboardHelperTests
{
    [Fact]
    public void Resolve_WhenDropDownOpen_EnterCommitsPreview()
    {
        var result = TextPickerKeyboardHelper.Resolve(Key.Enter, isDropDownOpen: true, hasSelectedValue: true, KeyModifiers.None);

        result.Action.Should().Be(TextPickerKeyAction.CommitPreview);
    }

    [Fact]
    public void Resolve_WhenDropDownOpen_EscapeRollsBack()
    {
        var result = TextPickerKeyboardHelper.Resolve(Key.Escape, isDropDownOpen: true, hasSelectedValue: true, KeyModifiers.None);

        result.Action.Should().Be(TextPickerKeyAction.Rollback);
    }

    [Fact]
    public void Resolve_WhenClosed_UpIncrementsValue()
    {
        var result = TextPickerKeyboardHelper.Resolve(Key.Up, isDropDownOpen: false, hasSelectedValue: true, KeyModifiers.None);

        result.Should().Be(new TextPickerKeyResult(TextPickerKeyAction.IncrementByOffset, 1));
    }

    [Fact]
    public void Resolve_WhenClosedWithModifiers_ReturnsNone()
    {
        var result = TextPickerKeyboardHelper.Resolve(Key.Up, isDropDownOpen: false, hasSelectedValue: true, KeyModifiers.Shift);

        result.Action.Should().Be(TextPickerKeyAction.None);
    }
}
