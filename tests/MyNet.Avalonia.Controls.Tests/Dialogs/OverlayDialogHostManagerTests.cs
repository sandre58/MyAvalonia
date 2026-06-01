// -----------------------------------------------------------------------
// <copyright file="OverlayDialogHostManagerTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using FluentAssertions;
using MyNet.Avalonia.Controls;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Dialogs;

public class OverlayDialogHostManagerTests
{
    [Fact]
    public void GetTopLevelKey_ReturnsNullForNullTopLevel()
    {
        OverlayDialogHostManager.GetTopLevelKey(null).Should().BeNull();
    }
}
