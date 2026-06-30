// -----------------------------------------------------------------------
// <copyright file="ColorPickerEx.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Avalonia.Controls.Primitives.Internal;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// A color selection control that allows the user to select dates from a drop down color view.
/// </summary>
[TemplatePart(PartButton, typeof(Button))]
[TemplatePart(PartPreviewer, typeof(ColorView))]
[TemplatePart(PartPopup, typeof(Popup))]
[TemplatePart(PartTextBox, typeof(TextBox))]
[PseudoClasses(PseudoClassName.FlyoutOpen, PseudoClassName.Pressed)]
[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "Improve Avalonia control")]
public class ColorPickerEx : TextPicker<Color?, ColorView>
{
    static ColorPickerEx() => CloseOnCommitProperty.OverrideDefaultValue<ColorPickerEx>(false);

    #region TextMode

    /// <summary>
    /// Provides TextMode Property.
    /// </summary>
    public static readonly StyledProperty<ColorDisplayNameMode> TextModeProperty = AvaloniaProperty.Register<ColorPickerEx, ColorDisplayNameMode>(nameof(TextMode));

    /// <summary>
    /// Gets or sets the TextMode property.
    /// </summary>
    public ColorDisplayNameMode TextMode
    {
        get => GetValue(TextModeProperty);
        set => SetValue(TextModeProperty, value);
    }

    #endregion

    #region Hexa

    /// <summary>
    /// Hexa DirectProperty definition.
    /// </summary>
    public static readonly DirectProperty<ColorPickerEx, string?> HexaProperty = AvaloniaProperty.RegisterDirect<ColorPickerEx, string?>(nameof(Hexa), o => o.Hexa);

    /// <summary>
    /// Gets the Hexa.
    /// </summary>
    public string? Hexa
    {
        get;
        private set => SetAndRaise(HexaProperty, ref field, value);
    }

    #endregion

    #region ColorView

    /// <summary>
    /// Defines the <see cref="ColorModel"/> property.
    /// </summary>
    public static readonly StyledProperty<ColorModel> ColorModelProperty = ColorView.ColorModelProperty.AddOwner<ColorView>();

    /// <summary>
    /// Defines the <see cref="ColorSpectrumComponents"/> property.
    /// </summary>
    public static readonly StyledProperty<ColorSpectrumComponents> ColorSpectrumComponentsProperty = ColorView.ColorSpectrumComponentsProperty.AddOwner<ColorView>();

    /// <summary>
    /// Defines the <see cref="ColorSpectrumShape"/> property.
    /// </summary>
    public static readonly StyledProperty<ColorSpectrumShape> ColorSpectrumShapeProperty = ColorView.ColorSpectrumShapeProperty.AddOwner<ColorView>();

    /// <summary>
    /// Defines the <see cref="HexInputAlphaPosition"/> property.
    /// </summary>
    public static readonly StyledProperty<AlphaComponentPosition> HexInputAlphaPositionProperty = ColorView.HexInputAlphaPositionProperty.AddOwner<ColorView>();

    /// <summary>
    /// Defines the <see cref="HsvColor"/> property.
    /// </summary>
    public static readonly StyledProperty<HsvColor> HsvColorProperty = ColorView.HsvColorProperty.AddOwner<ColorView>();

    /// <summary>
    /// Defines the <see cref="IsAccentColorsVisible"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> IsAccentColorsVisibleProperty = ColorView.IsAccentColorsVisibleProperty.AddOwner<ColorView>();

    /// <summary>
    /// Defines the <see cref="IsAlphaEnabled"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> IsAlphaEnabledProperty = ColorView.IsAlphaEnabledProperty.AddOwner<ColorView>();

    /// <summary>
    /// Defines the <see cref="IsAlphaVisible"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> IsAlphaVisibleProperty = ColorView.IsAlphaVisibleProperty.AddOwner<ColorView>();

    /// <summary>
    /// Defines the <see cref="IsColorComponentsVisible"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> IsColorComponentsVisibleProperty = ColorView.IsColorComponentsVisibleProperty.AddOwner<ColorView>();

    /// <summary>
    /// Defines the <see cref="IsColorModelVisible"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> IsColorModelVisibleProperty = ColorView.IsColorModelVisibleProperty.AddOwner<ColorView>();

    /// <summary>
    /// Defines the <see cref="IsColorPaletteVisible"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> IsColorPaletteVisibleProperty = ColorView.IsColorPaletteVisibleProperty.AddOwner<ColorView>();

    /// <summary>
    /// Defines the <see cref="IsColorPreviewVisible"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> IsColorPreviewVisibleProperty = ColorView.IsColorPreviewVisibleProperty.AddOwner<ColorView>();

    /// <summary>
    /// Defines the <see cref="IsColorSpectrumVisible"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> IsColorSpectrumVisibleProperty = ColorView.IsColorSpectrumVisibleProperty.AddOwner<ColorView>();

    /// <summary>
    /// Defines the <see cref="IsColorSpectrumSliderVisible"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> IsColorSpectrumSliderVisibleProperty = ColorView.IsColorSpectrumSliderVisibleProperty.AddOwner<ColorView>();

    /// <summary>
    /// Defines the <see cref="IsComponentSliderVisible"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> IsComponentSliderVisibleProperty = ColorView.IsComponentSliderVisibleProperty.AddOwner<ColorView>();

    /// <summary>
    /// Defines the <see cref="IsComponentTextInputVisible"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> IsComponentTextInputVisibleProperty = ColorView.IsComponentTextInputVisibleProperty.AddOwner<ColorView>();

    /// <summary>
    /// Defines the <see cref="IsHexInputVisible"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> IsHexInputVisibleProperty = ColorView.IsHexInputVisibleProperty.AddOwner<ColorView>();

    /// <summary>
    /// Defines the <see cref="MaxHue"/> property.
    /// </summary>
    public static readonly StyledProperty<int> MaxHueProperty = ColorView.MaxHueProperty.AddOwner<ColorView>();

    /// <summary>
    /// Defines the <see cref="MaxSaturation"/> property.
    /// </summary>
    public static readonly StyledProperty<int> MaxSaturationProperty = ColorView.MaxSaturationProperty.AddOwner<ColorView>();

    /// <summary>
    /// Defines the <see cref="MaxValue"/> property.
    /// </summary>
    public static readonly StyledProperty<int> MaxValueProperty = ColorView.MaxValueProperty.AddOwner<ColorView>();

    /// <summary>
    /// Defines the <see cref="MinHue"/> property.
    /// </summary>
    public static readonly StyledProperty<int> MinHueProperty = ColorView.MinHueProperty.AddOwner<ColorView>();

    /// <summary>
    /// Defines the <see cref="MinSaturation"/> property.
    /// </summary>
    public static readonly StyledProperty<int> MinSaturationProperty = ColorView.MinSaturationProperty.AddOwner<ColorView>();

    /// <summary>
    /// Defines the <see cref="MinValue"/> property.
    /// </summary>
    public static readonly StyledProperty<int> MinValueProperty = ColorView.MinValueProperty.AddOwner<ColorView>();

    /// <summary>
    /// Defines the <see cref="PaletteColors"/> property.
    /// </summary>
    public static readonly StyledProperty<IEnumerable<Color>?> PaletteColorsProperty = ColorView.PaletteColorsProperty.AddOwner<ColorView>();

    /// <summary>
    /// Defines the <see cref="PaletteColumnCount"/> property.
    /// </summary>
    public static readonly StyledProperty<int> PaletteColumnCountProperty = ColorView.PaletteColumnCountProperty.AddOwner<ColorView>();

    /// <summary>
    /// Defines the <see cref="Palette"/> property.
    /// </summary>
    public static readonly StyledProperty<IColorPalette?> PaletteProperty = ColorView.PaletteProperty.AddOwner<ColorView>();

    /// <summary>
    /// Defines the <see cref="SelectedIndex"/> property.
    /// </summary>
    public static readonly StyledProperty<int> SelectedIndexProperty = ColorView.SelectedIndexProperty.AddOwner<ColorView>();

    /// <inheritdoc cref="ColorSlider.ColorModel"/>
    /// <remarks>
    /// This property is only applicable to the Components tab.
    /// The spectrum tab must always be in HSV and the palette tab contains only pre-defined colors.
    /// </remarks>
    public ColorModel ColorModel
    {
        get => GetValue(ColorModelProperty);
        set => SetValue(ColorModelProperty, value);
    }

    /// <inheritdoc cref="ColorSpectrum.Components"/>
    public ColorSpectrumComponents ColorSpectrumComponents
    {
        get => GetValue(ColorSpectrumComponentsProperty);
        set => SetValue(ColorSpectrumComponentsProperty, value);
    }

    /// <inheritdoc cref="ColorSpectrum.Shape"/>
    public ColorSpectrumShape ColorSpectrumShape
    {
        get => GetValue(ColorSpectrumShapeProperty);
        set => SetValue(ColorSpectrumShapeProperty, value);
    }

    /// <summary>
    /// Gets or sets the position of the alpha component in the hexadecimal input box relative to
    /// all other color Components.
    /// </summary>
    public AlphaComponentPosition HexInputAlphaPosition
    {
        get => GetValue(HexInputAlphaPositionProperty);
        set => SetValue(HexInputAlphaPositionProperty, value);
    }

    /// <inheritdoc cref="ColorSpectrum.HsvColor"/>
    public HsvColor HsvColor
    {
        get => GetValue(HsvColorProperty);
        set => SetValue(HsvColorProperty, value);
    }

    /// <inheritdoc cref="ColorPreviewer.IsAccentColorsVisible"/>
    public bool IsAccentColorsVisible
    {
        get => GetValue(IsAccentColorsVisibleProperty);
        set => SetValue(IsAccentColorsVisibleProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the alpha component is enabled.
    /// When disabled (set to false) the alpha component will be fixed to maximum and
    /// editing controls disabled.
    /// </summary>
    public bool IsAlphaEnabled
    {
        get => GetValue(IsAlphaEnabledProperty);
        set => SetValue(IsAlphaEnabledProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the alpha component editing controls
    /// (Slider(s) and TextBox) are visible. When hidden, the existing alpha component
    /// value is maintained.
    /// </summary>
    /// <remarks>
    /// Note that <see cref="IsComponentTextInputVisible"/> also controls the alpha
    /// component TextBox visibility.
    /// </remarks>
    public bool IsAlphaVisible
    {
        get => GetValue(IsAlphaVisibleProperty);
        set => SetValue(IsAlphaVisibleProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the color Components tab/panel/page (subview) is visible.
    /// </summary>
    public bool IsColorComponentsVisible
    {
        get => GetValue(IsColorComponentsVisibleProperty);
        set => SetValue(IsColorComponentsVisibleProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the active color model indicator/selector is visible.
    /// </summary>
    public bool IsColorModelVisible
    {
        get => GetValue(IsColorModelVisibleProperty);
        set => SetValue(IsColorModelVisibleProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the color palette tab/panel/page (subview) is visible.
    /// </summary>
    public bool IsColorPaletteVisible
    {
        get => GetValue(IsColorPaletteVisibleProperty);
        set => SetValue(IsColorPaletteVisibleProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the color preview is visible.
    /// </summary>
    /// <remarks>
    /// Note that accent color visibility is controlled separately by
    /// <see cref="IsAccentColorsVisible"/>.
    /// </remarks>
    public bool IsColorPreviewVisible
    {
        get => GetValue(IsColorPreviewVisibleProperty);
        set => SetValue(IsColorPreviewVisibleProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the color spectrum tab/panel/page (subview) is visible.
    /// </summary>
    public bool IsColorSpectrumVisible
    {
        get => GetValue(IsColorSpectrumVisibleProperty);
        set => SetValue(IsColorSpectrumVisibleProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the color spectrum's third component slider
    /// is visible.
    /// </summary>
    public bool IsColorSpectrumSliderVisible
    {
        get => GetValue(IsColorSpectrumSliderVisibleProperty);
        set => SetValue(IsColorSpectrumSliderVisibleProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether color component sliders are visible.
    /// </summary>
    /// <remarks>
    /// All color Components are controlled by this property but alpha can also be
    /// controlled with <see cref="IsAlphaVisible"/>.
    /// </remarks>
    public bool IsComponentSliderVisible
    {
        get => GetValue(IsComponentSliderVisibleProperty);
        set => SetValue(IsComponentSliderVisibleProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether color component text inputs are visible.
    /// </summary>
    /// <remarks>
    /// All color Components are controlled by this property but alpha can also be
    /// controlled with <see cref="IsAlphaVisible"/>.
    /// </remarks>
    public bool IsComponentTextInputVisible
    {
        get => GetValue(IsComponentTextInputVisibleProperty);
        set => SetValue(IsComponentTextInputVisibleProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the hexadecimal color value text input
    /// is visible.
    /// </summary>
    public bool IsHexInputVisible
    {
        get => GetValue(IsHexInputVisibleProperty);
        set => SetValue(IsHexInputVisibleProperty, value);
    }

    /// <inheritdoc cref="ColorSpectrum.MaxHue"/>
    public int MaxHue
    {
        get => GetValue(MaxHueProperty);
        set => SetValue(MaxHueProperty, value);
    }

    /// <inheritdoc cref="ColorSpectrum.MaxSaturation"/>
    public int MaxSaturation
    {
        get => GetValue(MaxSaturationProperty);
        set => SetValue(MaxSaturationProperty, value);
    }

    /// <inheritdoc cref="ColorSpectrum.MaxValue"/>
    public int MaxValue
    {
        get => GetValue(MaxValueProperty);
        set => SetValue(MaxValueProperty, value);
    }

    /// <inheritdoc cref="ColorSpectrum.MinHue"/>
    public int MinHue
    {
        get => GetValue(MinHueProperty);
        set => SetValue(MinHueProperty, value);
    }

    /// <inheritdoc cref="ColorSpectrum.MinSaturation"/>
    public int MinSaturation
    {
        get => GetValue(MinSaturationProperty);
        set => SetValue(MinSaturationProperty, value);
    }

    /// <inheritdoc cref="ColorSpectrum.MinValue"/>
    public int MinValue
    {
        get => GetValue(MinValueProperty);
        set => SetValue(MinValueProperty, value);
    }

    /// <summary>
    /// Gets or sets the collection of individual colors in the palette.
    /// </summary>
    /// <remarks>
    /// This is not commonly set manually. Instead, it should be set automatically by
    /// providing an <see cref="IColorPalette"/> to the <see cref="Palette"/> property.
    /// <br/><br/>
    /// Also note that this property is what should be bound in the control template.
    /// <see cref="Palette"/> is too high-level to use on its own.
    /// </remarks>
    public IEnumerable<Color>? PaletteColors
    {
        get => GetValue(PaletteColorsProperty);
        set => SetValue(PaletteColorsProperty, value);
    }

    /// <summary>
    /// Gets or sets the number of colors in each row (section) of the color palette.
    /// Within a standard palette, rows are shades and columns are colors.
    /// </summary>
    /// <remarks>
    /// This is not commonly set manually. Instead, it should be set automatically by
    /// providing an <see cref="IColorPalette"/> to the <see cref="Palette"/> property.
    /// <br/><br/>
    /// Also note that this property is what should be bound in the control template.
    /// <see cref="Palette"/> is too high-level to use on its own.
    /// </remarks>
    public int PaletteColumnCount
    {
        get => GetValue(PaletteColumnCountProperty);
        set => SetValue(PaletteColumnCountProperty, value);
    }

    /// <summary>
    /// Gets or sets the color palette.
    /// </summary>
    /// <remarks>
    /// This will automatically set both <see cref="PaletteColors"/> and
    /// <see cref="PaletteColumnCount"/> overwriting any existing values.
    /// </remarks>
    public IColorPalette? Palette
    {
        get => GetValue(PaletteProperty);
        set => SetValue(PaletteProperty, value);
    }

    /// <summary>
    /// Gets or sets the index of the selected tab/panel/page (subview).
    /// </summary>
    /// <remarks>
    /// When using the default control theme, this property is designed to be used with the
    /// <see cref="ColorViewTab"/> enum. The <see cref="ColorViewTab"/> enum defines the
    /// index values of each of the three standard tabs.
    /// Use like `SelectedIndex = (int)ColorViewTab.Palette`.
    /// </remarks>
    public int SelectedIndex
    {
        get => GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }

    #endregion

    #region Selector

    protected override void AddPreviewerHandlers()
    {
        base.AddPreviewerHandlers();
        Previewer?.OnLoading<ColorView>(x => x.ColorChanged += OnColorChanged, x => x.ColorChanged -= OnColorChanged);
    }

    protected override void TryFocusPopupContent()
    {
        if (Previewer is ColorView colorView)
        {
            ColorViewFocusHelper.FocusDefaultContent(colorView);
            return;
        }

        base.TryFocusPopupContent();
    }

    private void OnColorChanged(object? sender, ColorChangedEventArgs e) => OnPreviewValueChanged();

    #endregion

    protected override Color? IncrementValue(int offset)
    {
        if (!SelectedValue.HasValue) return null;

        var hsv = SelectedValue.Value.ToHsv();
        var newHue = (hsv.H + offset) % 360;
        if (newHue < 0) newHue += 360;

        var newHsv = new HsvColor(SelectedValue.Value.A, newHue, hsv.S, hsv.V);
        return newHsv.ToRgb();
    }

    protected override Color? IncrementLargeValue(int offset)
    {
        if (!SelectedValue.HasValue) return null;

        var hsv = SelectedValue.Value.ToHsv();
        var newHue = (hsv.H + (offset * 10)) % 360;
        if (newHue < 0) newHue += 360;

        var newHsv = new HsvColor(SelectedValue.Value.A, newHue, hsv.S, hsv.V);
        return newHsv.ToRgb();
    }

    protected override string? ConvertValueToString(Color? value)
    {
        if (!value.HasValue)
        {
            return null;
        }

        if (TextMode == ColorDisplayNameMode.Hexa)
        {
            return value.Value.ToHex();
        }

        var name = value.Value.ToName();

        if (TextMode == ColorDisplayNameMode.Name)
        {
            return name;
        }

        var hex = value.Value.ToHex();
        return name == hex ? hex : $"{name} ({hex})";
    }

    protected override Color? ConvertValueFromString(string text) => text.TryToColor();

    protected override void SetPreviewValue(Color? value)
    {
        if (Previewer is not null && value.HasValue)
        {
            Previewer.Color = value.Value;
        }
    }

    protected override Color? GetPreviewValue() => Previewer?.Color;
}
