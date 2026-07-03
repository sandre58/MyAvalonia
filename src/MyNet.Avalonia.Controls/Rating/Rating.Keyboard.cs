// -----------------------------------------------------------------------
// <copyright file="Rating.Keyboard.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Input;
using MyNet.Avalonia.Controls.Internals;

namespace MyNet.Avalonia.Controls;

public partial class Rating
{
    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.Handled || !CanEdit() || e.KeyModifiers != KeyModifiers.None)
        {
            base.OnKeyDown(e);
            return;
        }

        if (ProcessKeyboardKey(e))
            e.Handled = true;

        base.OnKeyDown(e);
    }

    private bool ProcessKeyboardKey(KeyEventArgs e)
    {
        var minimum = GetEffectiveMinimum();
        var maximum = GetEffectiveMaximum();
        var step = RatingValueHelper.GetStep(Precision);

        switch (e.Key)
        {
            case Key.Right:
            case Key.Up:
                CommitValue(RatingValueHelper.Increment(Value, step, Precision, minimum, maximum));
                return true;

            case Key.Left:
            case Key.Down:
                CommitValue(RatingValueHelper.Increment(Value, -step, Precision, minimum, maximum));
                return true;

            case Key.Home:
                CommitValue(minimum);
                return true;

            case Key.End:
                CommitValue(maximum);
                return true;

            case Key.D0:
            case Key.NumPad0:

            case Key.Back:
            case Key.Delete:
                if (!IsClearable)
                    return false;

                CommitValue(minimum);
                return true;

            case Key.D1:
            case Key.NumPad1:
                return TryCommitDigit(1, maximum);

            case Key.D2:
            case Key.NumPad2:
                return TryCommitDigit(2, maximum);

            case Key.D3:
            case Key.NumPad3:
                return TryCommitDigit(3, maximum);

            case Key.D4:
            case Key.NumPad4:
                return TryCommitDigit(4, maximum);

            case Key.D5:
            case Key.NumPad5:
                return TryCommitDigit(5, maximum);

            case Key.D6:
            case Key.NumPad6:
                return TryCommitDigit(6, maximum);

            case Key.D7:
            case Key.NumPad7:
                return TryCommitDigit(7, maximum);

            case Key.D8:
            case Key.NumPad8:
                return TryCommitDigit(8, maximum);

            case Key.D9:
            case Key.NumPad9:
                return TryCommitDigit(9, maximum);

            default:
                return false;
        }
    }

    private bool TryCommitDigit(int digit, double maximum)
    {
        var value = System.Math.Min(digit, maximum);
        CommitValue(value);
        return true;
    }
}
