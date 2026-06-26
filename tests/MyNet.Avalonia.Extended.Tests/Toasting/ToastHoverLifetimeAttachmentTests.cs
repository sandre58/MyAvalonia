// -----------------------------------------------------------------------
// <copyright file="ToastHoverLifetimeAttachmentTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using FluentAssertions;
using MyNet.Avalonia.Extended.Toasting;
using MyNet.UI.Notifications.Models;
using MyNet.UI.Toasting.Models;
using MyNet.UI.Toasting.Settings;
using Xunit;

namespace MyNet.Avalonia.Extended.Tests.Toasting;

public class ToastHoverLifetimeAttachmentTests
{
    [Theory]
    [InlineData(ToastClosingStrategy.AutoClose, true, true)]
    [InlineData(ToastClosingStrategy.Both, true, true)]
    [InlineData(ToastClosingStrategy.AutoClose, false, false)]
    [InlineData(ToastClosingStrategy.CloseButton, true, false)]
    [InlineData(ToastClosingStrategy.None, true, false)]
    public void ShouldAttach_RequiresFreezeOnMouseEnterAndAutoCloseStrategy(
        ToastClosingStrategy closingStrategy,
        bool freezeOnMouseEnter,
        bool expected)
    {
        var toast = CreateToast(closingStrategy, freezeOnMouseEnter);

        ToastHoverLifetimeAttachment.ShouldAttach(toast).Should().Be(expected);
    }

    private static Toast CreateToast(ToastClosingStrategy closingStrategy, bool freezeOnMouseEnter)
        => new(
            new MessageNotification("message", severity: NotificationSeverity.Information),
            new()
            {
                ClosingStrategy = closingStrategy,
                FreezeOnMouseEnter = freezeOnMouseEnter
            });
}
