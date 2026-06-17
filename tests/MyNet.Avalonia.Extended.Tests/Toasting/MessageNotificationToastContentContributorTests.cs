// -----------------------------------------------------------------------
// <copyright file="MessageNotificationToastContentContributorTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using FluentAssertions;
using Material.Icons;
using MyNet.Avalonia.Extended.Controls;
using MyNet.Avalonia.Extended.Toasting;
using MyNet.UI.Notifications.Models;
using Xunit;

namespace MyNet.Avalonia.Extended.Tests.Toasting;

public class MessageNotificationToastContentContributorTests
{
    private readonly MessageNotificationToastContentContributor _contributor = new();

    [Fact]
    public void TryCreateContent_ReturnsFalse_ForNonMessageNotification()
    {
        var result = _contributor.TryCreateContent(new StubNotification(), null, out var content);

        result.Should().BeFalse();
        content.Should().BeNull();
    }

    [Theory]
    [InlineData(NotificationSeverity.Success, MaterialIconKind.Success)]
    [InlineData(NotificationSeverity.Warning, MaterialIconKind.AlertCircle)]
    [InlineData(NotificationSeverity.Error, MaterialIconKind.CloseCircle)]
    [InlineData(NotificationSeverity.Information, MaterialIconKind.InformationVariantCircle)]
    public void TryCreateContent_MapsSeverityToLeadingIcon(NotificationSeverity severity, MaterialIconKind expectedKind)
    {
        var result = _contributor.TryCreateContent(
            new MessageNotification("Body", "Title", severity),
            320,
            out var content);

        result.Should().BeTrue();
        var control = content.Should().BeOfType<MessageNotificationControl>().Subject;
        control.Header.Should().Be("Title");
        control.Content.Should().Be("Body");
        control.Width.Should().Be(320);
        control.Leading.Should().Be(expectedKind);
    }

    [Fact]
    public void TryCreateContent_OmitsLeading_ForUnmappedSeverity()
    {
        _contributor.TryCreateContent(
            new MessageNotification("Body only", severity: (NotificationSeverity)99),
            null,
            out var content);

        var control = content.Should().BeOfType<MessageNotificationControl>().Subject;
        control.Leading.Should().BeNull();
        control.Content.Should().Be("Body only");
    }

    private sealed class StubNotification : INotification
    {
        public Guid Id { get; } = Guid.NewGuid();

        public string Title => string.Empty;

        public string Message => "stub";

        public NotificationSeverity Severity => (NotificationSeverity)99;

        public DateTime Timestamp { get; } = DateTime.UtcNow;

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
