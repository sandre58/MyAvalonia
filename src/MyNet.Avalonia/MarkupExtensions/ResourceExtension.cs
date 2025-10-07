// -----------------------------------------------------------------------
// <copyright file="ResourceExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using MyNet.Humanizer;
using MyNet.Observable.Translatables;

namespace MyNet.Avalonia.MarkupExtensions;

public class ResourceExtension : MarkupExtension
{
    static ResourceExtension() => ResourceLocator.Initialize();

    public ResourceExtension(string key) => Key = key;

    public ResourceExtension(string key, string filename)
    {
        Key = key;
        Filename = filename;
    }

    [ConstructorArgument("key")]
    public string Key { get; set; }

    [ConstructorArgument("filename")]
    public string? Filename { get; set; }

    public LetterCasing Casing { get; set; } = LetterCasing.Normal;

    public override object ProvideValue(IServiceProvider serviceProvider)
        => new Binding(nameof(StringTranslatable.Value))
        {
            Source = new StringTranslatable(Key, Casing, Filename)
        };
}
