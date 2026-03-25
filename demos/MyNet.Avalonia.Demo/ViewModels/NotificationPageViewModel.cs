// -----------------------------------------------------------------------
// <copyright file="NotificationPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using MyNet.Avalonia.Demo.Resources;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;
using MyNet.Avalonia.Demo.Views.Samples;
using MyNet.Avalonia.Extended.Toasting;
using MyNet.Avalonia.Templates;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.Avalonia.Theme.Theming.Core;
using MyNet.Observable;
using MyNet.UI.Commands;
using MyNet.UI.Notifications;
using MyNet.UI.Toasting.Settings;
using MyNet.Utilities;
using MyNet.Utilities.Generator;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class NotificationPageViewModel : ControlCatalogViewModel
{
    [SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "Disposed in Cleanup")]
    private ToasterService? _toasterService;

    static NotificationPageViewModel() => RegisteredDataTemplate.Register<CustomNotification>(_ => new LargeContent1(), nameof(INotification));

    public NotificationPageViewModel()
        : base("Notifications",
            [
                new ControlThemeBuilder()
                    .AddRoles(ThemeRole.Success, ThemeRole.Error, ThemeRole.Warning, ThemeRole.Information, ThemeRole.Inverse)
            ])
    {
        ResetToasterService();
        ShowNotificationCommand = CommandsManager.CreateNotNull<ThemeRole>(ShowNotification);
    }

    /// <inheritdoc/>
    public override IconData Icon => IconData.MessageAlert;

    public int Duration { get; set; } = 2;

    public ToasterPosition Placement { get; set; } = ToasterPosition.BottomRight;

    public int MaxItems { get; set; } = 10;

    public int OffsetX { get; set; } = 10;

    public int OffsetY { get; set; } = 10;

    public int ToastWidth { get; set; } = 300;

    public ToastClosingStrategy ClosingStrategy { get; set; } = ToastClosingStrategy.Both;

    public bool FreezeOnMouseEnter { get; set; }

    public bool EnableOnClick { get; set; }

    public bool EnableOnClose { get; set; }

    public ICommand ShowNotificationCommand { get; }

    public void ClearNotifications() => _toasterService?.Clear();

    public void ShowNotification(ThemeRole role) => ShowNotification(CreateNotificationFromRole(role));

    public void ShowNotification(INotification notification)
    {
        var settings = new ToastSettings
        {
            ClosingStrategy = ClosingStrategy,
            FreezeOnMouseEnter = FreezeOnMouseEnter
        };

        var onClick = new Action<INotification>(x => _toasterService?.Show(new MessageNotification(NotificationPageResources.NotificationClickMessage.FormatWith(x), severity: NotificationSeverity.Information), ToastSettings.Default));
        var onClose = new Action(() => _toasterService?.Show(new MessageNotification(NotificationPageResources.NotificationClosedMessage, severity: NotificationSeverity.Success), ToastSettings.Default));
        _toasterService?.Show(notification, settings, onClick: EnableOnClick ? onClick : null, onClose: EnableOnClose ? onClose : null);
    }

    private static INotification CreateNotificationFromRole(ThemeRole role)
    {
        if (role == ThemeRole.Inverse) return new CustomNotification();

        var severity = role switch
        {
            ThemeRole.Success => NotificationSeverity.Success,
            ThemeRole.Warning => NotificationSeverity.Warning,
            ThemeRole.Error => NotificationSeverity.Error,
            _ => NotificationSeverity.Information
        };

        return new MessageNotification(SentenceGenerator.Paragraph(RandomGenerator.Int(4, 7), RandomGenerator.Int(1, 3)), role.ToString(), severity);
    }

    private void ResetToasterService()
    {
        _toasterService?.Dispose();
        _toasterService = new ToasterService(() => (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow, new ToasterSettings
        {
            Duration = TimeSpan.FromSeconds(Duration),
            Position = Placement,
            MaxItems = MaxItems,
            OffsetX = OffsetX,
            OffsetY = OffsetY,
            Width = ToastWidth
        });
    }

    protected override void OnPropertyIsModified(string propertyName, object before, object after)
    {
        base.OnPropertyIsModified(propertyName, before, after);

        ResetToasterService();
    }

    protected override void Cleanup()
    {
        base.Cleanup();
        _toasterService?.Dispose();
    }

    private sealed class CustomNotification : ObservableObject, INotification
    {
        public NotificationSeverity Severity => NotificationSeverity.None;

        public Guid Id { get; } = Guid.NewGuid();

        public bool IsSimilar(object? obj) => true;
    }
}
