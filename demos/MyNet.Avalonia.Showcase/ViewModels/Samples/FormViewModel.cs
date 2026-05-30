// -----------------------------------------------------------------------
// <copyright file="FormViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using MyNet.Observable;
using MyNet.Observable.Attributes;
using MyNet.Utilities;
using MyNet.Utilities.Geography;

namespace MyNet.Avalonia.Showcase.ViewModels.Samples;

internal sealed class FormViewModel : EditableObject
{
    // Account Information
    [IsRequired]
    public string? Login { get; set; }

    [IsRequired]
    public string? Password { get; set; }

    [IsRequired]
    public string? ConfirmPassword { get; set; }

    [IsEmailAddress(true)]
    public string? Email { get; set; }

    // Personal Information
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public GenderType Gender { get; set; } = GenderType.Male;

    [IsPhone(true)]
    public string? PhoneNumber { get; set; }

    public DateTime? BirthDate { get; set; }

    public Country? Country { get; set; }

    // Address Information
    public string? Address { get; set; }

    public string? City { get; set; }

    public string? PostalCode { get; set; }

    public string? State { get; set; }

    // Professional Information
    public string? SelectedRole { get; set; }

    public ObservableCollection<string> Roles { get; } = new(["User", "Administrator", "Moderator", "Developer", "Designer", "Manager", "Guest"]);

    public string? Company { get; set; }

    public string? JobTitle { get; set; }

    public int? YearsOfExperience { get; set; }

    public decimal? Salary { get; set; }

    // Skills & Languages
    public ObservableCollection<string> SelectedSkills { get; set; } = [];

    public ObservableCollection<string> AvailableSkills { get; } = new(["C#", "JavaScript", "Python", "Java", "TypeScript", "SQL", "React", "Angular", "Vue.js", "Node.js", "Docker", "Kubernetes"]);

    public ObservableCollection<CultureInfo> SelectedLanguages { get; set; } = [];

    public ObservableCollection<CultureInfo> AvailableLanguages { get; } = new List<Country>([Country.RussianFederation, Country.France, Country.Germany, Country.Spain, Country.Italy]).Select(x => new CultureInfo(x.Alpha2)).ToObservableCollection();

    // Preferences & Settings
    public bool AcceptTerms { get; set; }

    public bool ReceiveNewsletter { get; set; }

    public bool EnableNotifications { get; set; }

    public bool EnableTwoFactor { get; set; }

    public bool MakeProfilePublic { get; set; }

    // Additional Details
    public string? Bio { get; set; }

    // Availability
    public TimeSpan? PreferredStartTime { get; set; }

    public TimeSpan? PreferredEndTime { get; set; }

    public int? AvailabilityPercentage { get; set; } = 50;

    public bool MondayAvailable { get; set; }

    public bool TuesdayAvailable { get; set; }

    public bool WednesdayAvailable { get; set; }

    public bool ThursdayAvailable { get; set; }

    public bool FridayAvailable { get; set; }
}
