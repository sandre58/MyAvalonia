// -----------------------------------------------------------------------
// <copyright file="PlaceholderContentControl.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using MyNet.Avalonia.Controls.Internal;

namespace MyNet.Avalonia.Controls;

/// <summary>
/// Displays <see cref="ContentControl.Content"/> or a placeholder when content is empty or when
/// <see cref="PlaceholderActive"/> overrides the default condition.
/// </summary>
[PseudoClasses(":placeholder")]
public class PlaceholderContentControl : ContentControl
{
    public const string PartPresenter = "PART_Presenter";

    private ContentPresenter? _presenter;
    private bool _isPlaceholderVisible;

    static PlaceholderContentControl()
    {
        ContentProperty.Changed.AddClassHandler<PlaceholderContentControl>((c, _) => c.UpdatePresenterState());
        ContentTemplateProperty.Changed.AddClassHandler<PlaceholderContentControl>((c, _) => c.UpdatePresenterState());
        PlaceholderTextProperty.Changed.AddClassHandler<PlaceholderContentControl>((c, _) => c.UpdatePresenterState());
        PlaceholderTemplateProperty.Changed.AddClassHandler<PlaceholderContentControl>((c, _) => c.UpdatePresenterState());
        PlaceholderActiveProperty.Changed.AddClassHandler<PlaceholderContentControl>((c, _) => c.UpdatePresenterState());
        PlaceholderMinHeightProperty.Changed.AddClassHandler<PlaceholderContentControl>((c, _) => c.UpdatePresenterState());
        PlaceholderPaddingProperty.Changed.AddClassHandler<PlaceholderContentControl>((c, _) => c.UpdatePresenterState());
    }

    public PlaceholderContentControl() => UpdatePresenterState();

    #region PlaceholderActive

    /// <summary>
    /// Gets or sets whether the placeholder is shown.
    /// <see langword="null"/> uses the default empty-content condition; <see langword="true"/> forces the placeholder;
    /// <see langword="false"/> forces content.
    /// </summary>
    public static readonly StyledProperty<bool?> PlaceholderActiveProperty =
        AvaloniaProperty.Register<PlaceholderContentControl, bool?>(nameof(PlaceholderActive));

    /// <summary>
    /// Gets or sets whether the placeholder is shown.
    /// </summary>
    public bool? PlaceholderActive
    {
        get => GetValue(PlaceholderActiveProperty);
        set => SetValue(PlaceholderActiveProperty, value);
    }

    #endregion

    #region IsPlaceholderVisible

    /// <summary>
    /// Gets a value indicating whether the placeholder is currently displayed.
    /// </summary>
    public static readonly DirectProperty<PlaceholderContentControl, bool> IsPlaceholderVisibleProperty =
        AvaloniaProperty.RegisterDirect<PlaceholderContentControl, bool>(
            nameof(IsPlaceholderVisible),
            o => o.IsPlaceholderVisible);

    /// <summary>
    /// Gets a value indicating whether the placeholder is currently displayed.
    /// </summary>
    public bool IsPlaceholderVisible => _isPlaceholderVisible;

    #endregion

    #region PlaceholderText

    /// <summary>
    /// Provides PlaceholderText Property.
    /// </summary>
    public static readonly StyledProperty<object?> PlaceholderTextProperty =
        AvaloniaProperty.Register<PlaceholderContentControl, object?>(nameof(PlaceholderText));

    /// <summary>
    /// Gets or sets the PlaceholderText property.
    /// </summary>
    public object? PlaceholderText
    {
        get => GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    #endregion

    #region PlaceholderTemplate

    /// <summary>
    /// Provides PlaceholderTemplate Property.
    /// </summary>
    public static readonly StyledProperty<IDataTemplate?> PlaceholderTemplateProperty =
        AvaloniaProperty.Register<PlaceholderContentControl, IDataTemplate?>(nameof(PlaceholderTemplate));

    /// <summary>
    /// Gets or sets the PlaceholderTemplate property.
    /// </summary>
    public IDataTemplate? PlaceholderTemplate
    {
        get => GetValue(PlaceholderTemplateProperty);
        set => SetValue(PlaceholderTemplateProperty, value);
    }

    #endregion

    #region PlaceholderMinHeight

    /// <summary>
    /// Gets or sets the minimum height applied to the presenter when the placeholder is shown.
    /// </summary>
    public static readonly StyledProperty<double> PlaceholderMinHeightProperty =
        AvaloniaProperty.Register<PlaceholderContentControl, double>(nameof(PlaceholderMinHeight), double.NaN);

    /// <summary>
    /// Gets or sets the minimum height applied to the presenter when the placeholder is shown.
    /// </summary>
    public double PlaceholderMinHeight
    {
        get => GetValue(PlaceholderMinHeightProperty);
        set => SetValue(PlaceholderMinHeightProperty, value);
    }

    #endregion

    #region PlaceholderPadding

    /// <summary>
    /// Gets or sets the padding applied to the presenter when the placeholder is shown.
    /// </summary>
    public static readonly StyledProperty<Thickness> PlaceholderPaddingProperty =
        AvaloniaProperty.Register<PlaceholderContentControl, Thickness>(nameof(PlaceholderPadding));

    /// <summary>
    /// Gets or sets the padding applied to the presenter when the placeholder is shown.
    /// </summary>
    public Thickness PlaceholderPadding
    {
        get => GetValue(PlaceholderPaddingProperty);
        set => SetValue(PlaceholderPaddingProperty, value);
    }

    #endregion

    /// <inheritdoc />
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _presenter = e.NameScope.Find<ContentPresenter>(PartPresenter);
        UpdatePresenterState();
    }

    private void UpdatePresenterState()
    {
        var showPlaceholder = PlaceholderActive ?? EmptyValueHelper.IsEmptyLike(Content);
        PseudoClasses.Set(":placeholder", showPlaceholder);

        if (_isPlaceholderVisible != showPlaceholder)
        {
            _isPlaceholderVisible = showPlaceholder;
            RaisePropertyChanged(IsPlaceholderVisibleProperty, !showPlaceholder, showPlaceholder);
        }

        if (_presenter is null)
            return;

        if (showPlaceholder)
        {
            _presenter.Content = PlaceholderText;
            _presenter.ContentTemplate = PlaceholderTemplate;
            ApplyPlaceholderLayout(_presenter);
            return;
        }

        ClearPlaceholderLayout(_presenter);
        _presenter.Content = Content;
        _presenter.ContentTemplate = ContentTemplate;
    }

    private void ApplyPlaceholderLayout(ContentPresenter presenter)
    {
        if (!double.IsNaN(PlaceholderMinHeight))
            presenter.MinHeight = PlaceholderMinHeight;
        else
            presenter.ClearValue(ContentPresenter.MinHeightProperty);

        if (PlaceholderPadding != default)
            presenter.Padding = PlaceholderPadding;
    }

    private void ClearPlaceholderLayout(ContentPresenter presenter)
    {
        presenter.ClearValue(ContentPresenter.MinHeightProperty);
        presenter.ClearValue(ContentPresenter.PaddingProperty);
    }
}
