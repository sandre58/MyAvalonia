// -----------------------------------------------------------------------
// <copyright file="PickerValueValidationErrorEventArgs.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MyNet.Avalonia.Controls.Primitives;

public class PickerValueValidationErrorEventArgs(Exception exception, string text) : EventArgs
{
    // Summary:
    //     Gets the initial exception associated with the Avalonia.Controls.CalendarDatePicker.DateValidationError
    //     event.
    //
    // Value:
    //     The exception associated with the validation failure.
    public Exception Exception { get; } = exception;

    // Summary:
    //     Gets the text that caused the Avalonia.Controls.CalendarDatePicker.DateValidationError
    //     event.
    //
    // Value:
    //     The text that caused the validation failure.
    public string Text { get; } = text;

    // Summary:
    //     Gets or sets a value indicating whether Avalonia.Controls.CalendarDatePickerDateValidationErrorEventArgs.Exception
    //     should be thrown.
    //
    // Value:
    //     True if the exception should be thrown; otherwise, false.
    //
    // Exceptions:
    //   T:System.ArgumentException:
    //     If set to true and Avalonia.Controls.CalendarDatePickerDateValidationErrorEventArgs.Exception
    //     is null.
    public bool ThrowException
    {
        get;
        set
        {
            if (value && Exception == null)
            {
                throw new ArgumentException("Cannot Throw Null Exception");
            }

            field = value;
        }
    }
}
