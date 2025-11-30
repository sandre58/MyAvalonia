// -----------------------------------------------------------------------
// <copyright file="IIncrementableControl.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Controls.Primitives;

public interface IIncrementableControl
{
    bool Increment(int value);

    bool IncrementLarge(int value);
}
