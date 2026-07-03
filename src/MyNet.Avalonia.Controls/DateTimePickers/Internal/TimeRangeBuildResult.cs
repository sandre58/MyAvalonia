// -----------------------------------------------------------------------
// <copyright file="TimeRangeBuildResult.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Primitives.Intervals;

namespace MyNet.Avalonia.Controls.DateTimePickers.Internal;

internal readonly record struct TimeRangeBuildResult(Period? Period, bool IsValid);
