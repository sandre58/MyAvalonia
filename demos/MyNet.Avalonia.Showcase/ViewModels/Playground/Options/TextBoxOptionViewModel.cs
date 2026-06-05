// -----------------------------------------------------------------------
// -----------------------------------------------------------------------
// <copyright file="TextBoxOptionViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Windows.Input;
using MyNet.Avalonia.Commands;
using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;
using MyNet.Fakers.Static;
using MyNet.Observable;

namespace MyNet.Avalonia.Showcase.ViewModels.Playground.Options;

/// <summary>
/// ViewModel for a text box option, which represents a string value that can be edited by the user. This class inherits from <see cref="ValueOptionViewModel{T}"/>, which provides the necessary functionality to manage the state and display of the text box option in the UI.
/// </summary>
internal sealed class TextBoxOptionViewModel : ValueOptionViewModel<string>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TextBoxOptionViewModel"/> class.
    /// </summary>
    /// <param name="definition">The definition of the control option.</param>
    /// <param name="displayNameFunc">A function that provides the display name for the option.</param>
    /// <param name="initialValue">The initial value of the text box option.</param>
    public TextBoxOptionViewModel(IControlOptionDefinition definition, IObservableValue<string> displayNameFunc, string? initialValue = null)
        : base(definition, initialValue ?? definition.DefaultValue, displayNameFunc)
        => RandomizeTextCommand = ActionCommand.Create(() =>
        {
            switch (RandomizeText)
            {
                case RandomizeText.Disable:
                    break;
                case RandomizeText.Word:
                    Value = Faker.Texts.Word();
                    break;
                case RandomizeText.Words:
                    Value = Faker.Texts.Words(2, 5);
                    break;
                case RandomizeText.Sentence:
                    Value = Faker.Texts.Sentence(5, 12);
                    break;
                case RandomizeText.Paragraph:
                    Value = Faker.Texts.Paragraph(3, 8);
                    break;
            }
        });

    /// <summary>
    /// Gets or sets a value indicating whether the text box should allow multiple lines of text. When set to true, the text box will be configured to accept and display multiple lines of input, allowing users to enter longer text content that spans across several lines. When set to false, the text box will be limited to a single line of input, and any additional lines entered by the user will be ignored or truncated based on the control's behavior. This property can be used to customize the appearance and functionality of the text box based on the specific requirements of the application or user interface design.
    /// </summary>
    public bool IsMultiLine { get; set; }

    /// <summary>
    /// Gets or sets the randomization mode for the text box value. This property allows you to specify how the text box value should be randomized when the associated command is executed. The <see cref="RandomizeText"/> enum provides different options for randomizing the text, such as generating a single word, multiple words, a sentence, or a paragraph. By setting this property, you can control the type of random content that will be generated and assigned to the text box value when the randomization command is triggered.
    /// </summary>
    public RandomizeText RandomizeText { get; set; } = RandomizeText.Disable;

    /// <summary>
    /// Gets the command that randomizes the text box value based on the selected randomization mode. When executed, this command will generate random text according to the current value of the <see cref="RandomizeText"/> property and assign it to the text box value. The command uses the Faker library to generate realistic random content, allowing for dynamic testing and demonstration of the text box functionality in the user interface. Depending on the selected randomization mode, the generated text can range from a single word to a full paragraph, providing flexibility in how the text box value is randomized for different scenarios.
    /// </summary>
    public ICommand RandomizeTextCommand { get; }
}

internal enum RandomizeText
{
    Disable,

    Word,

    Words,

    Sentence,

    Paragraph
}
