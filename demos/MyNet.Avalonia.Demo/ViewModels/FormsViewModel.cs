// -----------------------------------------------------------------------
// <copyright file="FormsViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using MyNet.Observable;
using MyNet.Utilities;

namespace MyNet.Avalonia.Demo.ViewModels;

internal class FormsViewModel : EditableObject
{
    // Account Information
    public string? Login { get; set; }

    public string? Password { get; set; }

    public string? ConfirmPassword { get; set; }

    public string? Email { get; set; }

    // Personal Information
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public GenderType Gender { get; set; }

    public string? PhoneNumber { get; set; }

    public DateTime? BirthDate { get; set; }

    public string? SelectedCountry { get; set; }

    public ObservableCollection<string> Countries { get; } = new(["France", "United States", "Germany", "Spain", "Italy", "United Kingdom", "Canada", "Japan", "Australia", "Brazil"]);

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

    public ObservableCollection<string> SelectedLanguages { get; set; } = [];

    public ObservableCollection<string> AvailableLanguages { get; } = new(["English", "French", "Spanish", "German", "Italian", "Portuguese", "Chinese", "Japanese", "Arabic"]);

    // Preferences & Settings
    public bool AcceptTerms { get; set; }

    public bool ReceiveNewsletter { get; set; }

    public bool EnableNotifications { get; set; }

    public bool EnableTwoFactor { get; set; }

    public bool MakeProfilePublic { get; set; }

    // Additional Details
    public string? Bio { get; set; }

    public string? Website { get; set; }

    public string? LinkedIn { get; set; }

    public string? GitHub { get; set; }

    // Availability
    public TimeSpan? PreferredStartTime { get; set; }

    public TimeSpan? PreferredEndTime { get; set; }

    public int? AvailabilityPercentage { get; set; } = 50;

    public bool MondayAvailable { get; set; }

    public bool TuesdayAvailable { get; set; }

    public bool WednesdayAvailable { get; set; }

    public bool ThursdayAvailable { get; set; }

    public bool FridayAvailable { get; set; }

    // Contract Information
    public DateTime? ContractStartDate { get; set; }

    public DateTime? ContractEndDate { get; set; }

    public string? SelectedContractType { get; set; }

    public ObservableCollection<string> ContractTypes { get; } = new(["Full-time", "Part-time", "Contract", "Freelance", "Internship"]);

    // Emergency Contact
    public string? EmergencyContactName { get; set; }

    public string? EmergencyContactPhone { get; set; }

    public string? EmergencyContactRelation { get; set; }
}
