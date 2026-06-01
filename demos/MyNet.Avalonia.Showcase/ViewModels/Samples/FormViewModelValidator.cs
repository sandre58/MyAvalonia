// -----------------------------------------------------------------------
// <copyright file="FormViewModelValidator.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using FluentValidation;
using MyNet.Globalization.Facade;
using MyNet.Observable;

namespace MyNet.Avalonia.Showcase.ViewModels.Samples;

/// <summary>
/// FluentValidation rules for the showcase registration form sample.
/// </summary>
internal sealed class FormViewModelValidator : AbstractValidator<FormViewModel>
{
    public FormViewModelValidator()
    {
        this.RuleForLocalized(x => x.Login).NotEmptyRequired();

        this.RuleForLocalized(x => x.Email)
            .NotEmptyRequired()
            .EmailAddress();

        this.RuleForLocalized(x => x.Password).NotEmptyRequired();

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password)
            .WithMessage("PasswordsMustMatch".Translate());

        this.RuleForLocalized(x => x.FirstName).NotEmptyRequired();
        this.RuleForLocalized(x => x.LastName).NotEmptyRequired();

        RuleFor(x => x.AcceptTerms)
            .Equal(true)
            .WithMessage("TermsMustBeAccepted".Translate());
    }
}
