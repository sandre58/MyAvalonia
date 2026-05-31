// -----------------------------------------------------------------------
// <copyright file="TextPickerCommitHelperTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using FluentAssertions;
using MyNet.Avalonia.Controls.Primitives.Internal;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.TextPicker;

public class TextPickerCommitHelperTests
{
    [Fact]
    public void ResolveCommitAction_EmptyTextWithValue_ClearsValue()
    {
        TextPickerCommitHelper.ResolveCommitAction("   ", "2026-05-15", hasSelectedValue: true)
            .Should().Be(TextPickerTextCommitKind.ClearValue);
    }

    [Fact]
    public void ResolveCommitAction_UnchangedText_IsNoOp()
    {
        TextPickerCommitHelper.ResolveCommitAction("2026-05-15", "2026-05-15", hasSelectedValue: true)
            .Should().Be(TextPickerTextCommitKind.NoOp);
    }

    [Fact]
    public void ResolveCommitAction_ChangedText_ParsesAndApplies()
    {
        TextPickerCommitHelper.ResolveCommitAction("2026-06-01", "2026-05-15", hasSelectedValue: true)
            .Should().Be(TextPickerTextCommitKind.ParseAndApply);
    }

    [Fact]
    public void ShouldApplyParsedValue_WhenDifferent_ReturnsTrue()
    {
        TextPickerCommitHelper.ShouldApplyParsedValue(2, 1).Should().BeTrue();
        TextPickerCommitHelper.ShouldApplyParsedValue(1, 1).Should().BeFalse();
        TextPickerCommitHelper.ShouldApplyParsedValue((int?)null, 1).Should().BeFalse();
    }
}
