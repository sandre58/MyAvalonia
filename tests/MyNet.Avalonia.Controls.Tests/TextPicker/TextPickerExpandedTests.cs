// -----------------------------------------------------------------------
// <copyright file="TextPickerExpandedTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using FluentAssertions;
using MyNet.Avalonia.Controls.Primitives.Internal;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.TextPicker;

public class TextPickerExpandedTests
{
    [Fact]
    public void Resolve_WhenClosed_DownDecrementsValue()
    {
        var result = TextPickerKeyboardHelper.Resolve(Key.Down, false, true, KeyModifiers.None);

        result.Should().Be(new TextPickerKeyResult(TextPickerKeyAction.IncrementByOffset, -1));
    }

    [Fact]
    public void Resolve_WhenClosed_PageDownDecrementsLarge()
    {
        var result = TextPickerKeyboardHelper.Resolve(Key.PageDown, false, true, KeyModifiers.None);

        result.Should().Be(new TextPickerKeyResult(TextPickerKeyAction.IncrementLargeByOffset, -1));
    }

    [Fact]
    public void Resolve_WhenClosedWithoutValue_ReturnsNone() => TextPickerKeyboardHelper.Resolve(Key.Up, false, false, KeyModifiers.None)
        .Action.Should().Be(TextPickerKeyAction.None);

    [Fact]
    public void ResolveCommitAction_EmptyWithoutValue_IsNoOp() => TextPickerCommitHelper.ResolveCommitAction(string.Empty, null, false)
        .Should().Be(TextPickerTextCommitKind.NoOp);

    [Fact]
    public void Parse_WithNullConverterResult_ReturnsInvalidWhenNotValid()
    {
        var result = TextPickerValidationHelper.Parse<string?>("test", _ => null, v => v is not null);

        result.Status.Should().Be(TextPickerParseStatus.InvalidValue);
    }

    [Fact]
    public void TryHandleTextBoxTab_WhenKeyIsNotTab_ReturnsFalse()
    {
        var textBox = new TextBox();
        var previewer = new Panel();
        var e = new KeyEventArgs { Key = Key.Enter, KeyModifiers = KeyModifiers.None };

        TextPickerPopupFocusHelper.TryHandleTextBoxTab(previewer, textBox, e).Should().BeFalse();
    }

    [Fact]
    public void TryHandleTextBoxTab_WhenTabFromTextBox_InvokesFocusCallback()
    {
        Control? focused = null;
        var textBox = new TextBox();
        var previewer = new Panel();
        textBox.RaiseEvent(new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyDownEvent,
            Key = Key.Tab,
            KeyModifiers = KeyModifiers.None,
        });

        var e = new KeyEventArgs
        {
            RoutedEvent = InputElement.KeyDownEvent,
            Key = Key.Tab,
            KeyModifiers = KeyModifiers.None,
        };

        object? source = textBox;
        typeof(RoutedEventArgs).GetProperty(nameof(RoutedEventArgs.Source))!
            .SetValue(e, source);

        TextPickerPopupFocusHelper.TryHandleTextBoxTab(previewer, textBox, e, c => focused = c).Should().BeTrue();
        focused.Should().BeSameAs(previewer);
    }

    [Fact]
    public void GetTabFocusables_ExcludesNonTabStopControls()
    {
        var root = new Panel();
        var tabStop = new TextBox { Focusable = true, IsTabStop = true };
        var notTabStop = new TextBox { Focusable = true, IsTabStop = false };
        root.Children.Add(tabStop);
        root.Children.Add(notTabStop);

        var focusables = TextPickerPopupFocusHelper.GetTabFocusables(root);
        focusables.Should().ContainSingle().Which.Should().BeSameAs(tabStop);
    }
}
