// -----------------------------------------------------------------------
// <copyright file="BrushManagerBenchmarks.cs" company="Stéphane ANDRE">
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
internal class BrushManagerBenchmarks
{
    private BrushManager _manager = null!;
    private ColorInterpolation _halfOpacity = null!;
    private ColorInterpolation _empty = null!;
    private SolidColorBrush _registeredBrush = null!;

    [GlobalSetup]
    public void Setup()
    {
        _manager = new(null, null);
        _manager.Register("MyNet.Brush.Benchmark", MediaColors.SteelBlue);
        _halfOpacity = new(0.5);
        _empty = new();
        _registeredBrush = _manager.Register("MyNet.Brush.Benchmark.Second", MediaColors.Coral);
        _manager.Get("MyNet.Brush.Benchmark", _halfOpacity);
    }

    [Benchmark]
    public IBrush GetByKey_MainBrush() => _manager.Get("MyNet.Brush.Benchmark", _empty);

    [Benchmark]
    public IBrush GetByKey_CachedOpacity() => _manager.Get("MyNet.Brush.Benchmark", _halfOpacity);

    [Benchmark]
    public IBrush GetByKey_ColdOpacity() => _manager.Get("MyNet.Brush.Benchmark", new(0.37));

    [Benchmark]
    public IBrush GetByBrushInstance() => _manager.Get(_registeredBrush, _empty);
}
