// -----------------------------------------------------------------------
// <copyright file="IconsHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Material.Icons;
using MyNet.Avalonia.Controls.Icons;

namespace MyNet.Avalonia.Controls.Helpers;

[Obsolete("Use MyNet.Avalonia.Controls.Icons.MaterialIconCatalog instead.")]
public static class IconsHelper
{
    public static ICollection<MaterialIconKindGroup> Groups => MaterialIconCatalog.Groups;

    public static ICollection<MaterialIconKind> Kinds => MaterialIconCatalog.Kinds;
}
