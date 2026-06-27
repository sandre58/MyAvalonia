// -----------------------------------------------------------------------
// <copyright file="FieldsPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;
using Material.Icons;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Controls.Behaviors;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Theme.Assists;
using MyNet.Avalonia.Theme.Classes;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.Avalonia.Theme.Controls.Assists;
using MyNet.Collections;
using MyNet.Fakers.Static;
using MyNet.Generator.Facade;
using MyNet.Humanizer.Facade;
using MyNet.Primitives.Temporal;
using MyNet.UI.Commands;
using MyNet.UI.Resources;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class FieldsPageViewModel(ICommandFactory commands) : ShowcaseViewModel("Fields", commands, [
    AddProperties(new ControlThemeBuilder()
            .AddShapes(CssClass.ShapeCircle)
            .AddVariants(ControlVariant.Solid, ControlVariant.Outlined)
            .AddThemeRoles()
            .AddDefaultSizes(),
        false),

    AddProperties(new ControlThemeBuilder()
            .WithKind("underline")
            .AddThemeRoles()
            .AddDefaultSizes(),
        true)
])
{
    public ICommand IncreaseSpinnerCommand { get; } = commands.CreateRequired<Spinner>(IncreaseSpinner);

    public ICommand DecreaseSpinnerCommand { get; } = commands.CreateRequired<Spinner>(DecreaseSpinner);

    private static ControlThemeBuilder AddProperties(ControlThemeBuilder controlThemeBuilder, bool useDefaultFloatingPlaceholder)
        => controlThemeBuilder.AddAction<Control>(
                x =>
                {
                    switch (x)
                    {
                        case TextBox textBox:
                            textBox.SetValue(TextBox.TextProperty, Faker.Texts.Sentence(3, 8));
                            break;
                        case AutoCompleteBox autoCompleteBox:
                            autoCompleteBox.SetValue(AutoCompleteBox.SelectedItemProperty, RandomGenerator.Current.Item(autoCompleteBox.ItemsSource.OfType<object>().ToList()));
                            break;
                        case ButtonSpinner buttonSpinner:
                            buttonSpinner.SetValue(ContentControl.ContentProperty, RandomGenerator.Current.Int(-1000, 1000));
                            break;
                        case ComboBox comboBox:
                            comboBox.SetValue(global::Avalonia.Controls.Primitives.SelectingItemsControl.SelectedIndexProperty, RandomGenerator.Current.Int(0, comboBox.Items.Count));
                            break;
                        case MultiComboBox multiComboBox:
                            multiComboBox.SetValue(MultiComboBox.SelectedItemsProperty, RandomGenerator.Current.Subset([.. multiComboBox.Items.OfType<object>()], RandomGenerator.Current.Int(2, 5)).ToObservableCollection());
                            break;
                        case TagBox tagBox:
                            tagBox.SetValue(TagBox.TagsProperty, Enumerable.Range(1, RandomGenerator.Current.Int(0, 7)).Select(_ => RandomGenerator.Current.String(RandomGenerator.Current.Int(4, 8))).ToObservableCollection());
                            break;
                        case NumericUpDown numericUpDown:
                            numericUpDown.SetValue(NumericUpDown.ValueProperty, RandomGenerator.Current.Int(-1000, 1000));
                            break;
                        case CalendarDatePicker calendarDatePicker:
                            calendarDatePicker.SetValue(CalendarDatePicker.SelectedDateProperty, RandomGenerator.Current.Date(DateTime.Now.AddYears(-10), DateTime.Now.AddYears(10)));
                            break;
                        case CalendarDatePickerEx calendarDatePickerEx:
                            calendarDatePickerEx.SetValue(CalendarDatePickerEx.SelectedValueProperty, RandomGenerator.Current.Date(DateTime.Now.AddYears(-10), DateTime.Now.AddYears(10)));
                            break;
                        case DatePicker datePicker:
                            datePicker.SetValue(DatePicker.SelectedDateProperty, new DateTimeOffset(RandomGenerator.Current.Date(DateTime.Now.AddYears(-10), DateTime.Now.AddYears(10))));
                            break;
                        case TimePicker timePicker:
                            timePicker.SetValue(TimePicker.SelectedTimeProperty, RandomGenerator.Current.Date(DateTime.Now.AddYears(-10), DateTime.Now.AddYears(10)).TimeOfDay);
                            break;
                        case TimePickerEx timePickerEx:
                            timePickerEx.SetValue(TimePickerEx.SelectedValueProperty, RandomGenerator.Current.Date(DateTime.Now.AddYears(-10), DateTime.Now.AddYears(10)).TimeOfDay);
                            break;
                        case DateTimePickerEx dateTimePickerEx:
                            dateTimePickerEx.SetValue(DateTimePickerEx.SelectedValueProperty, RandomGenerator.Current.Date(DateTime.Now.AddYears(-10), DateTime.Now.AddYears(10)));
                            break;
                        case DateTimeScrollPickerEx dateTimeScrollPickerEx:
                            dateTimeScrollPickerEx.SetValue(DateTimeScrollPickerEx.SelectedDateTimeProperty, RandomGenerator.Current.Date(DateTime.Now.AddYears(-10), DateTime.Now.AddYears(10)));
                            break;
                        case ColorPickerEx colorPickerEx:
                            colorPickerEx.SetValue(ColorPickerEx.SelectedValueProperty, Faker.Colors.Hex().ToColor());
                            break;
                    }
                },
                x => x.DisplayName(nameof(SettingsResources.Random)).WithIcon(MaterialIconKind.Pencil).Of<ButtonEditor>(editor => editor.WithRole(Theme.Theming.Core.ThemeRole.Primary)))
            .AddAction<Control>(x =>
                {
                    switch (x)
                    {
                        case TextBox textBox:
                            textBox.SetValue(TextBox.TextProperty, string.Empty);
                            break;
                        case AutoCompleteBox autoCompleteBox:
                            autoCompleteBox.SetValue(AutoCompleteBox.TextProperty, string.Empty);
                            break;
                        case ButtonSpinner buttonSpinner:
                            buttonSpinner.SetValue(ContentControl.ContentProperty, null);
                            break;
                        case ComboBox comboBox:
                            comboBox.SetValue(global::Avalonia.Controls.Primitives.SelectingItemsControl.SelectedItemProperty, null);
                            break;
                        case MultiComboBox multiComboBox:
                            multiComboBox.SetValue(MultiComboBox.SelectedItemsProperty, null);
                            break;
                        case TagBox tagBox:
                            tagBox.SetValue(TagBox.TagsProperty, Array.Empty<string>());
                            break;
                        case NumericUpDown numericUpDown:
                            numericUpDown.SetValue(NumericUpDown.ValueProperty, null);
                            break;
                        case CalendarDatePicker calendarDatePicker:
                            calendarDatePicker.SetValue(CalendarDatePicker.SelectedDateProperty, null);
                            break;
                        case CalendarDatePickerEx calendarDatePickerEx:
                            calendarDatePickerEx.SetValue(CalendarDatePickerEx.SelectedValueProperty, null);
                            break;
                        case DatePicker datePicker:
                            datePicker.SetValue(DatePicker.SelectedDateProperty, null);
                            break;
                        case TimePicker timePicker:
                            timePicker.SetValue(TimePicker.SelectedTimeProperty, null);
                            break;
                        case TimePickerEx timePickerEx:
                            timePickerEx.SetValue(TimePickerEx.SelectedValueProperty, null);
                            break;
                        case DateTimePickerEx dateTimePickerEx:
                            dateTimePickerEx.SetValue(DateTimePickerEx.SelectedValueProperty, null);
                            break;
                        case DateTimeScrollPickerEx dateTimeScrollPickerEx:
                            dateTimeScrollPickerEx.SetValue(DateTimeScrollPickerEx.SelectedDateTimeProperty, null);
                            break;
                        case ColorPickerEx colorPickerEx:
                            colorPickerEx.SetValue(ColorPickerEx.SelectedValueProperty, null);
                            break;
                    }
                },
                x => x.DisplayName(nameof(UiResources.Clear)).WithIcon(MaterialIconKind.TrashCan))
            .AddValueAction(
                (x, y) => UpdateControl(new()
                    {
                        { typeof(TextBox), TextBox.PlaceholderTextProperty },
                        { typeof(AutoCompleteBox), AutoCompleteBox.PlaceholderTextProperty },
                        { typeof(ButtonSpinner), InputAssist.PlaceholderTextProperty },
                        { typeof(ComboBox), ComboBox.PlaceholderTextProperty },
                        { typeof(TagBox), TagBox.PlaceholderTextProperty },
                        { typeof(NumericUpDown), NumericUpDown.PlaceholderTextProperty },
                        { typeof(MultiComboBox), MultiComboBox.PlaceholderTextProperty },
                        { typeof(CalendarDatePicker), CalendarDatePicker.PlaceholderTextProperty },
                        { typeof(CalendarDatePickerEx), CalendarDatePickerEx.PlaceholderTextProperty },
                        { typeof(DatePicker), InputAssist.PlaceholderTextProperty },
                        { typeof(TimePicker), InputAssist.PlaceholderTextProperty },
                        { typeof(TimePickerEx), TimePickerEx.PlaceholderTextProperty },
                        { typeof(DateTimePickerEx), DateTimePickerEx.PlaceholderTextProperty },
                        { typeof(DateTimeScrollPickerEx), InputAssist.PlaceholderTextProperty },
                        { typeof(ColorPickerEx), ColorPickerEx.PlaceholderTextProperty }
                    },
                    x,
                    ((bool?)y).GetValueOrDefault() ? x.GetType().Name : string.Empty),
                useDefaultFloatingPlaceholder,
                x => x.DisplayName(nameof(SettingsResources.ShowPlaceholderText)).Of<ToggleSwitchEditor>())
            .AddProperty(InputAssist.UseFloatingPlaceholderProperty, useDefaultFloatingPlaceholder, x => x.DisplayName(nameof(SettingsResources.IsFloating)))
            .AddProperty(InputBehavior.IsTextEditableProperty,
                true,
                x => x.DisplayName(nameof(SettingsResources.IsEditable)),
                (x, y) => UpdateControl(new() { { typeof(TextBox), TextBox.IsReadOnlyProperty }, { typeof(ComboBox), ComboBox.IsEditableProperty } },
                    x,
                    ((bool?)y).GetValueOrDefault()))
            .AddProperty(InputAssist.ShowClearButtonProperty, false, x => x.DisplayName(nameof(SettingsResources.ShowClearButton)))
            .AddProperty(InputAssist.ShowClipboardButtonProperty, false, x => x.DisplayName(nameof(SettingsResources.ShowClipboardButton)))
            .AddProperty(InputAssist.InnerLeftContentProperty,
                string.Empty,
                x => x.DisplayName(nameof(SettingsResources.Prefix)).Of<TextBoxEditor>(),
                (x, y) => UpdateControl(new()
                    {
                        { typeof(TextBox), TextBox.InnerLeftContentProperty },
                        { typeof(AutoCompleteBox), AutoCompleteBox.InnerLeftContentProperty },
                        { typeof(NumericUpDown), NumericUpDown.InnerLeftContentProperty }
                    },
                    x,
                    y?.ToString()))
            .AddProperty(InputAssist.InnerRightContentProperty,
                string.Empty,
                x => x.DisplayName(nameof(SettingsResources.Suffix)).Of<TextBoxEditor>(),
                (x, y) => UpdateControl(new()
                    {
                        { typeof(TextBox), TextBox.InnerRightContentProperty },
                        { typeof(AutoCompleteBox), AutoCompleteBox.InnerRightContentProperty },
                        { typeof(NumericUpDown), NumericUpDown.InnerRightContentProperty }
                    },
                    x,
                    y?.ToString()))
            .AddProperty(InputAssist.UnderTextProperty, string.Empty, x => x.DisplayName(nameof(SettingsResources.UnderText)).Of<TextBoxEditor>())
            .AddValueAction(
                (x, y) =>
                {
                    if (Equals(y, "none"))
                    {
                        x.ClearValue(DataValidationErrors.ErrorsProperty);
                    }
                    else if (Application.Current?.TryGetResource($"MyNet.Theme.DataValidationErrors.{y}", null, out var value) == true && value is ControlTheme theme)
                    {
                        ValidationAssist.SetTheme(x, theme);
                        DataValidationErrors.SetError(x, new InvalidOperationException(SettingsResources.ErrorMessage));
                    }
                },
                "none",
                x => x.DisplayName(nameof(SettingsResources.ShowError)).Of<ListBoxEditor>(editor => editor.AddChoice("none", builder => builder.DisplayName(nameof(SettingsResources.ErrorNone)).WithIcon(MaterialIconKind.CircleOffOutline))
                    .AddChoice("Text", builder => builder.DisplayName(nameof(SettingsResources.ErrorText)).WithIcon(MaterialIconKind.FormatText))
                    .AddChoice("Icon", builder => builder.DisplayName(nameof(SettingsResources.ErrorToolTip)).WithIcon(MaterialIconKind.AlertCircle))
                    .AddChoice("Glyph", builder => builder.DisplayName(nameof(SettingsResources.ErrorGlyph)).WithIcon(MaterialIconKind.Triangle))))

            // TextBox
            .AddProperty(InputAssist.ShowRevealButtonProperty,
                false,
                x => x.DisplayName(nameof(SettingsResources.IsPassword)).Group(nameof(TextBox)),
                (x, y) => UpdateControl(new() { { typeof(TextBox), TextBox.PasswordCharProperty } },
                    x,
                    ((bool?)y).GetValueOrDefault() ? '*' : '\0'))

            // Spinner
            .AddValueAction(
                (x, y) => UpdateControl(new() { { typeof(ButtonSpinner), ButtonSpinner.ShowButtonSpinnerProperty }, { typeof(NumericUpDown), NumericUpDown.ShowButtonSpinnerProperty } },
                    x,
                    ((bool?)y).GetValueOrDefault()),
                true,
                x => x.DisplayName(nameof(SettingsResources.ShowButtons)).Group(nameof(Spinner)).Of<ToggleSwitchEditor>())
            .AddValueAction(
                (x, y) => UpdateControl(new() { { typeof(ButtonSpinner), SpinnerAssist.SwitchButtonsProperty }, { typeof(NumericUpDown), SpinnerAssist.SwitchButtonsProperty } },
                    x,
                    ((bool?)y).GetValueOrDefault()),
                false,
                x => x.DisplayName(nameof(SettingsResources.SwitchButtons)).Group(nameof(Spinner)).Of<ToggleSwitchEditor>())
            .AddValueAction(
                (x, y) => UpdateControl(new() { { typeof(ButtonSpinner), ButtonSpinner.ButtonSpinnerLocationProperty }, { typeof(NumericUpDown), NumericUpDown.ButtonSpinnerLocationProperty } },
                    x,
                    y ?? default(Location)),
                Location.Right,
                x => x.DisplayName(nameof(SettingsResources.ButtonsPosition)).Group(nameof(Spinner)).Of<ListBoxEditor>(editor => editor.AddChoices(Enum.GetValues<Location>(), (value, y) => y.DisplayName(() => value.Humanize())
                    .WithIcon(Enum.Parse<MaterialIconKind>($"GamepadCircle{value}")))))
            .AddValueAction(
                (x, y) => UpdateControl(new() { { typeof(ButtonSpinner), SpinnerAssist.LayoutProperty }, { typeof(NumericUpDown), SpinnerAssist.LayoutProperty } },
                    x,
                    y ?? default(SpinnerLayout)),
                SpinnerLayout.Horizontal,
                x => x.DisplayName(nameof(SettingsResources.ButtonsLayout)).Group(nameof(Spinner)).Of<ListBoxEditor>(editor => editor.AddChoices(Enum.GetValues<SpinnerLayout>(), (value, y) => y.DisplayName(() => value.Humanize()))))

            // DatePicker
            .AddValueAction(
                (x, y) => UpdateControl(new() { { typeof(DatePicker), DateTimePickerBehavior.OverridePlaceholderTextProperty } },
                    x,
                    ((bool?)y).GetValueOrDefault()),
                true,
                x => x.DisplayName(nameof(SettingsResources.ShowPlaceholderText)).Group(nameof(DatePicker)).Of<ToggleSwitchEditor>())
            .AddValueAction(
                (x, y) => UpdateControl(new() { { typeof(DatePicker), DatePicker.DayVisibleProperty }, { typeof(DateTimeScrollPickerEx), DateTimeScrollPickerEx.DayVisibleProperty } },
                    x,
                    ((bool?)y).GetValueOrDefault()),
                true,
                x => x.DisplayName(nameof(SettingsResources.DayVisible)).Group(nameof(DatePicker)).Of<ToggleSwitchEditor>())
            .AddValueAction(
                (x, y) => UpdateControl(new() { { typeof(DatePicker), DatePicker.MonthVisibleProperty }, { typeof(DateTimeScrollPickerEx), DateTimeScrollPickerEx.MonthVisibleProperty } },
                    x,
                    ((bool?)y).GetValueOrDefault()),
                true,
                x => x.DisplayName(nameof(SettingsResources.MonthVisible)).Group(nameof(DatePicker)).Of<ToggleSwitchEditor>())
            .AddValueAction(
                (x, y) => UpdateControl(new() { { typeof(DatePicker), DatePicker.YearVisibleProperty }, { typeof(DateTimeScrollPickerEx), DateTimeScrollPickerEx.YearVisibleProperty } },
                    x,
                    ((bool?)y).GetValueOrDefault()),
                true,
                x => x.DisplayName(nameof(SettingsResources.YearVisible)).Group(nameof(DatePicker)).Of<ToggleSwitchEditor>())

            // TimePicker
            .AddValueAction(
                (x, y) => UpdateControl(new() { { typeof(TimePicker), DateTimePickerBehavior.OverridePlaceholderTextProperty } },
                    x,
                    ((bool?)y).GetValueOrDefault()),
                true,
                x => x.DisplayName(nameof(SettingsResources.ShowPlaceholderText)).Group(nameof(TimePicker)).Of<ToggleSwitchEditor>())
            .AddValueAction(
                (x, y) => UpdateControl(new() { { typeof(TimePicker), TimePicker.UseSecondsProperty }, { typeof(TimePickerEx), TimePickerEx.ShowSecondsProperty }, { typeof(DateTimePickerEx), DateTimePickerEx.ShowSecondsProperty }, { typeof(DateTimeScrollPickerEx), DateTimeScrollPickerEx.UseSecondsProperty } },
                    x,
                    ((bool?)y).GetValueOrDefault()),
                false,
                x => x.DisplayName(nameof(SettingsResources.UseSeconds)).Group(nameof(TimePicker)).Of<ToggleSwitchEditor>())
            .AddValueAction(
                (x, y) =>
                {
                    UpdateControl(new() { { typeof(TimePickerEx), TimePickerEx.TimeFormatProperty }, { typeof(DateTimePickerEx), DateTimePickerEx.TimeFormatProperty } },
                        x,
                        y ?? default(TimeFormat));

                    if (x is TimePicker timePicker)
                        timePicker.SetValue(TimePicker.ClockIdentifierProperty, Equals(y, TimeFormat.TwelveHour) ? "12HourClock" : "24HourClock");

                    if (x is DateTimeScrollPickerEx dateTimeScrollPicker)
                        dateTimeScrollPicker.SetValue(DateTimeScrollPickerEx.ClockIdentifierProperty, Equals(y, TimeFormat.TwelveHour) ? "12HourClock" : "24HourClock");
                },
                TimeFormat.TwentyFourHour,
                x => x.DisplayName(nameof(SettingsResources.Format)).Group(nameof(TimePicker)).Of<ListBoxEditor>(editor => editor.AddChoice(TimeFormat.TwelveHour, builder => builder.DisplayName(() => TimeFormat.TwelveHour.Humanize()).WithIcon(MaterialIconKind.Hours12))
                    .AddChoice(TimeFormat.TwentyFourHour, builder => builder.DisplayName(() => TimeFormat.TwentyFourHour.Humanize()).WithIcon(MaterialIconKind.Hours24))));

    private static void UpdateControl(Dictionary<Type, AvaloniaProperty> mappingProperties, Control control, object? value)
    {
        foreach (var mapping in mappingProperties.Where(mapping => control.GetType().IsAssignableTo(mapping.Key)))
        {
            control.SetValue(mapping.Value, value);
        }
    }

    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.FocusFieldHorizontal;

    public static void IncreaseSpinner(Spinner spinner) => spinner.Content = (int?)spinner.Content + 1;

    public static void DecreaseSpinner(Spinner spinner) => spinner.Content = (int?)spinner.Content - 1;
}
