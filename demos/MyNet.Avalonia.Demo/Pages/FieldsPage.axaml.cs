// -----------------------------------------------------------------------
// <copyright file="FieldsPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Styling;
using MyNet.Avalonia.Controls.Assists;
using MyNet.Avalonia.Demo.Helpers;
using MyNet.Avalonia.Demo.Resources;
using MyNet.Avalonia.Extensions;
using MyNet.Avalonia.Theme;
using MyNet.Humanizer;
using MyNet.Utilities;
using MyNet.Utilities.Generator;
using MyNet.Utilities.Geography;
using MyNet.Utilities.Helpers;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class FieldsPage : Page
{
    private interface IControlMetadata
    {
        Type Type { get; }

        string Name { get; }

        Control Create();

        void Randomize(Control control);

        void Clear(Control control);
    }

    private sealed class ControlData<T>(Func<T> create, Action<T> randomize, Action<T> clear, string? name = null) : IControlMetadata
        where T : Control, new()
    {
        public Type Type => typeof(T);

        public string Name => name ?? typeof(T).Name;

        public Control Create() => create();

        public void Randomize(Control control) => randomize((T)control);

        public void Clear(Control control) => clear((T)control);
    }

    private readonly List<IControlMetadata> _controls;
    private readonly Dictionary<string, IEnumerable<string>> _themes = new()
    {
        { string.Empty, [string.Empty, "Outlined", "Outlined Transparent", "Circle", "Circle Outlined", "Circle Outlined Transparent"] },
        { "Underline",  [string.Empty] }
    };

    public FieldsPage()
    {
        InitializeComponent();

        _controls =
        [
            new ControlData<TextBox>(() =>
            {
                var item = new TextBox
                {
                    Width = 200,
                    [!TextBox.WatermarkProperty] = Watermark[!TextBox.TextProperty],
                    [!TextBox.InnerLeftContentProperty] = InnerLeftContent[!TextBox.TextProperty],
                    [!TextBox.InnerRightContentProperty] = InnerRightContent[!TextBox.TextProperty],
                    [!TextBox.UseFloatingWatermarkProperty] = IsFloating[!ToggleButton.IsCheckedProperty]
                };

                _ = item.Bind(TextBox.IsReadOnlyProperty, new Binding(nameof(ToggleButton.IsChecked))
                {
                    Source = IsEditable,
                    Converter = BoolConverters.Not
                });
                return item;
            },
            x => x.Text = SentenceGenerator.Sentence(1, 3),
            x => x.Text = string.Empty),

            new ControlData<AutoCompleteBox>(() => new AutoCompleteBox
                {
                Width = 200,
                [!AutoCompleteBox.WatermarkProperty] = Watermark[!TextBox.TextProperty],
                [!AutoCompleteBox.InnerLeftContentProperty] = InnerLeftContent[!TextBox.TextProperty],
                [!AutoCompleteBox.InnerRightContentProperty] = InnerRightContent[!TextBox.TextProperty],
                ItemsSource = EnumClass.GetAll<Country>(),
                ItemTemplate = (DataTemplate)this.FindResource("MyNet.DataTemplate.Country")!
                },
            x => x.Text = RandomGenerator.ListItem(x.ItemsSource?.OfType<Country>().ToList() ?? []).Humanize(),
            x => x.Text = string.Empty),

            new ControlData<ComboBox>(() => new ComboBox
                {
                Width = 200,
                [!ComboBox.PlaceholderTextProperty] = Watermark[!TextBox.TextProperty],
                ItemsSource = EnumClass.GetAll<Country>(),
                ItemTemplate = (DataTemplate?)this.FindResource("MyNet.DataTemplate.Country")
                },
            x => x.SelectedIndex = RandomGenerator.Int(0, x.ItemCount - 1),
            x => x.SelectedItem = null),

            new ControlData<Controls.MultiComboBox>(() => new Controls.MultiComboBox
                {
                Width = 200,
                [!Controls.MultiComboBox.WatermarkProperty] = Watermark[!TextBox.TextProperty],
                ItemsSource = EnumClass.GetAll<Country>(),
                ItemTemplate = (DataTemplate?)this.FindResource("MyNet.DataTemplate.Country"),
                SelectedItemTemplate = (DataTemplate?)this.FindResource("MyNet.DataTemplate.Country")
                },
            x => x.SelectedItems = new AvaloniaList<object?>(RandomGenerator.ListItems(x.Items.ToList(), RandomGenerator.Int(2, 5))),
            x => x.SelectedItems = null),

            new ControlData<Controls.TagBox>(() =>
            {
                var item = new Controls.TagBox
                {
                    Width = 200,
                    [!Controls.TagBox.WatermarkProperty] = Watermark[!TextBox.TextProperty],
                };
                return item;
            },
            x => x.Tags = SentenceGenerator.Words(2, 6).Split(" "),
            x =>
            {
                x.Text = string.Empty;
                x.Tags.Clear();
            }),

            new ControlData<NumericUpDown>(() =>
            {
                var item = new NumericUpDown
                {
                    Width = 120,
                    [!NumericUpDown.WatermarkProperty] = Watermark[!TextBox.TextProperty],
                    [!NumericUpDown.InnerLeftContentProperty] = InnerLeftContent[!TextBox.TextProperty],
                    [!NumericUpDown.InnerRightContentProperty] = InnerRightContent[!TextBox.TextProperty],
                    [!NumericUpDown.ShowButtonSpinnerProperty] = ShowButtonsSpinner[!ToggleButton.IsCheckedProperty],
                    [!NumericUpDown.ButtonSpinnerLocationProperty] = NumericUpDownButtonsPosition[!SelectingItemsControl.SelectedValueProperty],
                    [!SpinnerAssist.SwitchButtonsProperty] = SwitchButtonsSpinner[!ToggleButton.IsCheckedProperty],
                    [!SpinnerAssist.LayoutProperty] = NumericUpDownButtonsLayout[!SelectingItemsControl.SelectedValueProperty]
                };

                return item;
            },
            x => x.Value = RandomGenerator.Int(1, 100),
            x => x.Value = null),

            new ControlData<CalendarDatePicker>(() => new CalendarDatePicker
            {
                Width = 150,
                [!CalendarDatePicker.WatermarkProperty] = Watermark[!TextBox.TextProperty]
            },
            x => x.SelectedDate = RandomGenerator.Date(DateTime.Today.AddYears(-1), DateTime.Today.AddYears(1)),
            x => x.Text = string.Empty),

            new ControlData<Controls.CalendarDatePicker>(() => new Controls.CalendarDatePicker
            {
                Width = 150,
                [!Controls.Primitives.DatePickerBase.WatermarkProperty] = Watermark[!TextBox.TextProperty]
            },
            x => x.SelectedDate = RandomGenerator.Date(DateTime.Today.AddYears(-1), DateTime.Today.AddYears(1)),
            x => x.SelectedDate = null,
            "MyCalendarDatePicker"),

            new ControlData<DatePicker>(() => new DatePicker
            {
                Width = 220,
                [!DateTimePickerAssist.OverrideWatermarkProperty] = UseDateWatermark[!ToggleButton.IsCheckedProperty],
                [!DatePicker.DayVisibleProperty] = ShowDay[!ToggleButton.IsCheckedProperty],
                [!DatePicker.MonthVisibleProperty] = ShowMonth[!ToggleButton.IsCheckedProperty],
                [!DatePicker.YearVisibleProperty] = ShowYear[!ToggleButton.IsCheckedProperty]
            },
            x => x.SelectedDate = RandomGenerator.Date(DateTime.Today.AddYears(-1), DateTime.Today.AddYears(1)),
            x => x.SelectedDate = null),

            new ControlData<TimePicker>(() => new TimePicker
            {
                Width = 220,
                [!DateTimePickerAssist.OverrideWatermarkProperty] = UseTimeWatermark[!ToggleButton.IsCheckedProperty],
                [!TimePicker.UseSecondsProperty] = UseSeconds[!ToggleButton.IsCheckedProperty],
                [!TimePicker.ClockIdentifierProperty] = TimeMode[!SelectingItemsControl.SelectedValueProperty]
            },
            x => x.SelectedTime = RandomGenerator.Date(DateTime.Today.AddYears(-1), DateTime.Today.AddYears(1)).TimeOfDay,
            x => x.SelectedTime = null),

            new ControlData<Controls.TimePicker>(() => new Controls.TimePicker
            {
                Width = 110,
                NeedConfirmation = RandomGenerator.Bool(),
                [!Controls.Primitives.TimePickerBase.WatermarkProperty] = Watermark[!TextBox.TextProperty]
            },
            x => x.SelectedTime = RandomGenerator.Date(DateTime.Today.AddYears(-1), DateTime.Today.AddYears(1)).TimeOfDay,
            x => x.SelectedTime = null,
            "MyTimePicker"),

            new ControlData<Controls.DateTimePicker>(() => new Controls.DateTimePicker
            {
                Width = 220,
                NeedConfirmation = RandomGenerator.Bool(),
                [!Controls.Primitives.DatePickerBase.WatermarkProperty] = Watermark[!TextBox.TextProperty]
            },
            x => x.SelectedDate = RandomGenerator.Date(DateTime.Today.AddYears(-1), DateTime.Today.AddYears(1)),
            x => x.SelectedDate = null),

            new ControlData<Controls.DateRangePicker>(() => new Controls.DateRangePicker
            {
                Width = 200,
                [!Controls.Primitives.DatePickerBase.WatermarkProperty] = Watermark[!TextBox.TextProperty]
            },
            x =>
            {
                x.SelectedStartDate = RandomGenerator.Date(DateTime.Today.AddDays(-15), DateTime.Today);
                x.SelectedEndDate = RandomGenerator.Date(DateTime.Today, DateTime.Today.AddDays(15));
            },
            x =>
            {
                x.SelectedStartDate = null;
                x.SelectedEndDate = null;
            }),

            new ControlData<Controls.TimeRangePicker>(() => new Controls.TimeRangePicker
            {
                Width = 140,
                [!Controls.Primitives.TimePickerBase.WatermarkProperty] = Watermark[!TextBox.TextProperty]
            },
            x =>
            {
                x.StartTime = RandomGenerator.Date(DateTime.Today.AddDays(-15), DateTime.Today).TimeOfDay;
                x.EndTime = RandomGenerator.Date(DateTime.Today, DateTime.Today.AddDays(15)).TimeOfDay;
            },
            x =>
            {
                x.StartTime = null;
                x.EndTime = null;
            }),

            new ControlData<Controls.ColorPicker>(() => new Controls.ColorPicker
            {
                Width = 150,
                [!CalendarDatePicker.WatermarkProperty] = Watermark[!TextBox.TextProperty]
            },
            x => x.Color = RandomGenerator.Color().ToColor().GetValueOrDefault(),
            x => x.Color = default)
        ];
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        var panel = this.FindControl<Panel>("Root");

        if (panel is not null)
            Build(panel);
    }

    private void Build(Panel root)
    {
        // Controls
        var grid = new Grid
        {
            [!IsEnabledProperty] = this[!IsActiveProperty]
        };
        grid.RowDefinitions.AddRange(EnumerableHelper.Range(0, (_themes.Keys.Count * _themes.Values.Count * _themes.Values.Count) + 1).Select(_ => new RowDefinition(GridLength.Auto)));
        grid.ColumnDefinitions.AddRange(EnumerableHelper.Range(0, _controls.Count + 1).Select(x => new ColumnDefinition(GridLength.Auto) { SharedSizeGroup = $"column{x}" }));

        root.Children.Add(grid);
        var column = 1;

        foreach (var controlMetadata in _controls)
        {
            var row = 0;

            // Column title
            var columnTitle = new TextBlock
            {
                Text = controlMetadata.Name,
                HorizontalAlignment = global::Avalonia.Layout.HorizontalAlignment.Center
            };
            columnTitle.AddClasses("H5");
            Grid.SetRow(columnTitle, row);
            Grid.SetColumn(columnTitle, column);

            grid.Children.Add(columnTitle);

            row++;

            foreach (var (key, styles) in _themes)
            {
                foreach (var style in styles)
                {
                    if (column == 1)
                    {
                        // Row title
                        var stackPanel = new StackPanel { VerticalAlignment = global::Avalonia.Layout.VerticalAlignment.Center };
                        var rowTitle = new TextBlock
                        {
                            Text = key
                        };
                        stackPanel.Children.Add(rowTitle);
                        var rowSubTitle = new TextBlock
                        {
                            Text = style
                        };
                        rowSubTitle.AddClasses("Small", "Secondary");
                        stackPanel.Children.Add(rowSubTitle);
                        Grid.SetRow(stackPanel, row);
                        Grid.SetColumn(stackPanel, 0);
                        grid.Children.Add(stackPanel);
                    }

                    // CreateFormattedTextBlock control
                    var control = controlMetadata.Create();
                    if (!string.IsNullOrEmpty(key))
                    {
                        var themeKey = BuildHelper.ThemeKeyPattern.FormatWith(controlMetadata.Name, key);
                        if (Application.Current!.TryGetResource(themeKey, null, out var value) && value is ControlTheme t)
                            control.Theme = t;
                    }

                    control.AddClasses(style);

                    control.Margin = new Thickness(20, 12);
                    control[!TextFieldAssist.WatermarkProperty] = Watermark[!TextBox.TextProperty];
                    control[!TextFieldAssist.UseFloatingWatermarkProperty] = IsFloating[!ToggleButton.IsCheckedProperty];
                    control[!TextFieldAssist.InnerLeftContentProperty] = InnerLeftContent[!TextBox.TextProperty];
                    control[!TextFieldAssist.InnerRightContentProperty] = InnerRightContent[!TextBox.TextProperty];
                    control[!TextFieldAssist.UnderTextProperty] = UnderText[!TextBox.TextProperty];
                    control[!TextFieldAssist.ShowClearButtonProperty] = ShowClearButton[!ToggleButton.IsCheckedProperty];
                    control[!TextFieldAssist.ShowClipboardButtonProperty] = ShowClipboardButton[!ToggleButton.IsCheckedProperty];
                    control[!TextFieldAssist.ShowRevealButtonProperty] = IsPassword[!ToggleButton.IsCheckedProperty];
                    control[!TextFieldAssist.IsTextEditableProperty] = IsEditable[!ToggleButton.IsCheckedProperty];

                    Grid.SetRow(control, row);
                    Grid.SetColumn(control, column);

                    grid.Children.Add(control);

                    row++;
                }
            }

            column++;
        }
    }

    private void RandomizeAll_Click(object? sender, global::Avalonia.Interactivity.RoutedEventArgs e)
                => BuildHelper.ExecuteOnChildren<TemplatedControl>(Root, x =>
                {
                    var controlMetadata = _controls.FirstOrDefault(y => y.Type == x.GetType() && !x.Classes.Contains("no-disablable"));
                    controlMetadata?.Randomize(x);
                });

    private void ClearAll_Click(object? sender, global::Avalonia.Interactivity.RoutedEventArgs e)
        => BuildHelper.ExecuteOnChildren<TemplatedControl>(Root, x =>
        {
            var controlMetadata = _controls.FirstOrDefault(y => y.Type == x.GetType());
            controlMetadata?.Clear(x);
        });

    private void UseTheme_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        => BuildHelper.AddClassesOnChildren<TemplatedControl>(Root, [string.Empty, "Accent", "Inverse"], UseTheme?.SelectedIndex ?? 0);

    private void Icon_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        => BuildHelper.AddIconOnChildren<TemplatedControl>(Root, Icon?.SelectedIndex ?? 0);

    private void ShowError_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        => BuildHelper.ExecuteOnChildren<TemplatedControl>(Root, x =>
        {
            switch (ShowError.SelectedIndex)
            {
                case 0:
                    DataValidationErrors.SetErrors(x, []);
                    break;

                case 1:
                    DataValidationErrors.SetErrors(x, [DemoResources.ErrorOnFieldMessage]);

                    if (MyTheme.Current.TryGetResource("MyNet.Theme.DataValidationErrors.Text", null, out var value) && value is ControlTheme t)
                        ValidationAssist.SetTheme(x, t);
                    break;

                case 2:
                    if (MyTheme.Current.TryGetResource("MyNet.Theme.DataValidationErrors.Icon", null, out var value1) && value1 is ControlTheme t1)
                        ValidationAssist.SetTheme(x, t1);
                    DataValidationErrors.SetErrors(x, [DemoResources.ErrorOnFieldMessage]);
                    break;

                case 3:
                    if (MyTheme.Current.TryGetResource("MyNet.Theme.DataValidationErrors.Glyph", null, out var value2) && value2 is ControlTheme t2)
                        ValidationAssist.SetTheme(x, t2);
                    DataValidationErrors.SetErrors(x, [DemoResources.ErrorOnFieldMessage]);
                    break;
            }
        });

    private void IsPassword_IsCheckedChanged(object? sender, global::Avalonia.Interactivity.RoutedEventArgs e)
        => BuildHelper.ExecuteOnChildren<TextBox>(Root, x => x.PasswordChar = IsPassword.IsChecked.IsTrue() ? '*' : '\0');
}
