// -----------------------------------------------------------------------
// <copyright file="LeadingPresentation.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Controls.Enums;

/// <summary>
/// Defines how the <see cref="RegionControl.Leading"/> region is rendered on a <see cref="Card"/>.
/// </summary>
public enum LeadingPresentation
{
    /// <summary>
    /// Icon on a rounded tonal badge — settings rows and dense tiles.
    /// </summary>
    Badge,

    /// <summary>
    /// Plain icon or avatar without a badge background.
    /// </summary>
    Plain,

    /// <summary>
    /// Media fills the leading slot — use with <see cref="CardLayout.MediaTop"/> or <see cref="CardLayout.MediaLeft"/>.
    /// </summary>
    Hero,

    /// <summary>
    /// Hides the leading slot entirely.
    /// </summary>
    None,
}
