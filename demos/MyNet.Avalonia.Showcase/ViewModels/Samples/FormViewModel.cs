// -----------------------------------------------------------------------
// <copyright file="FormViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using MyNet.Collections;
using MyNet.Geography;
using MyNet.Globalization.Facade;
using MyNet.Observable;
using MyNet.Observable.Behaviors;
using MyNet.Observable.Behaviors.Metadata.Attributes;
using MyNet.Primitives;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Samples;

/// <summary>
/// Sample registration form demonstrating <see cref="ValidationBehavior{T}"/> with FluentValidation and MyNet form controls.
/// </summary>
internal sealed class FormViewModel : ObservableObject, IValidationAware
{
    [SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "Disposed in Cleanup method")]
    private readonly ValidationBehavior<FormViewModel> _validation;

    public FormViewModel(ICommandFactory commands)
    {
        _validation = this.UseValidation(new FormViewModelValidator());
        _validation.ErrorsChanged += (_, e) => ErrorsChanged?.Invoke(this, e);

        SubmitCommand = commands.Create(Submit);
        ResetCommand = commands.Create(Reset);

        Disposables.Add(_validation);
    }

    #region IValidationAware / INotifyDataErrorInfo

    /// <inheritdoc/>
    public IReadOnlyCollection<string> Errors => _validation.Errors;

    /// <inheritdoc/>
    public bool HasErrors => _validation.HasErrors;

    /// <inheritdoc/>
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    /// <inheritdoc/>
    IEnumerable INotifyDataErrorInfo.GetErrors(string? propertyName) => _validation.GetErrors(propertyName);

    /// <inheritdoc/>
    public bool Validate() => _validation.Validate();

    /// <inheritdoc/>
    public void ValidateProperty(string propertyName) => _validation.ValidateProperty(propertyName);

    /// <inheritdoc/>
    public void ResetValidation() => _validation.ResetValidation();

    #endregion

    #region Commands & status

    public ICommand SubmitCommand { get; }

    public ICommand ResetCommand { get; }

    public string? StatusMessage
    {
        get;
        private set => SetProperty(ref field, value);
    }

    public bool IsSubmitSuccessful
    {
        get;
        private set => SetProperty(ref field, value);
    }

    #endregion

    #region Validated properties

    public string Login { get; set => SetProperty(ref field, value); } = string.Empty;

    [AlsoValidate(nameof(ConfirmPassword))]
    public string Password { get; set => SetProperty(ref field, value); } = string.Empty;

    public string ConfirmPassword { get; set => SetProperty(ref field, value); } = string.Empty;

    public string Email { get; set => SetProperty(ref field, value); } = string.Empty;

    public string FirstName { get; set => SetProperty(ref field, value); } = string.Empty;

    public string LastName { get; set => SetProperty(ref field, value); } = string.Empty;

    public bool AcceptTerms { get; set => SetProperty(ref field, value); }

    #endregion

    #region Other fields (demo data, optional validation)

    public GenderType Gender { get; set; } = GenderType.Male;

    public string? PhoneNumber { get; set; }

    public DateTime? BirthDate { get; set; }

    public Country? Country { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? PostalCode { get; set; }

    public string? State { get; set; }

    public string? SelectedRole { get; set; }

    public ObservableCollection<string> Roles { get; } = new(["User", "Administrator", "Moderator", "Developer", "Designer", "Manager", "Guest"]);

    public string? Company { get; set; }

    public string? JobTitle { get; set; }

    public int? YearsOfExperience { get; set; }

    public decimal? Salary { get; set; }

    public ObservableCollection<string> SelectedSkills { get; set; } = [];

    public ObservableCollection<string> AvailableSkills { get; } = new(["C#", "JavaScript", "Python", "Java", "TypeScript", "SQL", "React", "Angular", "Vue.js", "Node.js", "Docker", "Kubernetes"]);

    public ObservableCollection<CultureInfo> SelectedLanguages { get; set; } = [];

    public ObservableCollection<CultureInfo> AvailableLanguages { get; } = new List<Country>([Country.RussianFederation, Country.France, Country.Germany, Country.Spain, Country.Italy]).Select(x => new CultureInfo(x.Alpha2)).ToObservableCollection();

    public bool ReceiveNewsletter { get; set; }

    public bool EnableNotifications { get; set; }

    public bool EnableTwoFactor { get; set; }

    public bool MakeProfilePublic { get; set; }

    public string? Bio { get; set; }

    public TimeSpan? PreferredStartTime { get; set; }

    public TimeSpan? PreferredEndTime { get; set; }

    public int? AvailabilityPercentage { get; set; } = 50;

    public bool MondayAvailable { get; set; }

    public bool TuesdayAvailable { get; set; }

    public bool WednesdayAvailable { get; set; }

    public bool ThursdayAvailable { get; set; }

    public bool FridayAvailable { get; set; }

    #endregion

    private void Submit()
    {
        StatusMessage = null;
        IsSubmitSuccessful = false;

        if (!Validate())
        {
            StatusMessage = "ValidationFailed".Translate();
            return;
        }

        IsSubmitSuccessful = true;
        StatusMessage = "SubmitSuccess".Translate();
    }

    private void Reset()
    {
        Login = string.Empty;
        Password = string.Empty;
        ConfirmPassword = string.Empty;
        Email = string.Empty;
        FirstName = string.Empty;
        LastName = string.Empty;
        AcceptTerms = false;
        ReceiveNewsletter = false;
        EnableNotifications = false;
        EnableTwoFactor = false;
        MakeProfilePublic = false;
        PhoneNumber = null;
        BirthDate = null;
        Country = null;
        Address = null;
        City = null;
        PostalCode = null;
        State = null;
        SelectedRole = null;
        Company = null;
        JobTitle = null;
        YearsOfExperience = null;
        Salary = null;
        Bio = null;
        SelectedSkills.Clear();
        SelectedLanguages.Clear();
        PreferredStartTime = null;
        PreferredEndTime = null;
        AvailabilityPercentage = 50;
        MondayAvailable = false;
        TuesdayAvailable = false;
        WednesdayAvailable = false;
        ThursdayAvailable = false;
        FridayAvailable = false;
        Gender = GenderType.Male;

        StatusMessage = null;
        IsSubmitSuccessful = false;
        ResetValidation();
    }
}
