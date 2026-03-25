// -----------------------------------------------------------------------
// <copyright file="NavigationService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using Avalonia.Controls;
using MyNet.UI.Navigation.Models;

namespace MyNet.Avalonia.Extended.Navigation;

/// <summary>
/// Provides navigation services for managing navigation between pages in an Avalonia application. Integrates with an
/// Avalonia NavigationPage to synchronize the navigation journal with the visual navigation stack.
/// </summary>
/// <param name="pageResolver">The IPageResolver used to resolve navigation pages to Avalonia page instances.</param>
public class NavigationService(IPageResolver pageResolver) : UI.Navigation.NavigationService
{
    private readonly IPageResolver _pageResolver = pageResolver;
    private NavigationPage? _navigationPage;
    private int _programmaticPopCount;

    /// <summary>
    /// Attaches an Avalonia <see cref="NavigationPage"/> to this service.
    /// Rebuilds the UI stack from the current journal so that any navigation
    /// that occurred before attachment is reflected in the visual tree.
    /// </summary>
    public void AttachNavigationPage(NavigationPage navigationPage)
    {
        _navigationPage?.Popped -= OnPagePopped;

        _navigationPage = navigationPage;
        _navigationPage.Popped += OnPagePopped;

        // Rebuild the Avalonia stack from journal (back entries bottom→top, then current)
        foreach (var entry in GetBackJournal().Reverse())
            _navigationPage.PushAsync(CreatePage(entry.Page));

        if (CurrentContext != null)
            _navigationPage.PushAsync(CreatePage(CurrentContext.Page));
    }

    /// <summary>
    /// Handles user-initiated pops (e.g. swipe-back gesture).
    /// Programmatic pops are tracked by <see cref="_programmaticPopCount"/> and skipped here.
    /// </summary>
    private void OnPagePopped(object? sender, NavigationEventArgs e)
    {
        if (_programmaticPopCount > 0)
        {
            _programmaticPopCount--;
            return;
        }

        if (CanGoBack())
            base.GoBack();
    }

    /// <summary>
    /// Navigates to the specified page, adding it to the navigation journal and updating the visual stack of the attached
    /// <see cref="NavigationPage"/>.
    /// </summary>
    /// <param name="page">The navigation page to navigate to.</param>
    /// <param name="navigationParameters">The parameters to pass to the navigation page.</param>
    /// <returns>True if the navigation was successful; otherwise, false.</returns>
    public override bool NavigateTo(INavigationPage page, NavigationParameters? navigationParameters = null)
    {
        var result = base.NavigateTo(page, navigationParameters);

        if (result && _navigationPage != null)
            _navigationPage.PushAsync(CreatePage(page));

        return result;
    }

    /// <summary>
    /// Goes back in the navigation journal, if possible. This method is called when the user initiates a back navigation action, such as pressing the back button or performing a swipe-back gesture.
    /// </summary>
    /// <returns>True if the navigation was successful; otherwise, false.</returns>
    public override bool GoBack()
    {
        if (!CanGoBack())
            return false;

        var result = base.GoBack();

        if (result && _navigationPage != null)
        {
            _programmaticPopCount++;
            _navigationPage.PopAsync();
        }

        return result;
    }

    /// <summary>
    /// Goes forward in the navigation journal, if possible. This method is called when the user navigates forward to a page that was previously navigated back from.
    /// </summary>
    /// <returns>True if the navigation was successful; otherwise, false.</returns>
    public override bool GoForward()
    {
        if (!CanGoForward())
            return false;

        var result = base.GoForward();

        if (result && _navigationPage != null && CurrentContext != null)
            _navigationPage.PushAsync(CreatePage(CurrentContext.Page));

        return result;
    }

    /// <summary>
    /// Clears the navigation journal, ensuring that only the current page remains. This method is called when the journal is cleared,
    /// and when the user navigates back to the root page.
    /// </summary>
    public override void ClearJournal()
    {
        base.ClearJournal();
        PopAllExceptCurrent();
    }

    /// <summary>
    /// Clears the navigation stack, ensuring that only the current page remains. This method is called when the journal is cleared,
    /// and when the user navigates back to the root page.
    /// </summary>
    public override void Clear()
    {
        base.Clear();
        PopAllExceptCurrent();
    }

    /// <summary>
    /// Removes all pages from the navigation stack except for the currently displayed page.
    /// </summary>
    /// <remarks>This method clears the navigation stack, ensuring that only the current page remains. It
    /// temporarily unsubscribes from the Popped event to prevent event handlers from being triggered during the removal
    /// process.</remarks>
    private void PopAllExceptCurrent()
    {
        if (_navigationPage == null)
            return;

        _navigationPage.Popped -= OnPagePopped;

        while (_navigationPage.NavigationStack.Count > 1)
            _navigationPage.PopAsync();

        _navigationPage.Popped += OnPagePopped;
    }

    /// <summary>
    /// Creates an Avalonia page instance for the given navigation page using the page resolver.
    /// </summary>
    /// <param name="page">The navigation page to create an Avalonia page for.</param>
    /// <returns>The created Avalonia page.</returns>
    private Page CreatePage(INavigationPage page) => _pageResolver.Resolve(page);
}
