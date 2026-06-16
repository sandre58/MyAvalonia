// -----------------------------------------------------------------------
// <copyright file="NavigationServiceBehaviorTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MyNet.UI.Navigation;
using MyNet.UI.Navigation.Models;
using Xunit;

namespace MyNet.Avalonia.Extended.Tests.Navigation;

public class NavigationServiceBehaviorTests
{
    [Fact]
    public async Task GoBack_rewinds_journal_to_match_singleton_reactivation()
    {
        var home = new TestNavigationPage("home");
        var theme = new TestNavigationPage("theme");
        var form = new TestNavigationPage("form");

        var services = new ServiceCollection();
        services.AddNavigation();
        var provider = services.BuildServiceProvider();
        var navigation = provider.GetRequiredService<INavigationService>();
        var journal = provider.GetRequiredService<INavigationJournal>();

        await navigation.NavigateToAsync(home);
        await navigation.NavigateToAsync(theme);
        await navigation.NavigateToAsync(form);

        journal.BackStack.Should().HaveCount(2);
        navigation.CurrentContext!.To.Should().BeSameAs(form);

        (await navigation.GoBackAsync()).Status.Should().Be(NavigationStatus.Succeeded);

        journal.BackStack.Should().HaveCount(1);
        navigation.CurrentContext!.To.Should().BeSameAs(theme);
    }

    private sealed class TestNavigationPage(string name) : INavigationPage, INavigationLifecycle
    {
        public Task OnNavigatingToAsync(NavigationContext context, CancellationToken cancellationToken)
            => Task.CompletedTask;

        public Task OnNavigatedAsync(NavigationContext context, CancellationToken cancellationToken)
            => Task.CompletedTask;

        public Task OnNavigatingFromAsync(NavigationContext context, CancellationToken cancellationToken)
            => Task.CompletedTask;

        public override string ToString() => name;
    }
}
