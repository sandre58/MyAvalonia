// -----------------------------------------------------------------------
// <copyright file="ControlDefinitionsView.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;
using PropertyChanged;

namespace MyNet.Avalonia.Demo.Views.ControlCatalog;

[DoNotNotify]
internal sealed partial class ControlDefinitionsView : HeaderedContentControl
{
    public ControlDefinitionsView() => InitializeComponent();

    public static readonly StyledProperty<IDataTemplate?> ControlTemplateProperty = AvaloniaProperty.Register<ControlDefinitionsView, IDataTemplate?>(nameof(ControlTemplate));

    public IDataTemplate? ControlTemplate
    {
        get => GetValue(ControlTemplateProperty);
        set => SetValue(ControlTemplateProperty, value);
    }

    public static readonly StyledProperty<ControlThemeDefinition?> ControlThemeDefinitionProperty = AvaloniaProperty.Register<ControlDefinitionsView, ControlThemeDefinition?>(nameof(ControlThemeDefinition));

    public ControlThemeDefinition? ControlThemeDefinition
    {
        get => GetValue(ControlThemeDefinitionProperty);
        set => SetValue(ControlThemeDefinitionProperty, value);
    }
}
