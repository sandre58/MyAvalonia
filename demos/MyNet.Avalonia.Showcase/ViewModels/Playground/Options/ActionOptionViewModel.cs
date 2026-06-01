// -----------------------------------------------------------------------
// <copyright file="ActionOptionViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Reactive.Subjects;
using System.Windows.Input;
using Material.Icons;
using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;
using MyNet.Avalonia.Theme.Theming.Core;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Playground.Options;

/// <summary>
/// View model for a control action setting, which represents an interactive action associated with a control, such as a command or event handler. This view model encapsulates the logic for executing the action and provides a command that can be bound to UI elements to trigger the action when invoked. The Subject property allows for reactive handling of the action's execution, enabling subscribers to respond to the action being triggered in a decoupled manner.
/// </summary>
internal abstract class ActionOptionViewModel : OptionViewModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ActionOptionViewModel"/> class with the specified action definition and display name provider. The constructor sets up the command for executing the action and initializes the reactive subject for handling action execution events. The provided action definition contains the logic to be executed when the command is invoked, and the display name provider supplies a user-friendly name for the setting in the UI.
    /// </summary>
    /// <param name="commands">The command factory used to create commands for the action. Cannot be null.</param>
    /// <param name="definition">The definition of the control action, providing metadata and behavior for the setting. Cannot be null.</param>
    /// <param name="displayNameFunc">A provider that supplies the display name for the setting, used to present the setting in the UI.</param>
    /// <param name="icon">An optional icon associated with the setting, which can be used for visual representation in the user interface. The icon can be of any type, such as a string representing a resource path, an image object, or any other relevant representation depending on the UI framework being used. This property allows for enhanced visual cues when displaying the setting in the UI, making it easier for users to identify and understand the purpose of the setting at a glance.</param>
    protected ActionOptionViewModel(ICommandFactory commands, ControlActionDefinition definition, IObservableValue<string> displayNameFunc, MaterialIconKind? icon = null)
        : base(definition, displayNameFunc, icon) => ExecuteCommand = commands.Create(() => ExecuteSubject.OnNext(null));

    /// <summary>
    /// Gets the command that executes an action when invoked.
    /// </summary>
    /// <remarks>This command can be bound to UI elements, allowing users to trigger the associated action. It
    /// is typically used in MVVM patterns to facilitate user interactions.</remarks>
    public ICommand ExecuteCommand { get; }

    /// <summary>
    /// Gets the subject that represents a stream of boolean values.
    /// </summary>
    /// <remarks>This subject can be used to publish boolean values to subscribers, allowing for reactive
    /// programming patterns.</remarks>
    public Subject<object?> ExecuteSubject { get; } = new();
}

/// <summary>
/// View model for a button action setting, which represents an interactive action associated with a control, such as a command or event handler. This view model encapsulates the logic for executing the action and provides a command that can be bound to UI elements to trigger the action when invoked. The Subject property allows for reactive handling of the action's execution, enabling subscribers to respond to the action being triggered in a decoupled manner.
/// </summary>
/// <param name="commands">The command factory used to create commands for the action. Cannot be null.</param>
/// <param name="definition">The definition of the control action, providing metadata and behavior for the setting. Cannot be null.</param>
/// <param name="displayNameFunc">A provider that supplies the display name for the setting, used to present the setting in the UI.</param>
/// <param name="icon">An optional icon associated with the setting, which can be used for visual representation in the user interface. The icon can be of any type, such as a string representing a resource path, an image object, or any other relevant representation depending on the UI framework being used. This property allows for enhanced visual cues when displaying the setting in the UI, making it easier for users to identify and understand the purpose of the setting at a glance.</param>
internal sealed class ButtonOptionViewModel(ICommandFactory commands, ControlActionDefinition definition, IObservableValue<string> displayNameFunc, MaterialIconKind? icon = null) : ActionOptionViewModel(commands, definition, displayNameFunc, icon)
{
    /// <summary>
    /// Gets or sets the role of the button, which indicates the intended purpose or behavior of the button in the user interface. The role can be used to determine how the button should be displayed or interacted with, such as whether it is a primary action, a secondary action, or a destructive action. This information can be utilized by the UI framework to apply appropriate styling or behavior to the button based on its assigned role.
    /// </summary>
    public ThemeRole Role { get; set; }
}
