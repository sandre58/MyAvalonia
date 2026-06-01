// -----------------------------------------------------------------------
// <copyright file="GlobalizationTestCollection.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace MyNet.Avalonia.Controls.Tests.Infrastructure;

[CollectionDefinition(Name)]
[SuppressMessage("Naming", "CA1711:Les identificateurs ne doivent pas avoir un suffixe incorrect", Justification = "Test collection definition")]
#pragma warning disable CA1515 // xUnit collection definitions must be public
public sealed class GlobalizationTestCollection : ICollectionFixture<GlobalizationTestFixture>
#pragma warning restore CA1515
{
    public const string Name = "Globalization";
}
