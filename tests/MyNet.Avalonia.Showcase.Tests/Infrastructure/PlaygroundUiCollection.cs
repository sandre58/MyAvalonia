// -----------------------------------------------------------------------
// <copyright file="PlaygroundUiCollection.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace MyNet.Avalonia.Showcase.Tests.Infrastructure;

[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "This class is intended to define collection names for playground UI tests.")]
internal static class PlaygroundUiCollectionNames
{
    public const string Name = "Playground UI";
}

[CollectionDefinition(PlaygroundUiCollectionNames.Name, DisableParallelization = true)]
[SuppressMessage("Maintainability", "CA1515:Envisager de rendre les types publics internes", Justification = "This collection definition is intended to be used across multiple test classes within the same assembly, and making it internal would limit its accessibility.")]
public sealed class PlaygroundUiTestGroupDefinition;
