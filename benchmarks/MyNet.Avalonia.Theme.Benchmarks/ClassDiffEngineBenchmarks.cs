// -----------------------------------------------------------------------
// <copyright file="ClassDiffEngineBenchmarks.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using BenchmarkDotNet.Attributes;
using MyNet.Avalonia.Theme.Classes.Engine;
using MyNet.Avalonia.Theme.Classes.Registry;

namespace MyNet.Avalonia.Theme.Benchmarks;

[MemoryDiagnoser]
internal class ClassDiffEngineBenchmarks
{
    private const string BenchmarkClass = "benchmark-utility-class";

    private Border _border = null!;
    private ClassesRuntimeState _state = null!;

    [GlobalSetup]
    public void Setup()
    {
        ClassRegistry.Register<Border>(BenchmarkClass, border =>
        {
            border.Tag = "applied";
            return new BenchmarkRegistration(() => border.Tag = null);
        });

        _border = new();
        _state = new();
        ClassDiffEngine.ApplyDiff(_border, _state, [BenchmarkClass]);
    }

    [Benchmark]
    public void ApplyDiff_NoChange() => ClassDiffEngine.ApplyDiff(_border, _state, [BenchmarkClass]);

    [Benchmark]
    public void ApplyDiff_RemoveAndReadd()
    {
        ClassDiffEngine.ApplyDiff(_border, _state, []);
        ClassDiffEngine.ApplyDiff(_border, _state, [BenchmarkClass]);
    }

    private sealed class BenchmarkRegistration(Action onDispose) : IDisposable
    {
        public void Dispose() => onDispose();
    }
}
