// -----------------------------------------------------------------------
// <copyright file="AlignClassRegistry.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Layout;
using MyNet.Avalonia.Theme.Assists;

namespace MyNet.Avalonia.Theme.Classes.Registry;

/// <summary>
/// Provides utility methods for registering alignment-related styling options for layoutable controls within the
/// MyNet.Avalonia.Theme.
/// </summary>
/// <remarks>The AlignClassRegistry class enables the registration of horizontal, vertical, and header alignment
/// utilities, supporting both individual and combined alignment scenarios. These utilities facilitate flexible and
/// consistent alignment styling for controls, making it easier to apply CSS-like alignment classes throughout the
/// theme.</remarks>
public static class AlignClassRegistry
{
    /// <summary>
    /// Registers alignment utilities for horizontal and vertical alignments, as well as header alignments. This includes both individual and combined alignments, allowing for flexible styling of layoutable controls in the MyNet.Avalonia.Theme.
    /// </summary>
    public static void Register()
    {
        // Horizontal & vertical alignments
        ClassRegistry.RegisterMany<HorizontalAlignment, Layoutable>(CssPrefix.Alignment, (x, y) => x.SetProperty(Layoutable.HorizontalAlignmentProperty, y));
        ClassRegistry.RegisterMany<VerticalAlignment, Layoutable>(CssPrefix.VerticalAlignment, (x, y) => x.SetProperty(Layoutable.VerticalAlignmentProperty, y));

        // Header alignments
        ClassRegistry.RegisterMany<HorizontalAlignment, Layoutable>(CssPrefix.HeaderAlignment, (x, y) => x.SetProperty(HeaderAssist.HorizontalAlignmentProperty, y));
        ClassRegistry.RegisterMany<VerticalAlignment, Layoutable>(CssPrefix.VerticalHeaderAlignment, (x, y) => x.SetProperty(HeaderAssist.VerticalAlignmentProperty, y));

        // Horizontal & vertical content alignments
        ClassRegistry.RegisterMany<HorizontalAlignment, ContentControl>(CssPrefix.ContentAlignment, (x, y) => x.SetProperty(ContentControl.HorizontalContentAlignmentProperty, y));
        ClassRegistry.RegisterMany<VerticalAlignment, ContentControl>(CssPrefix.VerticalContentAlignment, (x, y) => x.SetProperty(ContentControl.VerticalContentAlignmentProperty, y));

        // Both alignments
        foreach (var horizontal in Enum.GetValues<HorizontalAlignment>())
        {
            foreach (var vertical in Enum.GetValues<VerticalAlignment>())
            {
                var vclass = vertical == VerticalAlignment.Center ? CssSuffix.Middle : vertical.ToString();
                ClassRegistry.Register<Layoutable>(CssClass.Alignment($"{vclass}-{horizontal}"), x => new CompositeDisposable
                    {
                        x.SetProperty(Layoutable.HorizontalAlignmentProperty, horizontal),
                        x.SetProperty(Layoutable.VerticalAlignmentProperty, vertical)
                    });
                ClassRegistry.Register<Layoutable>(CssClass.HeaderAlignment($"{vclass}-{horizontal}"), x => new CompositeDisposable
                    {
                        x.SetProperty(HeaderAssist.HorizontalAlignmentProperty, horizontal),
                        x.SetProperty(HeaderAssist.VerticalAlignmentProperty, vertical)
                    });
                ClassRegistry.Register<ContentControl>(CssClass.ContentAlignment($"{vclass}-{horizontal}"), x => new CompositeDisposable
                    {
                        x.SetProperty(ContentControl.HorizontalContentAlignmentProperty, horizontal),
                        x.SetProperty(ContentControl.VerticalContentAlignmentProperty, vertical)
                    });
            }
        }

        // Mixed alignments
        ClassRegistry.Register<Layoutable>(CssClass.Centered, x => new CompositeDisposable
        {
            x.SetProperty(Layoutable.HorizontalAlignmentProperty, HorizontalAlignment.Center),
            x.SetProperty(HeaderAssist.HorizontalAlignmentProperty, HorizontalAlignment.Center)
        });
        ClassRegistry.Register<Layoutable>(CssClass.VerticalCentered, x => new CompositeDisposable
        {
            x.SetProperty(Layoutable.VerticalAlignmentProperty, VerticalAlignment.Center),
            x.SetProperty(HeaderAssist.VerticalAlignmentProperty, VerticalAlignment.Center)
        });
    }
}
