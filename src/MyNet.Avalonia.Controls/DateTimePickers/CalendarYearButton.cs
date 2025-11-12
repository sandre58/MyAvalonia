// -----------------------------------------------------------------------
// <copyright file="CalendarYearButton.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls.Metadata;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[PseudoClasses(PseudoClassName.Range, PseudoClassName.Selected, PseudoClassName.Inactive, PseudoClassName.Today)]
public class CalendarYearButton : CalendarDateButton;
