// -----------------------------------------------------------------------
// <copyright file="ColorEyeDropper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using MyNet.Avalonia.Controls.Helpers;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class ColorEyeDropper : ContentControl
{
    private DispatcherOperation? _currentTask;
    private Popup? _previewPopup;

    public ColorEyeDropper()
    {
        AddHandler(PointerReleasedEvent, PointerReleasedHandler);
        AddHandler(PointerPressedEvent, PointerPressedHandler);
        AddHandler(PointerMovedEvent, PointerMovedHandler);
    }
    #region PreviewImageOuterPixelCount

    /// <summary>
    /// Provides PreviewImageOuterPixelCount Property.
    /// </summary>
    public static readonly StyledProperty<int> PreviewImageOuterPixelCountProperty = AvaloniaProperty.Register<ColorEyeDropper, int>(nameof(PreviewImageOuterPixelCount), 2);

    /// <summary>
    /// Gets or sets the PreviewImageOuterPixelCount property.
    /// </summary>
    public int PreviewImageOuterPixelCount
    {
        get => GetValue(PreviewImageOuterPixelCountProperty);
        set => SetValue(PreviewImageOuterPixelCountProperty, value);
    }

    #endregion

    #region EyeDropperCursor

    /// <summary>
    /// Provides EyeDropperCursor Property.
    /// </summary>
    public static readonly StyledProperty<Cursor> EyeDropperCursorProperty = AvaloniaProperty.Register<ColorEyeDropper, Cursor>(nameof(EyeDropperCursor));

    /// <summary>
    /// Gets or sets the EyeDropperCursor property.
    /// </summary>
    public Cursor EyeDropperCursor
    {
        get => GetValue(EyeDropperCursorProperty);
        set => SetValue(EyeDropperCursorProperty, value);
    }

    #endregion

    #region PreviewImage

    /// <summary>
    /// PreviewImage DirectProperty definition.
    /// </summary>
    public static readonly DirectProperty<ColorEyeDropper, Bitmap?> PreviewImageProperty = AvaloniaProperty.RegisterDirect<ColorEyeDropper, Bitmap?>(nameof(PreviewImage), o => o.PreviewImage);

    /// <summary>
    /// Gets the PreviewImage.
    /// </summary>
    public Bitmap? PreviewImage
    {
        get;
        private set => SetAndRaise(PreviewImageProperty, ref field, value);
    }

    #endregion

    #region PreviewBrush

    /// <summary>
    /// PreviewBrush DirectProperty definition.
    /// </summary>
    public static readonly DirectProperty<ColorEyeDropper, IBrush?> PreviewBrushProperty = AvaloniaProperty.RegisterDirect<ColorEyeDropper, IBrush?>(nameof(PreviewBrush), o => o.PreviewBrush);

    /// <summary>
    /// Gets the PreviewBrush.
    /// </summary>
    public IBrush? PreviewBrush
    {
        get;
        private set => SetAndRaise(PreviewBrushProperty, ref field, value);
    }

= Brushes.Transparent;

    #endregion

    #region Color

    /// <summary>
    /// Provides Color Property.
    /// </summary>
    public static readonly StyledProperty<Color> ColorProperty = AvaloniaProperty.Register<ColorEyeDropper, Color>(nameof(Color));

    /// <summary>
    /// Gets or sets the Color property.
    /// </summary>
    public Color Color
    {
        get => GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    #endregion

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _previewPopup = e.NameScope.Find<Popup>("PART_Popup");
    }

    private void SetPreview(Point mousePos)
    {
        var translationX = mousePos.X + 16;
        var translationY = mousePos.Y + 16;

        _previewPopup?.SetCurrentValue(Popup.HorizontalOffsetProperty, translationX);
        _previewPopup?.SetCurrentValue(Popup.VerticalOffsetProperty, translationY);

        if (_currentTask?.Status is DispatcherOperationStatus.Executing or DispatcherOperationStatus.Pending)
            _ = _currentTask.Abort();

        var action = new Action(() =>
        {
            var pixelMousePos = this.PointToScreen(mousePos);
            var outerPixelCount = PreviewImageOuterPixelCount;
            var posX = pixelMousePos.X - outerPixelCount;
            var posY = pixelMousePos.Y - outerPixelCount;
            var region = new Rect(posX, posY, (2 * outerPixelCount) + 1, (2 * outerPixelCount) + 1);
            var previewImage = EyeDropperHelper.CaptureRegion(region);
            var previewBrush = new SolidColorBrush(EyeDropperHelper.GetPixelColor(pixelMousePos));

            PreviewImage = previewImage;
            PreviewBrush = previewBrush;
        });

        _currentTask = Dispatcher.UIThread.InvokeAsync(action);
    }

    private void PointerPressedHandler(object? sender, PointerPressedEventArgs e)
    {
        var point = e.GetCurrentPoint(this);
        if (point.Properties.PointerUpdateKind != PointerUpdateKind.LeftButtonPressed) return;
        if (point.Pointer.Type != PointerType.Mouse)
            return;
        _previewPopup?.Open();

        var bitmap = new Bitmap(AssetLoader.Open(new Uri("avares://MyNet.Avalonia.Controls/Cursors/EyeDropper.cur")));
        Cursor = new Cursor(bitmap, new PixelPoint(16, 16));

        SetPreview(e.GetPosition(this));
    }

    private void PointerReleasedHandler(object? sender, PointerReleasedEventArgs e)
    {
        if (e.Handled || e.InitialPressMouseButton is not MouseButton.Left)
            return;
        _previewPopup?.Close();

        Cursor = Cursor.Default;

        SetCurrentValue(ColorProperty, EyeDropperHelper.GetPixelColor(this.PointToScreen(e.GetPosition(this))));
    }

    private void PointerMovedHandler(object? sender, PointerEventArgs e)
    {
        var point = e.GetCurrentPoint(this);
        if (point.Properties.IsLeftButtonPressed)
            SetPreview(e.GetPosition(this));
    }
}
