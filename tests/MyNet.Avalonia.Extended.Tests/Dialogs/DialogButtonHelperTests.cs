// -----------------------------------------------------------------------
// <copyright file="DialogButtonHelperTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using FluentAssertions;
using MyNet.Avalonia.Extended.Controls;
using MyNet.UI.Dialogs.MessageBox;
using Xunit;

namespace MyNet.Avalonia.Extended.Tests.Dialogs;

public class DialogButtonHelperTests
{
    [Theory]
    [InlineData(MessageBoxResultOption.Ok, MessageBoxResult.Ok)]
    [InlineData(MessageBoxResultOption.OkCancel, MessageBoxResult.Ok)]
    [InlineData(MessageBoxResultOption.YesNo, MessageBoxResult.Yes)]
    [InlineData(MessageBoxResultOption.YesNoCancel, MessageBoxResult.Yes)]
    public void GetAffirmativeResult_ReturnsExpected(MessageBoxResultOption buttons, MessageBoxResult expected)
        => DialogButtonHelper.GetAffirmativeResult(buttons).Should().Be(expected);

    [Theory]
    [InlineData(MessageBoxResultOption.OkCancel, MessageBoxResult.Cancel)]
    [InlineData(MessageBoxResultOption.YesNo, MessageBoxResult.No)]
    [InlineData(MessageBoxResultOption.YesNoCancel, MessageBoxResult.Cancel)]
    public void GetDefaultCloseResult_ReturnsDismissAction(MessageBoxResultOption buttons, MessageBoxResult expected)
        => DialogButtonHelper.GetDefaultCloseResult(buttons).Should().Be(expected);
}
