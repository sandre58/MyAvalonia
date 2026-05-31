// -----------------------------------------------------------------------
// <copyright file="EnumConverterTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using FluentAssertions;
using MyNet.Avalonia.Converters;
using Xunit;

namespace MyNet.Avalonia.Tests.Converters;

public class EnumConverterTests
{
    private enum Sample
    {
        None = 0,
        Active = 1,
        Disabled = 2
    }

    [Fact]
    public void Any_MatchingValue_ReturnsTrue()
    {
        EnumConverter.Any.Convert(Sample.Active, typeof(bool), Sample.Active, CultureInfo.InvariantCulture).Should().Be(true);
    }

    [Fact]
    public void NotAny_DifferentValue_ReturnsTrue()
    {
        EnumConverter.NotAny.Convert(Sample.Active, typeof(bool), Sample.Disabled, CultureInfo.InvariantCulture).Should().Be(true);
    }

    [Fact]
    public void Convert_NullValue_ReturnsFalse()
    {
        EnumConverter.Any.Convert(null, typeof(bool), Sample.Active, CultureInfo.InvariantCulture).Should().Be(false);
    }
}
