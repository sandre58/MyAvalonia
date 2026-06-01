// -----------------------------------------------------------------------
// <copyright file="BrushSetLruBenchmarks.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Media;
using BenchmarkDotNet.Attributes;
using MyNet.Avalonia.Colors;
using MyNet.Avalonia.Theme.Theming.Brushes;
using MediaColors = Avalonia.Media.Colors;

namespace MyNet.Avalonia.Theme.Benchmarks;

[MemoryDiagnoser]
internal class BrushSetLruBenchmarks
{
    private BrushSet _set = null!;
    private ColorInterpolation _hot = null!;
    private int _missCounter;

    [Params(16, 48)]
    public int Capacity { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _set = new(MediaColors.DodgerBlue, MediaColors.White, transformedBrushCapacity: Capacity);
        _hot = new(0.5);
        _set.GetTransformedBrush(_hot);
    }

    [Benchmark]
    public SolidColorBrush GetTransformed_Hit() => _set.GetTransformedBrush(_hot);

    [Benchmark]
    public SolidColorBrush GetTransformed_Miss()
    {
        var opacity = 0.01 + ((++_missCounter % 99) * 0.01);
        return _set.GetTransformedBrush(new(opacity));
    }
}
