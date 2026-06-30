// -----------------------------------------------------------------------
// <copyright file="TextPicker.Commit.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using System.Text;
using Avalonia.Controls;
using MyNet.Avalonia.Controls.Localization;
using MyNet.Avalonia.Controls.Primitives.Internal;

namespace MyNet.Avalonia.Controls.Primitives;

public abstract partial class TextPicker<T, TPreviewer>
{
    private static readonly CompositeFormat InvalidFormat = CompositeFormat.Parse(MessagesResources.InvalidFormatError);

    private T? TryParseText(string? text)
    {
        var result = TextPickerValidationHelper.Parse(text, ConvertValueFromString, IsValidValue);

        switch (result.Status)
        {
            case TextPickerParseStatus.Empty:
                return default;

            case TextPickerParseStatus.Success:
                return result.Value;

            case TextPickerParseStatus.InvalidValue:
                ReportInvalidValue(text);
                return default;

            case TextPickerParseStatus.FormatError:
                ReportFormatError(text, result.Error!);
                return default;

            default:
                return default;
        }
    }

    private void ReportInvalidValue(string? text)
    {
        var errorMessage = MessagesResources.InvalidValueError;
        var valueValidationError = new PickerValueValidationErrorEventArgs(new ArgumentOutOfRangeException(nameof(text), errorMessage), text);
        OnValueValidationError(valueValidationError);

        DataValidationErrors.SetError(this, valueValidationError.Exception);

        if (valueValidationError.ThrowException)
            throw valueValidationError.Exception;
    }

    private void ReportFormatError(string? text, Exception innerException)
    {
        var ex = new FormatException(string.Format(CultureInfo.CurrentCulture, InvalidFormat, text), innerException);
        var textParseError = new PickerValueValidationErrorEventArgs(ex, text);
        OnValueValidationError(textParseError);

        DataValidationErrors.SetError(this, textParseError.Exception);

        if (textParseError.ThrowException)
            throw textParseError.Exception;
    }

    private void CommitFromTextBox()
    {
        DataValidationErrors.ClearErrors(this);

        if (TextBox == null) return;

        var text = TextBox.Text;
        var action = TextPickerCommitHelper.ResolveCommitAction(
            text,
            SelectedValue is not null ? ConvertValueToString(SelectedValue) : null,
            SelectedValue is not null);

        switch (action)
        {
            case TextPickerTextCommitKind.ClearValue:
                SetCurrentValue(SelectedValueProperty, null);
                break;

            case TextPickerTextCommitKind.ParseAndApply:
                var parsedValue = TryParseText(text);
                if (TextPickerCommitHelper.ShouldApplyParsedValue(parsedValue, SelectedValue))
                    SetCurrentValue(SelectedValueProperty, parsedValue);
                break;
        }
    }

    protected virtual void OnPreviewValueChanged()
    {
        if (_previewValueChangedSuspender.IsSuspended) return;

        if (AutoCommit)
            CommitFromPreview();

        if (ShouldCloseAfterSingleSelection())
            CloseAfterSingleSelection();
    }
}
