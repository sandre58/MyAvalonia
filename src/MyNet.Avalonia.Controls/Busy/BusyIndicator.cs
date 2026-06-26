// -----------------------------------------------------------------------
// <copyright file="BusyIndicator.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using MyNet.Avalonia.Controls.Enums;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130

/// <summary>
/// Full-surface busy overlay. Bind <see cref="IsOpen"/> and optional <see cref="BusyContent"/> from a view model,
/// or host it behind an <c>IBusyService</c> adapter from MyNet.Avalonia.Extended.
/// </summary>
/// <remarks>
/// When <see cref="BusyContent"/> is unset, the theme shows a <see cref="Loader"/> with an optional <see cref="Message"/>.
/// Set <see cref="BusyContent"/> to replace the default presentation entirely (for example with service-specific models).
/// Customize the scrim with <see cref="OverlayBackground"/> and <see cref="OverlayOpacity"/>, the card with
/// <c>VariantAssist</c>, <c>ShadowAssist</c>, and theme roles/variants, and the loader with <c>size-*</c> classes.
/// </remarks>
[PseudoClasses(":open", ":blocking", ":custom-content", ":message-empty")]
[TemplatePart("PART_Root", typeof(Panel))]
[TemplatePart("PART_Scrim", typeof(Border))]
[TemplatePart("PART_Surface", typeof(Border))]
[TemplatePart("PART_Loader", typeof(Loader))]
[TemplatePart("PART_BusyContent", typeof(ContentPresenter))]
public class BusyIndicator : TemplatedControl
{
    private const int CloseAnimationDurationMs = 150;

    private int _hideGeneration;

    /// <summary>
    /// Defines the <see cref="IsOpen"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> IsOpenProperty =
        AvaloniaProperty.Register<BusyIndicator, bool>(nameof(IsOpen));

    /// <summary>
    /// Defines the <see cref="IsBlocking"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> IsBlockingProperty =
        AvaloniaProperty.Register<BusyIndicator, bool>(nameof(IsBlocking), true);

    /// <summary>
    /// Defines the <see cref="Message"/> property.
    /// </summary>
    public static readonly StyledProperty<object?> MessageProperty =
        AvaloniaProperty.Register<BusyIndicator, object?>(nameof(Message));

    /// <summary>
    /// Defines the <see cref="Animation"/> property.
    /// </summary>
    public static readonly StyledProperty<LoaderAnimation> AnimationProperty =
        AvaloniaProperty.Register<BusyIndicator, LoaderAnimation>(nameof(Animation));

    /// <summary>
    /// Defines the <see cref="BusyContent"/> property.
    /// </summary>
    public static readonly StyledProperty<object?> BusyContentProperty =
        AvaloniaProperty.Register<BusyIndicator, object?>(nameof(BusyContent));

    /// <summary>
    /// Defines the <see cref="OverlayBackground"/> property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> OverlayBackgroundProperty =
        AvaloniaProperty.Register<BusyIndicator, IBrush?>(nameof(OverlayBackground));

    /// <summary>
    /// Defines the <see cref="OverlayOpacity"/> property.
    /// </summary>
    public static readonly StyledProperty<double> OverlayOpacityProperty =
        AvaloniaProperty.Register<BusyIndicator, double>(nameof(OverlayOpacity), 1d);

    /// <summary>
    /// Defines the <see cref="SurfacePadding"/> property.
    /// </summary>
    public static readonly StyledProperty<Thickness> SurfacePaddingProperty =
        AvaloniaProperty.Register<BusyIndicator, Thickness>(nameof(SurfacePadding));

    /// <summary>
    /// Defines the <see cref="SurfaceCornerRadius"/> property.
    /// </summary>
    public static readonly StyledProperty<CornerRadius> SurfaceCornerRadiusProperty =
        AvaloniaProperty.Register<BusyIndicator, CornerRadius>(nameof(SurfaceCornerRadius));

    static BusyIndicator()
    {
        IsHitTestVisibleProperty.OverrideDefaultValue<BusyIndicator>(false);
        IsVisibleProperty.OverrideDefaultValue<BusyIndicator>(false);

        IsOpenProperty.Changed.AddClassHandler<BusyIndicator>((control, _) => control.UpdateOpenState());
        IsBlockingProperty.Changed.AddClassHandler<BusyIndicator>((control, _) => control.UpdateOpenState());
        BusyContentProperty.Changed.AddClassHandler<BusyIndicator>((control, _) => control.UpdateContentState());
        MessageProperty.Changed.AddClassHandler<BusyIndicator>((control, _) => control.UpdateMessageState());
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BusyIndicator"/> class.
    /// </summary>
    public BusyIndicator()
    {
        UpdateOpenState();
        UpdateContentState();
        UpdateMessageState();
    }

    /// <summary>
    /// Gets or sets a value indicating whether the overlay is visible.
    /// </summary>
    public bool IsOpen
    {
        get => GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the overlay captures pointer input while open.
    /// </summary>
    public bool IsBlocking
    {
        get => GetValue(IsBlockingProperty);
        set => SetValue(IsBlockingProperty, value);
    }

    /// <summary>
    /// Gets or sets the message shown below the default <see cref="Loader"/>.
    /// </summary>
    public object? Message
    {
        get => GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    /// <summary>
    /// Gets or sets the loader animation used by the default presentation.
    /// </summary>
    public LoaderAnimation Animation
    {
        get => GetValue(AnimationProperty);
        set => SetValue(AnimationProperty, value);
    }

    /// <summary>
    /// Gets or sets custom busy content. When set, replaces the default loader and message.
    /// </summary>
    public object? BusyContent
    {
        get => GetValue(BusyContentProperty);
        set => SetValue(BusyContentProperty, value);
    }

    /// <summary>
    /// Gets or sets the scrim brush. Surface styling uses <c>VariantAssist</c>, theme roles, and variants.
    /// </summary>
    public IBrush? OverlayBackground
    {
        get => GetValue(OverlayBackgroundProperty);
        set => SetValue(OverlayBackgroundProperty, value);
    }

    /// <summary>
    /// Gets or sets the opacity multiplier applied to the scrim.
    /// </summary>
    public double OverlayOpacity
    {
        get => GetValue(OverlayOpacityProperty);
        set => SetValue(OverlayOpacityProperty, value);
    }

    /// <summary>
    /// Gets or sets the padding of the centered busy surface card.
    /// </summary>
    public Thickness SurfacePadding
    {
        get => GetValue(SurfacePaddingProperty);
        set => SetValue(SurfacePaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the corner radius of the centered busy surface card.
    /// </summary>
    public CornerRadius SurfaceCornerRadius
    {
        get => GetValue(SurfaceCornerRadiusProperty);
        set => SetValue(SurfaceCornerRadiusProperty, value);
    }

    /// <inheritdoc/>
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        UpdateOpenState();
        UpdateContentState();
        UpdateMessageState();
    }

    private void UpdateOpenState()
    {
        var isOpen = IsOpen;

        if (isOpen)
            _hideGeneration++;

        if (isOpen)
            IsVisible = true;

        IsHitTestVisible = isOpen && IsBlocking;
        PseudoClasses.Set(":open", isOpen);
        PseudoClasses.Set(":blocking", isOpen && IsBlocking);

        if (!isOpen)
            _ = HideAfterCloseAnimationAsync();
    }

    private async Task HideAfterCloseAnimationAsync()
    {
        var generation = _hideGeneration;
        await Task.Delay(CloseAnimationDurationMs).ConfigureAwait(true);

        if (!IsOpen && generation == _hideGeneration)
            IsVisible = false;
    }

    private void UpdateContentState() =>
        PseudoClasses.Set(":custom-content", BusyContent is not null);

    private void UpdateMessageState() =>
        PseudoClasses.Set(":message-empty", IsEmptyLike(Message));

    private static bool IsEmptyLike(object? value) => value is null || value switch
    {
        string text => string.IsNullOrWhiteSpace(text),
        _ => false
    };
}
