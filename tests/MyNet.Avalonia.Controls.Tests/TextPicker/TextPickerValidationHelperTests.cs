// -----------------------------------------------------------------------
// <copyright file="TextPickerValidationHelperTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using FluentAssertions;
using MyNet.Avalonia.Controls.Primitives.Internal;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.TextPicker;

public class TextPickerValidationHelperTests
{
    [Fact]
    public void Parse_EmptyText_ReturnsEmpty()
    {
        var result = TextPickerValidationHelper.Parse<string?>("  ", s => s, _ => true);

        result.Status.Should().Be(TextPickerParseStatus.Empty);
    }

    [Fact]
    public void Parse_ValidValue_ReturnsSuccess()
    {
        var result = TextPickerValidationHelper.Parse("42", int.Parse, v => v > 0);

        result.Status.Should().Be(TextPickerParseStatus.Success);
        result.Value.Should().Be(42);
    }

    [Fact]
    public void Parse_InvalidValue_ReturnsInvalidValue()
    {
        var result = TextPickerValidationHelper.Parse("42", int.Parse, v => v < 0);

        result.Status.Should().Be(TextPickerParseStatus.InvalidValue);
    }

    [Fact]
    public void Parse_FormatException_ReturnsFormatError()
    {
        var result = TextPickerValidationHelper.Parse("abc", int.Parse, _ => true);

        result.Status.Should().Be(TextPickerParseStatus.FormatError);
        result.Error.Should().BeOfType<FormatException>();
    }
}
