// -----------------------------------------------------------------------
// <copyright file="MaterialIcon.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Media;
using Material.Icons;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public sealed class MaterialIcon : ExtendedIcon
{
    public static void InitializeGeometryParser() => MaterialIconDataProvider.InitializeGeometryParser(Geometry.Parse);

    #region Constructor

    static MaterialIcon() => InitializeGeometryParser();

    #endregion

    #region Properties

    public static readonly StyledProperty<MaterialIconKind?> KindProperty = AvaloniaProperty.Register<MaterialIcon, MaterialIconKind?>(nameof(Kind));

    /// <summary>
    /// Gets or sets the icon to display.
    /// </summary>
    public MaterialIconKind? Kind
    {
        get => GetValue(KindProperty);
        set => SetValue(KindProperty, value);
    }

    public static readonly StyledProperty<MaterialIconAnimation> AnimationProperty = AvaloniaProperty.Register<MaterialIcon, MaterialIconAnimation>(nameof(Animation));

    /// <summary>
    /// Gets or sets the icon animation to play.
    /// </summary>
    public MaterialIconAnimation Animation
    {
        get => GetValue(AnimationProperty);
        set => SetValue(AnimationProperty, value);
    }

    #endregion

    #region Overrides

    /// <inheritdoc />
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == KindProperty)
        {
            var newValue = change.GetNewValue<MaterialIconKind?>();
            var data = newValue.HasValue ? MaterialIconDataProvider.Get<Geometry>(newValue.Value) : null;
            SetValue(DataProperty, data);
        }
    }

    #endregion
}
