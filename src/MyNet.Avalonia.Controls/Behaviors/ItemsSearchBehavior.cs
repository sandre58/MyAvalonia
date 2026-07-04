// -----------------------------------------------------------------------
// <copyright file="ItemsSearchBehavior.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Automation;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Styling;
using Avalonia.Threading;
using Avalonia.VisualTree;
using MyNet.Avalonia.Controls.Internal;
using MyNet.Avalonia.Controls.Localization;

namespace MyNet.Avalonia.Controls.Behaviors;

/// <summary>
/// Provides optional in-popup search and item filtering for <see cref="SelectingItemsControl"/> hosts.
/// </summary>
public static class ItemsSearchBehavior
{
    public const string PartSearchBox = "PART_SearchBox";
    public const string PartSearchEmpty = "PART_SearchEmpty";
    public const string PartSearchPlaceholder = "PART_SearchPlaceholder";
    public const string PartSearchItems = "PART_SearchItems";
    public const string PartItemsPresenter = "PART_ItemsPresenter";
    public const string PartPopup = "PART_Popup";

    private sealed class State
    {
        public TextBox? SearchBox { get; set; }

        public TextBlock? SearchEmpty { get; set; }

        public PlaceholderContentControl? SearchPlaceholder { get; set; }

        public ScrollViewer? SearchItemsScrollViewer { get; set; }

        public ItemsPresenter? ItemsPresenter { get; set; }

        public EventHandler<TemplateAppliedEventArgs>? TemplateAppliedHandler { get; set; }

        public EventHandler? DropDownOpenedHandler { get; set; }

        public EventHandler? DropDownClosedHandler { get; set; }

        public EventHandler<KeyEventArgs>? KeyDownHandler { get; set; }

        public bool ContainerHandlersAttached { get; set; }

        public ITemplate<Panel?>? OriginalItemsPanel { get; set; }

        public DispatcherTimer? FilterTimer { get; set; }
    }

    private static readonly ITemplate<Panel?> NonVirtualizingItemsPanel = new FuncTemplate<Panel?>(() => new StackPanel());

    private static readonly ConditionalWeakTable<SelectingItemsControl, State> States = [];

    static ItemsSearchBehavior()
    {
        IsEnabledProperty.Changed.Subscribe(OnIsEnabledChanged);
        TextProperty.Changed.Subscribe(OnFilterOptionsChanged);
        FilterModeProperty.Changed.Subscribe(OnFilterOptionsChanged);
        IsCaseSensitiveProperty.Changed.Subscribe(OnFilterOptionsChanged);
        MinimumLengthProperty.Changed.Subscribe(OnFilterOptionsChanged);
        SearchMemberPathProperty.Changed.Subscribe(OnFilterOptionsChanged);
        FilterDelayProperty.Changed.Subscribe(OnFilterOptionsChanged);
    }

    #region IsEnabled

    /// <summary>
    /// Provides IsEnabled Property for attached ItemsSearchBehavior element.
    /// </summary>
    public static readonly AttachedProperty<bool> IsEnabledProperty =
        AvaloniaProperty.RegisterAttached<SelectingItemsControl, bool>("IsEnabled", typeof(ItemsSearchBehavior));

    /// <summary>
    /// Accessor for Attached <see cref="IsEnabledProperty"/>.
    /// </summary>
    public static void SetIsEnabled(SelectingItemsControl element, bool value) => element.SetValue(IsEnabledProperty, value);

    /// <summary>
    /// Accessor for Attached <see cref="IsEnabledProperty"/>.
    /// </summary>
    public static bool GetIsEnabled(SelectingItemsControl element) => element.GetValue(IsEnabledProperty);

    #endregion

    #region Text

    /// <summary>
    /// Provides Text Property for attached ItemsSearchBehavior element.
    /// </summary>
    public static readonly AttachedProperty<string?> TextProperty =
        AvaloniaProperty.RegisterAttached<SelectingItemsControl, string?>("Text", typeof(ItemsSearchBehavior), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Accessor for Attached <see cref="TextProperty"/>.
    /// </summary>
    public static void SetText(SelectingItemsControl element, string? value) => element.SetValue(TextProperty, value);

    /// <summary>
    /// Accessor for Attached <see cref="TextProperty"/>.
    /// </summary>
    public static string? GetText(SelectingItemsControl element) => element.GetValue(TextProperty);

    #endregion

    #region FilterMode

    /// <summary>
    /// Provides FilterMode Property for attached ItemsSearchBehavior element.
    /// </summary>
    public static readonly AttachedProperty<ItemsSearchFilterMode> FilterModeProperty =
        AvaloniaProperty.RegisterAttached<SelectingItemsControl, ItemsSearchFilterMode>("FilterMode", typeof(ItemsSearchBehavior), ItemsSearchFilterMode.Contains);

    /// <summary>
    /// Accessor for Attached <see cref="FilterModeProperty"/>.
    /// </summary>
    public static void SetFilterMode(SelectingItemsControl element, ItemsSearchFilterMode value) => element.SetValue(FilterModeProperty, value);

    /// <summary>
    /// Accessor for Attached <see cref="FilterModeProperty"/>.
    /// </summary>
    public static ItemsSearchFilterMode GetFilterMode(SelectingItemsControl element) => element.GetValue(FilterModeProperty);

    #endregion

    #region IsCaseSensitive

    /// <summary>
    /// Provides IsCaseSensitive Property for attached ItemsSearchBehavior element.
    /// </summary>
    public static readonly AttachedProperty<bool> IsCaseSensitiveProperty =
        AvaloniaProperty.RegisterAttached<SelectingItemsControl, bool>("IsCaseSensitive", typeof(ItemsSearchBehavior));

    /// <summary>
    /// Accessor for Attached <see cref="IsCaseSensitiveProperty"/>.
    /// </summary>
    public static void SetIsCaseSensitive(SelectingItemsControl element, bool value) => element.SetValue(IsCaseSensitiveProperty, value);

    /// <summary>
    /// Accessor for Attached <see cref="IsCaseSensitiveProperty"/>.
    /// </summary>
    public static bool GetIsCaseSensitive(SelectingItemsControl element) => element.GetValue(IsCaseSensitiveProperty);

    #endregion

    #region MinimumLength

    /// <summary>
    /// Provides MinimumLength Property for attached ItemsSearchBehavior element.
    /// </summary>
    public static readonly AttachedProperty<int> MinimumLengthProperty =
        AvaloniaProperty.RegisterAttached<SelectingItemsControl, int>("MinimumLength", typeof(ItemsSearchBehavior));

    /// <summary>
    /// Accessor for Attached <see cref="MinimumLengthProperty"/>.
    /// </summary>
    public static void SetMinimumLength(SelectingItemsControl element, int value) => element.SetValue(MinimumLengthProperty, value);

    /// <summary>
    /// Accessor for Attached <see cref="MinimumLengthProperty"/>.
    /// </summary>
    public static int GetMinimumLength(SelectingItemsControl element) => element.GetValue(MinimumLengthProperty);

    #endregion

    #region ClearOnClose

    /// <summary>
    /// Provides ClearOnClose Property for attached ItemsSearchBehavior element.
    /// </summary>
    public static readonly AttachedProperty<bool> ClearOnCloseProperty =
        AvaloniaProperty.RegisterAttached<SelectingItemsControl, bool>("ClearOnClose", typeof(ItemsSearchBehavior), true);

    /// <summary>
    /// Accessor for Attached <see cref="ClearOnCloseProperty"/>.
    /// </summary>
    public static void SetClearOnClose(SelectingItemsControl element, bool value) => element.SetValue(ClearOnCloseProperty, value);

    /// <summary>
    /// Accessor for Attached <see cref="ClearOnCloseProperty"/>.
    /// </summary>
    public static bool GetClearOnClose(SelectingItemsControl element) => element.GetValue(ClearOnCloseProperty);

    #endregion

    #region PlaceholderText

    /// <summary>
    /// Provides PlaceholderText Property for attached ItemsSearchBehavior element.
    /// </summary>
    public static readonly AttachedProperty<string?> PlaceholderTextProperty =
        AvaloniaProperty.RegisterAttached<SelectingItemsControl, string?>("PlaceholderText", typeof(ItemsSearchBehavior));

    /// <summary>
    /// Accessor for Attached <see cref="PlaceholderTextProperty"/>.
    /// </summary>
    public static void SetPlaceholderText(SelectingItemsControl element, string? value) => element.SetValue(PlaceholderTextProperty, value);

    /// <summary>
    /// Accessor for Attached <see cref="PlaceholderTextProperty"/>.
    /// </summary>
    public static string? GetPlaceholderText(SelectingItemsControl element) => element.GetValue(PlaceholderTextProperty);

    #endregion

    #region TextBoxTheme

    /// <summary>
    /// Provides TextBoxTheme Property for attached ItemsSearchBehavior element.
    /// </summary>
    public static readonly AttachedProperty<ControlTheme?> TextBoxThemeProperty =
        AvaloniaProperty.RegisterAttached<SelectingItemsControl, ControlTheme?>("TextBoxTheme", typeof(ItemsSearchBehavior));

    /// <summary>
    /// Accessor for Attached <see cref="TextBoxThemeProperty"/>.
    /// </summary>
    public static void SetTextBoxTheme(SelectingItemsControl element, ControlTheme? value) => element.SetValue(TextBoxThemeProperty, value);

    /// <summary>
    /// Accessor for Attached <see cref="TextBoxThemeProperty"/>.
    /// </summary>
    public static ControlTheme? GetTextBoxTheme(SelectingItemsControl element) => element.GetValue(TextBoxThemeProperty);

    #endregion

    #region FilterDelay

    /// <summary>
    /// Provides FilterDelay Property for attached ItemsSearchBehavior element.
    /// </summary>
    public static readonly AttachedProperty<int> FilterDelayProperty =
        AvaloniaProperty.RegisterAttached<SelectingItemsControl, int>("FilterDelay", typeof(ItemsSearchBehavior), 150);

    /// <summary>
    /// Accessor for Attached <see cref="FilterDelayProperty"/>.
    /// </summary>
    public static void SetFilterDelay(SelectingItemsControl element, int value) => element.SetValue(FilterDelayProperty, value);

    /// <summary>
    /// Accessor for Attached <see cref="FilterDelayProperty"/>.
    /// </summary>
    public static int GetFilterDelay(SelectingItemsControl element) => element.GetValue(FilterDelayProperty);

    #endregion

    #region SearchMemberPath

    /// <summary>
    /// Provides SearchMemberPath Property for attached ItemsSearchBehavior element.
    /// </summary>
    public static readonly AttachedProperty<string?> SearchMemberPathProperty =
        AvaloniaProperty.RegisterAttached<SelectingItemsControl, string?>("SearchMemberPath", typeof(ItemsSearchBehavior));

    /// <summary>
    /// Accessor for Attached <see cref="SearchMemberPathProperty"/>.
    /// </summary>
    public static void SetSearchMemberPath(SelectingItemsControl element, string? value) =>
        element.SetValue(SearchMemberPathProperty, value);

    /// <summary>
    /// Accessor for Attached <see cref="SearchMemberPathProperty"/>.
    /// </summary>
    public static string? GetSearchMemberPath(SelectingItemsControl element) =>
        element.GetValue(SearchMemberPathProperty);

    #endregion

    internal static bool IsItemVisible(Control itemContainer) => itemContainer.IsVisible;

    internal static int GetMatchCount(SelectingItemsControl control) => ItemsSearchEngine.GetMatchCount(control);

    internal static void FlushApplyFilter(SelectingItemsControl control)
    {
        if (States.TryGetValue(control, out var state))
            StopFilterTimer(state);

        ApplyFilter(control);
    }

    internal static void ApplyFilter(SelectingItemsControl control)
    {
        if (!GetIsEnabled(control))
            return;

        var state = States.GetValue(control, static _ => new State());
        var text = GetText(control);
        var applyFilter = ItemsSearchEngine.ShouldApplyFilter(text, GetMinimumLength(control));
        var filterActive = applyFilter && !string.IsNullOrEmpty(text);
        UpdateItemsPanel(control, state, filterActive);
        var matchCount = 0;

        for (var i = 0; i < control.ItemCount; i++)
        {
            var item = control.Items[i];
            var matches = !applyFilter
                          || string.IsNullOrEmpty(text)
                          || ItemsSearchEngine.IsMatch(
                              text,
                              ItemsSearchEngine.GetItemText(control, item),
                              GetFilterMode(control),
                              GetIsCaseSensitive(control));

            if (matches)
                matchCount++;

            if (control.ContainerFromIndex(i) is { } container)
                container.IsVisible = matches;
        }

        var showEmpty = applyFilter && !string.IsNullOrEmpty(text) && matchCount == 0;

        if (state.SearchPlaceholder is not null)
        {
            state.SearchPlaceholder.PlaceholderActive = showEmpty;
        }
        else
        {
            if (state.SearchEmpty is not null)
                state.SearchEmpty.IsVisible = showEmpty;

            if (state.SearchItemsScrollViewer is not null)
                state.SearchItemsScrollViewer.IsVisible = !showEmpty;
        }

        UpdateSearchBoxAutomation(control, state, matchCount, applyFilter);
    }

    internal static bool IsSearchBoxFocused(SelectingItemsControl control)
    {
        if (!GetIsEnabled(control))
            return false;

        return control.GetVisualDescendants()
            .OfType<TextBox>()
            .Any(x => x.Name == PartSearchBox && x.IsFocused);
    }

    internal static void ClearSearchText(SelectingItemsControl control) =>
        control.SetCurrentValue(TextProperty, null);

    internal static void FocusSearchBoxIfEnabled(SelectingItemsControl control)
    {
        if (!GetIsEnabled(control))
            return;

        var state = States.GetValue(control, static _ => new State());
        if (state.SearchBox is null)
            return;

        Dispatcher.UIThread.Post(
            () => ItemsSearchFocusHelper.FocusSearchBox(state.SearchBox),
            DispatcherPriority.Input);
    }

    internal static bool TryHandleKeyDown(SelectingItemsControl control, KeyEventArgs e)
    {
        if (!GetIsEnabled(control) || !control.IsPopupOpen())
            return false;

        var state = States.GetValue(control, static _ => new State());
        if (state.SearchBox is null || state.ItemsPresenter is null)
            TryResolveTemplatePartsFromVisualTree(control);

        if (state.SearchBox is null || state.ItemsPresenter is null)
            return false;

        if (e.Key == Key.Escape && !string.IsNullOrEmpty(GetText(control)))
        {
            ClearSearchText(control);
            FocusSearchBoxIfEnabled(control);
            e.Handled = true;
            return true;
        }

        if (ItemsSearchFocusHelper.TryHandleSearchBoxKeyDown(
                control,
                state.SearchBox,
                state.ItemsPresenter,
                e,
                () => ClearSearchText(control),
                () => control.ClosePopup()))
            return true;

        return ItemsSearchFocusHelper.TryHandleItemKeyDown(control, state.SearchBox, state.ItemsPresenter, e);
    }

    internal static void SelectAllVisibleItems(MultiComboBox control)
    {
        if (!GetIsEnabled(control))
        {
            control.Selection.SelectAll();
            return;
        }

        ApplyFilter(control);

        for (var i = 0; i < control.ItemCount; i++)
        {
            var container = control.ContainerFromIndex(i);
            if (container is not { IsVisible: true })
                continue;

            control.Selection.Select(i);
        }
    }

    private static void OnIsEnabledChanged(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Sender is not SelectingItemsControl control)
            return;

        if (args.NewValue as bool? == true)
            Attach(control);
        else
            Detach(control);

        ApplyFilter(control);
    }

    private static void OnFilterOptionsChanged(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Sender is not SelectingItemsControl control)
            return;

        if (args.Property == TextProperty)
            ScheduleApplyFilter(control);
        else
            ApplyFilter(control);
    }

    private static void ScheduleApplyFilter(SelectingItemsControl control)
    {
        if (!GetIsEnabled(control))
            return;

        var delay = GetFilterDelay(control);
        if (delay <= 0)
        {
            if (States.TryGetValue(control, out var immediateState))
                StopFilterTimer(immediateState);

            ApplyFilter(control);
            return;
        }

        var state = States.GetValue(control, static _ => new State());
        StopFilterTimer(state);

        state.FilterTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(delay) };
        state.FilterTimer.Tick += (_, _) =>
        {
            StopFilterTimer(state);
            ApplyFilter(control);
        };
        state.FilterTimer.Start();
    }

    private static void StopFilterTimer(State state)
    {
        if (state.FilterTimer is null)
            return;

        state.FilterTimer.Stop();
        state.FilterTimer = null;
    }

    private static void Attach(SelectingItemsControl control)
    {
        var state = States.GetValue(control, static _ => new State());

        if (state.TemplateAppliedHandler is null)
        {
            state.TemplateAppliedHandler = (_, e) => ResolveTemplateParts(control, e);
            control.TemplateApplied += state.TemplateAppliedHandler;
        }

        switch (control)
        {
            case ComboBox comboBox:
                state.DropDownOpenedHandler ??= (_, _) => OnPopupOpened(control);
                state.DropDownClosedHandler ??= (_, _) => OnPopupClosed(control);
                comboBox.DropDownOpened += state.DropDownOpenedHandler;
                comboBox.DropDownClosed += state.DropDownClosedHandler;
                break;
            case MultiComboBox multiComboBox:
                state.DropDownOpenedHandler ??= (_, _) => OnPopupOpened(control);
                state.DropDownClosedHandler ??= (_, _) => OnPopupClosed(control);
                multiComboBox.DropDownOpened += state.DropDownOpenedHandler;
                multiComboBox.DropDownClosed += state.DropDownClosedHandler;
                break;
        }

        if (state.KeyDownHandler is null)
        {
            state.KeyDownHandler = (_, e) =>
            {
                if (TryHandleKeyDown(control, e))
                    e.Handled = true;
            };
            control.AddHandler(InputElement.KeyDownEvent, state.KeyDownHandler, RoutingStrategies.Tunnel);
        }

        TryResolveTemplatePartsFromVisualTree(control);
    }

    private static void Detach(SelectingItemsControl control)
    {
        if (!States.TryGetValue(control, out var state))
            return;

        if (state.TemplateAppliedHandler is not null)
        {
            control.TemplateApplied -= state.TemplateAppliedHandler;
            state.TemplateAppliedHandler = null;
        }

        switch (control)
        {
            case ComboBox comboBox:
                if (state.DropDownOpenedHandler is not null)
                    comboBox.DropDownOpened -= state.DropDownOpenedHandler;
                if (state.DropDownClosedHandler is not null)
                    comboBox.DropDownClosed -= state.DropDownClosedHandler;
                break;
            case MultiComboBox multiComboBox:
                if (state.DropDownOpenedHandler is not null)
                    multiComboBox.DropDownOpened -= state.DropDownOpenedHandler;
                if (state.DropDownClosedHandler is not null)
                    multiComboBox.DropDownClosed -= state.DropDownClosedHandler;
                break;
        }

        if (state.KeyDownHandler is not null)
        {
            control.RemoveHandler(InputElement.KeyDownEvent, state.KeyDownHandler);
            state.KeyDownHandler = null;
        }

        StopFilterTimer(state);
        DetachContainerHandlers(control, state);
        RestoreAllItems(control);
    }

    private static void ResolveTemplateParts(SelectingItemsControl control, TemplateAppliedEventArgs e)
    {
        var state = States.GetValue(control, static _ => new State());
        state.SearchBox = e.NameScope.Find<TextBox>(PartSearchBox);
        state.SearchEmpty = e.NameScope.Find<TextBlock>(PartSearchEmpty);
        state.SearchPlaceholder = e.NameScope.Find<PlaceholderContentControl>(PartSearchPlaceholder);
        state.SearchItemsScrollViewer = e.NameScope.Find<ScrollViewer>(PartSearchItems);
        state.ItemsPresenter = e.NameScope.Find<ItemsPresenter>(PartItemsPresenter);
        state.SearchItemsScrollViewer ??= state.ItemsPresenter?.GetVisualParent() as ScrollViewer;

        AttachContainerHandlers(control, state);
        ApplyFilter(control);
    }

    private static void AttachContainerHandlers(SelectingItemsControl control, State state)
    {
        if (state.ContainerHandlersAttached)
            return;

        control.ContainerPrepared += OnContainerPrepared;
        control.ContainerIndexChanged += OnContainerIndexChanged;
        state.ContainerHandlersAttached = true;
    }

    private static void DetachContainerHandlers(SelectingItemsControl control, State state)
    {
        if (!state.ContainerHandlersAttached)
            return;

        control.ContainerPrepared -= OnContainerPrepared;
        control.ContainerIndexChanged -= OnContainerIndexChanged;
        state.ContainerHandlersAttached = false;
    }

    private static void OnContainerPrepared(object? sender, ContainerPreparedEventArgs e)
    {
        if (sender is not SelectingItemsControl control || e.Container is null)
            return;

        SetContainerVisibility(control, e.Index, e.Container);
    }

    private static void OnContainerIndexChanged(object? sender, ContainerIndexChangedEventArgs e)
    {
        if (sender is not SelectingItemsControl control || e.Container is null)
            return;

        SetContainerVisibility(control, e.NewIndex, e.Container);
    }

    private static void SetContainerVisibility(SelectingItemsControl control, int index, Control container)
    {
        if (!GetIsEnabled(control))
            return;

        var text = GetText(control);
        var applyFilter = ItemsSearchEngine.ShouldApplyFilter(text, GetMinimumLength(control));
        if (!applyFilter || string.IsNullOrEmpty(text))
        {
            container.IsVisible = true;
            return;
        }

        var item = index >= 0 && index < control.ItemCount ? control.Items[index] : null;
        container.IsVisible = item is null
                                || ItemsSearchEngine.IsMatch(
                                    text,
                                    ItemsSearchEngine.GetItemText(control, item),
                                    GetFilterMode(control),
                                    GetIsCaseSensitive(control));
    }

    private static void UpdateItemsPanel(SelectingItemsControl control, State state, bool filterActive)
    {
        if (filterActive)
        {
            state.OriginalItemsPanel ??= control.ItemsPanel;

            if (control.ItemsPanel != NonVirtualizingItemsPanel)
                control.ItemsPanel = NonVirtualizingItemsPanel;
        }
        else
        {
            RestoreItemsPanel(control, state);
        }
    }

    private static void RestoreItemsPanel(SelectingItemsControl control, State state)
    {
        if (state.OriginalItemsPanel is null)
            return;

        control.ItemsPanel = state.OriginalItemsPanel;
        state.OriginalItemsPanel = null;
    }

    private static void OnPopupOpened(SelectingItemsControl control)
    {
        TryResolveTemplatePartsFromVisualTree(control);
        ApplyFilter(control);

        if (GetIsEnabled(control))
            FocusSearchBoxIfEnabled(control);
    }

    private static void OnPopupClosed(SelectingItemsControl control)
    {
        if (States.TryGetValue(control, out var state))
            StopFilterTimer(state);

        if (GetClearOnClose(control))
            ClearSearchText(control);

        RestoreAllItems(control);
    }

    private static void RestoreAllItems(SelectingItemsControl control)
    {
        for (var i = 0; i < control.ItemCount; i++)
        {
            var container = control.ContainerFromIndex(i);
            if (container is null)
                continue;

            container.IsVisible = true;
        }

        if (!States.TryGetValue(control, out var state))
            return;

        if (state.SearchPlaceholder is not null)
            state.SearchPlaceholder.PlaceholderActive = false;
        else
        {
            if (state.SearchEmpty is not null)
                state.SearchEmpty.IsVisible = false;

            if (state.SearchItemsScrollViewer is not null)
                state.SearchItemsScrollViewer.IsVisible = true;
        }

        RestoreItemsPanel(control, state);
    }

    private static void TryResolveTemplatePartsFromVisualTree(SelectingItemsControl control)
    {
        var state = States.GetValue(control, static _ => new State());

        state.SearchBox ??= FindTemplatePart<TextBox>(control, PartSearchBox);
        state.SearchEmpty ??= FindTemplatePart<TextBlock>(control, PartSearchEmpty);
        state.SearchPlaceholder ??= FindTemplatePart<PlaceholderContentControl>(control, PartSearchPlaceholder);
        state.SearchItemsScrollViewer ??= FindTemplatePart<ScrollViewer>(control, PartSearchItems);
        state.ItemsPresenter ??= FindTemplatePart<ItemsPresenter>(control, PartItemsPresenter);
        state.SearchItemsScrollViewer ??= state.ItemsPresenter?.GetVisualParent() as ScrollViewer;

        if (state.SearchBox is null && state.ItemsPresenter is null)
            return;

        AttachContainerHandlers(control, state);
        ApplyFilter(control);
    }

    private static T? FindTemplatePart<T>(SelectingItemsControl control, string name)
        where T : Control
    {
        var fromTree = control.GetVisualDescendants()
            .OfType<T>()
            .FirstOrDefault(x => x.Name == name);
        if (fromTree is not null)
            return fromTree;

        var popup = control.GetVisualDescendants().OfType<Popup>().FirstOrDefault();
        return popup?.Child is Control popupRoot
            ? popupRoot.GetVisualDescendants().OfType<T>().FirstOrDefault(x => x.Name == name)
            : null;
    }

    private static void UpdateSearchBoxAutomation(
        SelectingItemsControl control,
        State state,
        int matchCount,
        bool applyFilter)
    {
        if (state.SearchBox is null)
            return;

        var text = GetText(control);
        if (applyFilter && !string.IsNullOrEmpty(text))
        {
            AutomationProperties.SetName(
                state.SearchBox,
                string.Format(ItemsSearchResources.SearchAutomationNameWithResults, matchCount));
            return;
        }

        AutomationProperties.SetName(
            state.SearchBox,
            GetPlaceholderText(control) ?? ItemsSearchResources.SearchPlaceholder);
    }
}
