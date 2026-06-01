// -----------------------------------------------------------------------
// <copyright file="ClassDiffEngineTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using FluentAssertions;
using MyNet.Avalonia.Theme.Classes.Engine;
using MyNet.Avalonia.Theme.Classes.Registry;
using Xunit;

namespace MyNet.Avalonia.Theme.Tests.Classes;

public class ClassDiffEngineTests
{
    private const string TestClassName = "p1-test-utility-class";

    static ClassDiffEngineTests() => ClassRegistry.Register<Border>(TestClassName, border =>
    {
        border.Tag = "applied";
        return new TestRegistration(() => border.Tag = null);
    });

    [Fact]
    public void ApplyDiff_AddsRegisteredClass_InvokesRegistryAction()
    {
        var border = new Border();
        var state = new ClassesRuntimeState();

        ClassDiffEngine.ApplyDiff(border, state, [TestClassName]);

        border.Tag.Should().Be("applied");
        state.ActiveActions.Should().ContainKey(TestClassName);
    }

    [Fact]
    public void ApplyDiff_RemovesClass_DisposesActiveAction()
    {
        var border = new Border();
        var state = new ClassesRuntimeState();

        ClassDiffEngine.ApplyDiff(border, state, [TestClassName]);
        ClassDiffEngine.ApplyDiff(border, state, []);

        border.Tag.Should().BeNull();
        state.ActiveActions.Should().BeEmpty();
    }

    [Fact]
    public void ApplyDiff_UnknownClass_DoesNotRegisterAction()
    {
        var border = new Border();
        var state = new ClassesRuntimeState();

        ClassDiffEngine.ApplyDiff(border, state, ["not-a-registered-class"]);

        state.ActiveActions.Should().BeEmpty();
    }

    private sealed class TestRegistration(Action onDispose) : IDisposable
    {
        public void Dispose() => onDispose();
    }
}
