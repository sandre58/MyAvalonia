// -----------------------------------------------------------------------
// <copyright file="CalendarDayCellState.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.Avalonia.Controls.Primitives;

namespace MyNet.Avalonia.Controls.Internals.Calendar;

internal readonly record struct CalendarDayCellState(DayContext DateContext, DateTime Date, bool IsInactive);
