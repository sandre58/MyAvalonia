// -----------------------------------------------------------------------
// <copyright file="WindowMessageBoxHeadlessTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Avalonia.Headless.XUnit;
using FluentAssertions;
using MyNet.Avalonia.Extended.Controls;
using MyNet.UI.Dialogs.MessageBox;

namespace MyNet.Avalonia.Extended.Headless.Tests.Dialogs;

public class WindowMessageBoxHeadlessTests
{
    [AvaloniaFact]
    public void WindowMessageBox_OkButton_SetsLastCloseResult()
    {
        var messageBox = new WindowMessageBox(MessageBoxResultOption.OkCancel)
        {
            Content = "Body",
            Title = "Test",
            Width = 360,
            Height = 200
        };

        messageBox.Show();

        var okButton = HeadlessControlHost.FindByName<Button>(messageBox, WindowMessageBox.PartOkButton);
        okButton.Should().NotBeNull();
        HeadlessControlHost.Click(okButton);

        messageBox.LastCloseResult.Should().Be(MessageBoxResult.Ok);
    }
}
