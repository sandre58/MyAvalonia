// -----------------------------------------------------------------------
// <copyright file="AppCommandsService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.UI.Services;

namespace MyNet.Avalonia.UI.Services;

public class AppCommandsService : IAppCommandsService
{
    public void Exit() => System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
}
