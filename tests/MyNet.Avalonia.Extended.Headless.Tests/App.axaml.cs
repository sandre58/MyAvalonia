// -----------------------------------------------------------------------
// <copyright file="App.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Markup.Xaml;

namespace MyNet.Avalonia.Extended.Headless.Tests;

[SuppressMessage("Maintainability", "CA1515:Envisager de rendre les types publics internes", Justification = "Used by Avalonia XAML")]
[SuppressMessage("ReSharper", "PartialTypeWithSinglePart", Justification = "Used by Avalonia XAML")]
public partial class ExtendedHeadlessTestApp : Application
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);
}
