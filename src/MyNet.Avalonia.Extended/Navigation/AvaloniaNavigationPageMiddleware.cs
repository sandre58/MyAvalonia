// -----------------------------------------------------------------------
// <copyright file="AvaloniaNavigationPageMiddleware.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using MyNet.UI.Locators;
using MyNet.UI.Navigation;
using MyNet.UI.Navigation.Models;

namespace MyNet.Avalonia.Extended.Navigation;

/// <summary>
/// Updates the Avalonia navigation page stack before the navigation pipeline commits.
/// </summary>
public sealed class AvaloniaNavigationPageMiddleware(
    IAvaloniaNavigationPageHost host,
    IAvaloniaPageFactory pageFactory) : INavigationMiddleware
{
    /// <inheritdoc />
    public async Task<NavigationResult> InvokeAsync(
        NavigationContext? from,
        NavigationContext to,
        Func<Task<NavigationResult>> next,
        CancellationToken cancellationToken)
    {
        try
        {
            switch (to.Mode)
            {
                case NavigationMode.Back:
                    host.Pop();
                    break;

                case NavigationMode.Normal:
                case NavigationMode.Forward:
                    host.Push(pageFactory.Create(to.To));
                    break;
            }
        }
        catch (ViewResolutionException ex)
        {
            return new(NavigationStatus.Failed, ex.Message, ex);
        }
        catch (Exception ex)
        {
            return new(NavigationStatus.Failed, ex.Message, ex);
        }

        return await next().ConfigureAwait(false);
    }
}
