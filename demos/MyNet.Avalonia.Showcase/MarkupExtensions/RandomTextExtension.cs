// -----------------------------------------------------------------------
// <copyright file="RandomTextExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Markup.Xaml;
using MyNet.Fakers.Static;
using MyNet.Generator.Facade;

namespace MyNet.Avalonia.Showcase.MarkupExtensions;

internal sealed class RandomTextExtension : MarkupExtension
{
    public int MinWords { get; set; } = 8;

    public int MaxWords { get; set; } = 12;

    public int MinSentences { get; set; } = 1;

    public int MaxSentences { get; set; } = 2;

    public override object ProvideValue(IServiceProvider serviceProvider)
        => Faker.Texts.Paragraph(RandomGenerator.Current.Int(MinWords, MaxWords), RandomGenerator.Current.Int(MinSentences, MaxSentences));
}
