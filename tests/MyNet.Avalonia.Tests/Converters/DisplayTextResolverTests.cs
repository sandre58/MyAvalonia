// -----------------------------------------------------------------------
// <copyright file="DisplayTextResolverTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using FluentAssertions;
using MyNet.Avalonia.Converters;
using MyNet.Globalization.Localization.Translation;
using Xunit;

namespace MyNet.Avalonia.Tests.Converters;

public class DisplayTextResolverTests
{
    [Fact]
    public void TryConvertRegistered_UnregisteredType_ReturnsFalse()
    {
        var stub = new UnregisteredStub("value");

        DisplayTextResolver.TryConvertRegistered(stub, CultureInfo.InvariantCulture, out var text).Should().BeFalse();
        text.Should().BeNull();
    }

    [Fact]
    public void TryConvertRegistered_Integer_ReturnsCultureFormattedValue()
    {
        var culture = CultureInfo.GetCultureInfo("fr-FR");

        DisplayTextResolver.TryConvertRegistered(1234, culture, out var text).Should().BeTrue();
        text.Should().Be(1234.ToString(culture));
    }

    [Fact]
    public void IsRegisteredType_ReportsKnownAndUnknownTypes()
    {
        DisplayTextResolver.IsRegisteredType(typeof(int)).Should().BeTrue();
        DisplayTextResolver.IsRegisteredType(typeof(UnregisteredStub)).Should().BeFalse();
    }

    [Fact]
    public void RegisterTypeConverter_ForwardedFromStringConverter_IsUsedByResolver()
    {
        StringConverter.RegisterTypeConverter<ForwardedStub>((value, _, _, _) => value.Label);

        DisplayTextResolver.TryConvertRegistered(new ForwardedStub("resolved"), CultureInfo.InvariantCulture, out var text)
            .Should().BeTrue();
        text.Should().Be("resolved");
    }

    [Fact]
    public void Convert_RegisteredType_MatchesTryConvertRegisteredWithoutFallback()
    {
        var culture = CultureInfo.GetCultureInfo("fr-FR");
        const int value = 42;

        DisplayTextResolver.TryConvertRegistered(value, culture, out var registeredText).Should().BeTrue();
        DisplayTextResolver.Convert(value, TranslationOptionsPresets.Default, culture)
            .Should().Be(registeredText);
    }

    private sealed record UnregisteredStub(string Value);

    private sealed record ForwardedStub(string Label);
}
