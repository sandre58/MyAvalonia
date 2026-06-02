// -----------------------------------------------------------------------
// <copyright file="GlobalizationBindingSourceTests.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MyNet.Avalonia;
using MyNet.Avalonia.Bindings;
using MyNet.Globalization;
using MyNet.Globalization.Culture;
using Xunit;

namespace MyNet.Avalonia.Tests.Bindings;

public class GlobalizationBindingSourceTests
{
    [Fact]
    public void UseAvaloniaGlobalization_ReconnectsBindingSourceToConfiguredCultureService()
    {
        var services = new ServiceCollection()
            .AddGlobalization()
            .AddLocalization()
            .BuildServiceProvider();

        _ = GlobalizationBindingSource.Instance.Culture;

        services.UseAvaloniaGlobalization();
        services.UseLocalization();

        var cultureService = services.GetRequiredService<ICultureService>();
        var targetCulture = cultureService.CurrentCulture.Name == SupportedCultures.French.Name
            ? SupportedCultures.English
            : SupportedCultures.French;

        var notifications = 0;
        GlobalizationBindingSource.Instance.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(GlobalizationBindingSource.Culture))
                notifications++;
        };

        cultureService.SetCulture(targetCulture);

        notifications.Should().Be(1);
        GlobalizationBindingSource.Instance.Culture.Name.Should().Be(targetCulture.Name);
    }
}
