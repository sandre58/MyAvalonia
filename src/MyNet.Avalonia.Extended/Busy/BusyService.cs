// -----------------------------------------------------------------------
// <copyright file="BusyService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using MyNet.Observable;
using MyNet.UI.Loading;

namespace MyNet.Avalonia.Extended.Busy;

public sealed class BusyService : ObservableObject, IBusyService
{
    public bool IsBusy => false;

    public void Resume() { }

    TBusy IBusyService.GetCurrent<TBusy>()
        where TBusy : class
        => Activator.CreateInstance<TBusy>();

    TBusy IBusyService.Wait<TBusy>() => Activator.CreateInstance<TBusy>();

    Task IBusyService.WaitAsync<TBusy>(Action<TBusy> action) => Task.Run(() => action(new TBusy()));

    Task IBusyService.WaitAsync<TBusy>(Func<TBusy, Task> action) => action(new TBusy());
}
