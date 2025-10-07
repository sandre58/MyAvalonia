// -----------------------------------------------------------------------
// <copyright file="TagBox.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Styling;
using MyNet.Utilities.Suspending;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[TemplatePart(PartItemsControl, typeof(ItemsControl))]
[PseudoClasses(PseudoClassName.Empty)]
public class TagBox : TemplatedControl
{
    public const string PartItemsControl = "PART_ItemsControl";

    private readonly TextBox _textBox;
    private readonly Suspender _textChanged = new();
    private ItemsControl? _itemsControl;

    private TextPresenter? _presenter;

    public static readonly StyledProperty<IList<string>> TagsProperty =
        AvaloniaProperty.Register<TagBox, IList<string>>(
            nameof(Tags));

    public static readonly StyledProperty<string?> WatermarkProperty = TextBox.WatermarkProperty.AddOwner<TagBox>();

    public static readonly StyledProperty<bool> AcceptsReturnProperty =
        TextBox.AcceptsReturnProperty.AddOwner<TagBox>();

    public bool AcceptsReturn
    {
        get => GetValue(AcceptsReturnProperty);
        set => SetValue(AcceptsReturnProperty, value);
    }

    public static readonly StyledProperty<int> MaxCountProperty = AvaloniaProperty.Register<TagBox, int>(
        nameof(MaxCount), int.MaxValue);

    public static readonly DirectProperty<TagBox, IList> ItemsProperty =
        AvaloniaProperty.RegisterDirect<TagBox, IList>(
            nameof(Items), o => o.Items);

    public static readonly StyledProperty<ControlTheme> InputThemeProperty =
        AvaloniaProperty.Register<TagBox, ControlTheme>(
            nameof(InputTheme));

    public static readonly StyledProperty<IDataTemplate?> ItemTemplateProperty =
        AvaloniaProperty.Register<TagBox, IDataTemplate?>(
            nameof(ItemTemplate));

    public static readonly StyledProperty<string> SeparatorProperty = AvaloniaProperty.Register<TagBox, string>(
        nameof(Separator));

    public static readonly StyledProperty<LostFocusBehavior> LostFocusBehaviorProperty =
        AvaloniaProperty.Register<TagBox, LostFocusBehavior>(
            nameof(LostFocusBehavior));

    public static readonly StyledProperty<bool> AllowDuplicatesProperty = AvaloniaProperty.Register<TagBox, bool>(
        nameof(AllowDuplicates), true);

    /// <summary>
    /// Identifies the <see cref="Text" /> property.
    /// </summary>
    /// <value>The identifier for the <see cref="Text" /> property.</value>
    public static readonly StyledProperty<string?> TextProperty =
        TextBox.TextProperty.AddOwner<TagBox>(new(string.Empty,
            defaultBindingMode: BindingMode.TwoWay,
            enableDataValidation: true));

    public static readonly RoutedEvent<TextChangedEventArgs> TextChangedEvent = RoutedEvent.Register<TagBox, TextChangedEventArgs>(nameof(TextChanged), RoutingStrategies.Bubble);

    public static readonly RoutedEvent<SelectionChangedEventArgs> TagsChangedEvent = RoutedEvent.Register<TagBox, SelectionChangedEventArgs>(nameof(TagsChanged), RoutingStrategies.Bubble);

    public event EventHandler<TextChangedEventArgs> TextChanged
    {
        add => AddHandler(TextChangedEvent, value);
        remove => RemoveHandler(TextChangedEvent, value);
    }

    public event EventHandler<SelectionChangedEventArgs> TagsChanged
    {
        add => AddHandler(TagsChangedEvent, value);
        remove => RemoveHandler(TagsChangedEvent, value);
    }

    static TagBox()
    {
        _ = TextProperty.Changed.AddClassHandler<TagBox>((x, e) => x.OnTextPropertyChanged(e));
        _ = InputThemeProperty.Changed.AddClassHandler<TagBox>((o, e) => o.OnInputThemePropertyChanged(e));
        _ = TagsProperty.Changed.AddClassHandler<TagBox>((o, e) => o.OnTagsPropertyChanged(e));
    }

    public TagBox()
    {
        _textBox = new TextBox
        {
            [!AcceptsReturnProperty] = this.GetObservable(AcceptsReturnProperty).ToBinding()
        };
        _textBox.AddHandler(KeyDownEvent, OnTextBoxKeyDown, RoutingStrategies.Tunnel);
        _textBox.AddHandler(LostFocusEvent, OnTextBox_LostFocus, RoutingStrategies.Bubble);
        Items = new AvaloniaList<object>
        {
            _textBox
        };
        Tags = new ObservableCollection<string>();
    }

    /// <summary>
    /// Gets or sets the text in the text box portion of the
    /// <see cref="TagBox" /> control.
    /// </summary>
    /// <value>The text in the text box portion of the
    /// <see cref="TagBox" /> control.</value>
    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string? Watermark
    {
        get => GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Used for binding")]
    public IList<string> Tags
    {
        get => GetValue(TagsProperty);
        set => SetValue(TagsProperty, value);
    }

    public int MaxCount
    {
        get => GetValue(MaxCountProperty);
        set => SetValue(MaxCountProperty, value);
    }

    public IList Items
    {
        get;
        private set => SetAndRaise(ItemsProperty, ref field, value);
    }

= null!;

    public ControlTheme InputTheme
    {
        get => GetValue(InputThemeProperty);
        set => SetValue(InputThemeProperty, value);
    }

    public IDataTemplate? ItemTemplate
    {
        get => GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    public string Separator
    {
        get => GetValue(SeparatorProperty);
        set => SetValue(SeparatorProperty, value);
    }

    public LostFocusBehavior LostFocusBehavior
    {
        get => GetValue(LostFocusBehaviorProperty);
        set => SetValue(LostFocusBehaviorProperty, value);
    }

    public bool AllowDuplicates
    {
        get => GetValue(AllowDuplicatesProperty);
        set => SetValue(AllowDuplicatesProperty, value);
    }

    private void OnTextBox_LostFocus(object? sender, RoutedEventArgs e)
    {
        switch (LostFocusBehavior)
        {
            case LostFocusBehavior.Add:
                AddTags(_textBox.Text);
                break;
            case LostFocusBehavior.Clear:
                _textBox.Text = string.Empty;
                break;
            case LostFocusBehavior.None:
            default:
                break;
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _itemsControl = e.NameScope.Find<ItemsControl>(PartItemsControl);
    }

    /// <summary>
    /// TextProperty property changed handler.
    /// </summary>
    /// <param name="e">Event arguments.</param>
    private void OnTextPropertyChanged(AvaloniaPropertyChangedEventArgs e)
    {
        using (_textChanged.Suspend())
        {
            _textBox.Text = e.NewValue?.ToString() ?? string.Empty;
        }
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        _presenter = _textBox.GetTemplateChildren().OfType<TextPresenter>().FirstOrDefault();
        _ = _presenter?.GetObservable(TextPresenter.PreeditTextProperty).Subscribe(_ => OnTextChanged());
        _ = _textBox.GetObservable(TextBox.TextProperty).Subscribe(_ => OnTextChanged());
    }

    private void OnInputThemePropertyChanged(AvaloniaPropertyChangedEventArgs args)
    {
        var newTheme = args.GetNewValue<ControlTheme?>();
        if (newTheme?.TargetType == typeof(TextBox)) _textBox.Theme = newTheme;
    }

    private void OnTextChanged()
    {
        if (_textChanged.IsSuspended) return;

        _ = SetValue(TextProperty, _textBox.Text);
        if (string.IsNullOrWhiteSpace(_presenter?.PreeditText) && string.IsNullOrEmpty(_textBox.Text) && Tags.Count == 0)
        {
            PseudoClasses.Set(PseudoClassName.Empty, true);
        }
        else
        {
            PseudoClasses.Set(PseudoClassName.Empty, false);
        }

        RaiseEvent(new TextChangedEventArgs(TextChangedEvent) { RoutedEvent = TextChangedEvent, Source = this });
    }

    private void OnTagsPropertyChanged(AvaloniaPropertyChangedEventArgs args)
    {
        var newTags = args.GetNewValue<IList<string>?>();
        var oldTags = args.GetOldValue<IList<string>?>();

        if (Items is AvaloniaList<object> avaloniaList)
        {
            avaloniaList.RemoveRange(0, avaloniaList.Count - 1);
        }
        else if (Items.Count != 0)
        {
            Items.Clear();
            _ = Items.Add(_textBox);
        }

        if (newTags != null)
        {
            foreach (var newTag in newTags)
                Items.Insert(Items.Count - 1, newTag);
        }

        if (oldTags is INotifyCollectionChanged inccold) inccold.CollectionChanged -= OnCollectionChanged;

        if (Tags is INotifyCollectionChanged incc) incc.CollectionChanged += OnCollectionChanged;

        RaiseEvent(new SelectionChangedEventArgs(TagsChangedEvent, (IList)oldTags!, (IList)newTags!) { RoutedEvent = TagsChangedEvent, Source = this });
    }

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            var items = e.NewItems;
            if (items is null) return;
            var index = e.NewStartingIndex;
            foreach (var item in items)
            {
                if (item is string s)
                {
                    Items.Insert(index, s);
                    index++;
                }
            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Remove)
        {
            var items = e.OldItems;
            if (items is null) return;
            var index = e.OldStartingIndex;
            foreach (var item in items)
            {
                if (item is string)
                    Items.RemoveAt(index);
            }
        }
        else if (e.Action == NotifyCollectionChangedAction.Reset)
        {
            Items.Clear();
            _ = Items.Add(_textBox);
            InvalidateVisual();
        }

        OnTextChanged();
        RaiseEvent(new SelectionChangedEventArgs(TagsChangedEvent, e.OldItems!, e.NewItems!) { RoutedEvent = TagsChangedEvent, Source = this });
    }

    /// <inheritdoc/>
    protected override void OnGotFocus(GotFocusEventArgs e)
    {
        base.OnGotFocus(e);
        if (!IsEnabled)
            return;
        _ = _textBox.Focus();
        _textBox.SelectAll();
    }

    private void OnTextBoxKeyDown(object? sender, KeyEventArgs args)
    {
        if (!AcceptsReturn && args.Key == Key.Enter)
        {
            AddTags(_textBox.Text);
        }
        else if (AcceptsReturn && args.Key == Key.Enter)
        {
            var texts = _textBox.Text?.Split(["\r", "\n"], StringSplitOptions.RemoveEmptyEntries) ?? [];
            foreach (var text in texts)
            {
                AddTags(text);
            }

            args.Handled = true;
        }
        else if (args.Key is Key.Delete or Key.Back)
        {
            if (string.IsNullOrEmpty(_textBox.Text) || _textBox.Text?.Length == 0)
            {
                if (Tags.Count == 0) return;
                var index = Items.Count - 2;

                Tags.RemoveAt(index);
            }
        }
    }

    private void AddTags(string? text)
    {
        if (!(text?.Length > 0)) return;
        if (Tags.Count >= MaxCount) return;
        string[] values = [];
        if (!string.IsNullOrEmpty(Separator))
        {
            values = text.Split([Separator],
                StringSplitOptions.RemoveEmptyEntries);
        }
        else if (_textBox.Text is not null)
        {
            values = [_textBox.Text];
        }

        if (!AllowDuplicates)
            values = [.. values.Distinct().Except(Tags)];

        foreach (var value in values)
        {
            var index = Items.Count - 1;
            Tags?.Insert(index, value);
        }

        _textBox.Clear();
    }

    public void Clear()
    {
        _textBox.Clear();
        if (Tags.Count == 0) return;
        Tags.Clear();
    }

    public void Close(object o)
    {
        if (o is Control t)
        {
            if (t.Parent is ContentPresenter presenter)
            {
                var index = _itemsControl?.IndexFromContainer(presenter);
                if (index is >= 0 && index < Items.Count - 1)
                    Tags.RemoveAt(index.Value);
            }
        }
    }
}
