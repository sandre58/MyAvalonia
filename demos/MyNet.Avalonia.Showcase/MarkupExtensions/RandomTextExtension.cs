// -----------------------------------------------------------------------
// <copyright file="RandomTextExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Markup.Xaml;
using MyNet.Fakers.Static;

namespace MyNet.Avalonia.Showcase.MarkupExtensions;

internal sealed class RandomTextExtension : MarkupExtension
{
    public int MinWords { get; set; } = 8;

    public int MaxWords { get; set; } = 12;

    public int? MinSentences { get; set; }

    public int? MaxSentences { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (MinSentences.HasValue || MaxSentences.HasValue)
        {
            var minSentences = MinSentences ?? 1;
            var maxSentences = MaxSentences ?? Math.Max(minSentences, 2);
            return Faker.Texts.Paragraph(minSentences, maxSentences);
        }

        return Faker.Texts.Words(MinWords, MaxWords);
    }
}
