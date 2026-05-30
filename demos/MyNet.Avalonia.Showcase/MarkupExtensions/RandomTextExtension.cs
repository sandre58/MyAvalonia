// -----------------------------------------------------------------------
// <copyright file="RandomTextExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Markup.Xaml;
using MyNet.Utilities.Generator;

namespace MyNet.Avalonia.Showcase.MarkupExtensions;

internal sealed class RandomTextExtension : MarkupExtension
{
    public int MinWords { get; set; } = 8;

    public int MaxWords { get; set; } = 12;

    public int MinSentences { get; set; } = 1;

    public int MaxSentences { get; set; } = 2;

    public override object ProvideValue(IServiceProvider serviceProvider)
        => SentenceGenerator.Paragraph(RandomGenerator.Int(MinWords, MaxWords), RandomGenerator.Int(MinSentences, MaxSentences));
}
