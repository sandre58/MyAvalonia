// -----------------------------------------------------------------------
// <copyright file="ThemeBindingHelperTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Data;
using FluentAssertions;
using MyNet.Avalonia.Theme.MarkupExtensions.Helpers;
using Xunit;

namespace MyNet.Avalonia.Theme.Tests.MarkupExtensions;

public class ThemeBindingHelperTests
{
    [Fact]
    public void Create_ReturnsStandardBinding_NotReflectionBinding()
    {
        var binding = ThemeBindingHelper.Create("Primary", new(RelativeSourceMode.Self));

        binding.Should().BeOfType<Binding>();
        binding.Path.Should().Be("Primary");
    }

    [Fact]
    public void CreateScaledAncestorFontSize_ReturnsStandardBinding()
    {
        var binding = ThemeBindingHelper.CreateScaledAncestorFontSize(0.75);

        binding.Should().BeOfType<Binding>();
        binding.Path.Should().Be("(TextElement.FontSize)");
        binding.Converter.Should().NotBeNull();
        binding.ConverterParameter.Should().Be(0.75);
    }
}
