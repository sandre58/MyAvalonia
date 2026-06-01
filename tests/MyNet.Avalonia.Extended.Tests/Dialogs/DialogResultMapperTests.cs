// -----------------------------------------------------------------------
// <copyright file="DialogResultMapperTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using FluentAssertions;
using MyNet.Avalonia.Extended.Dialogs.Internal;
using MyNet.UI.Dialogs.ContentDialogs;
using MyNet.UI.Dialogs.MessageBox;
using Xunit;

namespace MyNet.Avalonia.Extended.Tests.Dialogs;

public class DialogResultMapperTests
{
    [Theory]
    [InlineData(MessageBoxResult.Ok)]
    [InlineData(MessageBoxResult.Yes)]
    public void Map_MessageBoxAffirmative_ReturnsOk(MessageBoxResult result) => DialogResultMapper.Map(result).IsSuccess.Should().BeTrue();

    [Theory]
    [InlineData(MessageBoxResult.Cancel)]
    [InlineData(MessageBoxResult.No)]
    public void Map_MessageBoxNegative_ReturnsCancel(MessageBoxResult result)
    {
        var mapped = DialogResultMapper.Map(result);
        mapped.IsSuccess.Should().BeFalse();
        mapped.Should().Be(DialogResult.Cancel());
    }

    [Fact]
    public void Map_BoolTrue_ReturnsOk() => DialogResultMapper.Map(true).IsSuccess.Should().BeTrue();

    [Fact]
    public void Map_BoolFalse_ReturnsCancel() => DialogResultMapper.Map(false).Should().Be(DialogResult.Cancel());

    [Fact]
    public void Map_Null_ReturnsDismiss() => DialogResultMapper.Map(null).Should().Be(DialogResult.Dismiss());
}
