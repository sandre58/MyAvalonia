// -----------------------------------------------------------------------
// <copyright file="TextPickerExpandedTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Input;
using FluentAssertions;
using MyNet.Avalonia.Controls.Primitives.Internal;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.TextPicker;

public class TextPickerExpandedTests
{
    [Fact]
    public void Resolve_WhenClosed_DownDecrementsValue()
    {
        var result = TextPickerKeyboardHelper.Resolve(Key.Down, false, true, KeyModifiers.None);

        result.Should().Be(new TextPickerKeyResult(TextPickerKeyAction.IncrementByOffset, -1));
    }

    [Fact]
    public void Resolve_WhenClosed_PageDownDecrementsLarge()
    {
        var result = TextPickerKeyboardHelper.Resolve(Key.PageDown, false, true, KeyModifiers.None);

        result.Should().Be(new TextPickerKeyResult(TextPickerKeyAction.IncrementLargeByOffset, -1));
    }

    [Fact]
    public void Resolve_WhenClosedWithoutValue_ReturnsNone()
    {
        TextPickerKeyboardHelper.Resolve(Key.Up, false, false, KeyModifiers.None)
            .Action.Should().Be(TextPickerKeyAction.None);
    }

    [Fact]
    public void ResolveCommitAction_EmptyWithoutValue_IsNoOp()
    {
        TextPickerCommitHelper.ResolveCommitAction(string.Empty, null, false)
            .Should().Be(TextPickerTextCommitKind.NoOp);
    }

    [Fact]
    public void Parse_WithNullConverterResult_ReturnsInvalidWhenNotValid()
    {
        var result = TextPickerValidationHelper.Parse<string?>("test", _ => null, v => v is not null);

        result.Status.Should().Be(TextPickerParseStatus.InvalidValue);
    }
}
