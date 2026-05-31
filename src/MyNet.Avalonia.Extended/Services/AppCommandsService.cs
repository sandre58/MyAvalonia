// -----------------------------------------------------------------------
// <copyright file="AppCommandsService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using MyNet.UI.Services;

namespace MyNet.Avalonia.Extended.Services;

/// <summary>
/// Avalonia implementation of <see cref="IAppCommandsService"/>.
/// </summary>
public sealed class AppCommandsService : IAppCommandsService
{
    /// <inheritdoc />
    public void Exit()
    {
        if (Application.Current?.ApplicationLifetime is IControlledApplicationLifetime controlled)
            controlled.Shutdown();
    }
}
