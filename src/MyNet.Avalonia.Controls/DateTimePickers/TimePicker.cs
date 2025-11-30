// -----------------------------------------------------------------------
// <copyright file="TimePicker.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Avalonia.Extensions;
using MyNet.Utilities;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[TemplatePart(PartButton, typeof(Button))]
[TemplatePart(PartPopup, typeof(Popup))]
[TemplatePart(PartTextBox, typeof(TextBox))]
[TemplatePart(PartPreviewer, typeof(Control))]
[PseudoClasses(PseudoClassName.FlyoutOpen, PseudoClassName.Pressed)]
public class TimePicker : TextPicker<TimeSpan?, TimeView>
{
    static TimePicker()
    {
        CloseOnCommitProperty.OverrideDefaultValue<TimePicker>(false);
        DisplayFormatProperty.OverrideDefaultValue<TimePicker>("hh\\:mm");
    }

    #region Selector

    protected override void AddPreviewerHandlers() => Previewer?.OnLoading<TimeView>(x => x.SelectedValueChanged += OnTimeChanged, x => x.SelectedValueChanged -= OnTimeChanged);

    private void OnTimeChanged(object? sender, SelectionChangedEventArgs e) => OnPreviewValueChanged();

    #endregion

    protected override TimeSpan? IncrementValue(int offset) => SelectedValue?.Add(offset.Minutes());

    protected override TimeSpan? IncrementLargeValue(int offset) => SelectedValue?.Add(offset.Hours());

    protected override string? ConvertValueToString(TimeSpan? value) => !string.IsNullOrWhiteSpace(DisplayFormat)
            ? value?.ToString(DisplayFormat, CultureInfo.CurrentCulture)
            : value?.ToString();

    protected override TimeSpan? ConvertValueFromString(string text) => TimeSpan.Parse(text, CultureInfo.CurrentCulture);

    protected override void SetPreviewValue(TimeSpan? value) => Previewer?.SelectedValue = value;

    protected override TimeSpan? GetPreviewValue() => Previewer?.SelectedValue;
}
