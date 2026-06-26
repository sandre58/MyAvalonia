// -----------------------------------------------------------------------
// <copyright file="SingletonNavigationMiddlewareTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MyNet.Avalonia.Extended.Navigation;
using MyNet.UI.Locators;
using MyNet.UI.Navigation;
using MyNet.UI.Navigation.Models;
using Xunit;

namespace MyNet.Avalonia.Extended.Tests.Navigation;

public class SingletonNavigationMiddlewareTests
{
    [Fact]
    public async Task Reactivating_existing_page_rewinds_journal_without_growing_history()
    {
        var home = new TestNavigationPage("home");
        var theme = new TestNavigationPage("theme");
        var form = new TestNavigationPage("form");

        var host = new FakeNavigationPageHost();
        var pageFactory = new TestPageFactory();

        var services = new ServiceCollection();
        services.AddViewLocators();
        services.AddNavigation();
        services.RemoveAll<INavigationService>();
        services.AddSingleton<NavigationService>();
        services.AddSingleton<INavigationService, SingletonNavigationService>();
        services.AddSingleton<IAvaloniaNavigationPageHost>(host);
        services.AddSingleton<IAvaloniaPageFactory>(pageFactory);
        services.AddSingleton<INavigationMiddleware, AvaloniaNavigationPageMiddleware>();
        var provider = services.BuildServiceProvider();
        var navigation = provider.GetRequiredService<INavigationService>();
        var journal = provider.GetRequiredService<INavigationJournal>();

        await navigation.NavigateToAsync(home);
        await navigation.NavigateToAsync(theme);
        await navigation.NavigateToAsync(form);

        host.Stack.Should().Equal(home, theme, form);
        journal.BackStack.Should().HaveCount(2);

        await navigation.NavigateToAsync(theme);

        host.Stack.Should().Equal(home, theme);
        journal.BackStack.Should().HaveCount(1);
        navigation.CurrentContext!.To.Should().BeSameAs(theme);
        navigation.CanGoBack.Should().BeTrue();
    }

    [Fact]
    public async Task Repeated_reactivation_then_go_back_stays_aligned()
    {
        var home = new TestNavigationPage("home");
        var theme = new TestNavigationPage("theme");
        var form = new TestNavigationPage("form");

        var host = new FakeNavigationPageHost();
        var pageFactory = new TestPageFactory();

        var services = new ServiceCollection();
        services.AddViewLocators();
        services.AddNavigation();
        services.RemoveAll<INavigationService>();
        services.AddSingleton<NavigationService>();
        services.AddSingleton<INavigationService, SingletonNavigationService>();
        services.AddSingleton<IAvaloniaNavigationPageHost>(host);
        services.AddSingleton<IAvaloniaPageFactory>(pageFactory);
        services.AddSingleton<INavigationMiddleware, AvaloniaNavigationPageMiddleware>();
        var provider = services.BuildServiceProvider();
        var navigation = provider.GetRequiredService<INavigationService>();

        await navigation.NavigateToAsync(home);
        await navigation.NavigateToAsync(theme);
        await navigation.NavigateToAsync(form);
        await navigation.NavigateToAsync(theme);
        await navigation.NavigateToAsync(form);

        host.Stack.Should().Equal(home, theme, form);
        navigation.CurrentContext!.To.Should().BeSameAs(form);

        (await navigation.GoBackAsync()).Status.Should().Be(NavigationStatus.Succeeded);
        host.Stack.Should().Equal(home, theme);
        navigation.CurrentContext!.To.Should().BeSameAs(theme);

        (await navigation.GoBackAsync()).Status.Should().Be(NavigationStatus.Succeeded);
        host.Stack.Should().Equal(home);
        navigation.CurrentContext!.To.Should().BeSameAs(home);
        navigation.CanGoBack.Should().BeFalse();
    }

    private sealed class FakeNavigationPageHost : IAvaloniaNavigationPageHost
    {
        private readonly List<INavigationPage> _stack = [];
        private int _skipAvaloniaBackPopCount;

        public IReadOnlyList<INavigationPage> Stack => _stack;

        public bool IsAttached => true;

        public void Attach(NavigationPage navigationPage)
        {
        }

        public void Push(Page view) => _stack.Add((INavigationPage)view.DataContext!);

        public bool Contains(Page view) => GetStackDistance(view) >= 0;

        public int GetStackDistance(Page view)
        {
            var page = (INavigationPage)view.DataContext!;
            for (var i = _stack.Count - 1; i >= 0; i--)
            {
                if (!ReferenceEquals(_stack[i], page))
                    continue;

                return _stack.Count - 1 - i;
            }

            return -1;
        }

        public void PopTo(Page view, int distance)
        {
            if (distance <= 0)
                return;

            _stack.RemoveRange(_stack.Count - distance, distance);
        }

        public void Pop()
        {
            if (_skipAvaloniaBackPopCount > 0)
            {
                _skipAvaloniaBackPopCount--;
                return;
            }

            if (_stack.Count > 0)
                _stack.RemoveAt(_stack.Count - 1);
        }

        public void SuppressAvaloniaBackPops(int count)
        {
            if (count > 0)
                _skipAvaloniaBackPopCount += count;
        }

        public void Clear() => _stack.Clear();

        public bool TryConsumeProgrammaticPop() => false;
    }

    private sealed class TestPageFactory : IAvaloniaPageFactory
    {
        private readonly Dictionary<INavigationPage, Page> _pages = new(ReferenceEqualityComparer.Instance);

        public Page Create(INavigationPage page)
        {
            if (!_pages.TryGetValue(page, out var view))
            {
                view = new ContentPage { DataContext = page };
                _pages[page] = view;
            }

            return view;
        }

        public void Clear() => _pages.Clear();
    }

    private sealed class TestNavigationPage(string name) : INavigationPage
    {
        public Task OnNavigatingToAsync(NavigationContext context, System.Threading.CancellationToken cancellationToken)
            => Task.CompletedTask;

        public Task OnNavigatedAsync(NavigationContext context, System.Threading.CancellationToken cancellationToken)
            => Task.CompletedTask;

        public Task OnNavigatingFromAsync(NavigationContext context, System.Threading.CancellationToken cancellationToken)
            => Task.CompletedTask;

        public override string ToString() => name;
    }
}
