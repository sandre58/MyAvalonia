// -----------------------------------------------------------------------
// <copyright file="NullFallbackConverterTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using Avalonia.Data;
using FluentAssertions;
using MyNet.Avalonia.Converters;
using Xunit;

namespace MyNet.Avalonia.Tests.Converters;

public class NullFallbackConverterTests
{
    private static readonly NullFallbackConverter Converter = NullFallbackConverter.Default;

    [Fact]
    public void Convert_UsesPrimaryWhenPresent()
    {
        var result = Converter.Convert("primary", typeof(string), "fallback", CultureInfo.InvariantCulture);

        result.Should().Be("primary");
    }

    [Fact]
    public void Convert_UsesFallbackWhenPrimaryNull()
    {
        var result = Converter.Convert(null!, typeof(string), "fallback", CultureInfo.InvariantCulture);

        result.Should().Be("fallback");
    }

    [Fact]
    public void Convert_AllNull_ReturnsDoNothing()
    {
        var result = Converter.Convert(null!, typeof(string), null, CultureInfo.InvariantCulture);

        result.Should().BeSameAs(BindingOperations.DoNothing);
    }
}
