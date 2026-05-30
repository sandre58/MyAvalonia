// -----------------------------------------------------------------------
// <copyright file="PulseEasing.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Avalonia.Animation.Easings;

namespace MyNet.Avalonia.Controls.Animations;

public class PulseEasing : Easing
{
    private const int StepsCount = 8;

    private static readonly IEnumerable<double> Steps =
    [
        .. Enumerable
            .Range(0, StepsCount + 1)
            .Select(index => 1.0 / StepsCount * index)
    ];

    public override double Ease(double progress) => Steps.Last(step => step <= progress);
}
