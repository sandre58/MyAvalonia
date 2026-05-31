// -----------------------------------------------------------------------
// <copyright file="StringConverterTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using FluentAssertions;
using MyNet.Avalonia.Converters;
using MyNet.Globalization.Localization.Translation;
using MyNet.Text.TextCasing;
using Xunit;

namespace MyNet.Avalonia.Tests.Converters;

public class StringConverterTests
{
    [Fact]
    public void Convert_Integer_UsesCultureFormatting()
    {
        var converter = StringConverter.Default;
        var culture = CultureInfo.GetCultureInfo("fr-FR");

        var result = converter.Convert(1234, typeof(string), "N0", culture);

        result.Should().Be(1234.ToString("N0", culture));
    }

    [Fact]
    public void Convert_Null_ReturnsNull()
    {
        StringConverter.Default.Convert(null, null, CultureInfo.InvariantCulture).Should().BeNull();
    }

    [Fact]
    public void QuantityFromValue_PassesQuantityToOptions()
    {
        var converter = new StringConverter(LetterCasing.Normal, TranslationOptionsPresets.Default)
        {
            QuantityFromValue = true
        };

        converter.Convert(5, typeof(string), null, CultureInfo.InvariantCulture).Should().NotBeNull();
    }

    [Fact]
    public void ToTitle_AppliesCasing()
    {
        var result = StringConverter.ToTitle.Convert("hello world", typeof(string), null, CultureInfo.InvariantCulture);

        result.Should().Be("Hello World");
    }
}
