// -----------------------------------------------------------------------
// <copyright file="RegisteredDataTemplate.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using MyNet.Utilities;

namespace MyNet.Avalonia.Templates;

public class RegisteredDataTemplate : IDataTemplate
{
    private static readonly Dictionary<string, Dictionary<Type, Func<object, Control>>> CreateControls = [];

    public string Key { get; set; } = string.Empty;

    public static void Register<T>(Func<T, Control> create, string key = "")
    {
        var createControls = CreateControls.GetOrAdd(key, []);
        _ = createControls.AddOrUpdate(typeof(T), x => create((T)x));
    }

    public Control? Build(object? param) => param is null
            ? null
            : CreateControls.TryGetValue(Key, out var value) && value.ContainsKey(param.GetType())
            ? value[param.GetType()].Invoke(param)
            : CreateControls.TryGetValue(string.Empty, out var value1) && value1.ContainsKey(param.GetType())
            ? value1[param.GetType()].Invoke(param)
            : null;

    public bool Match(object? data) => data is not null
        && ((CreateControls.ContainsKey(Key) && CreateControls[Key].ContainsKey(data.GetType()))
            || (CreateControls.ContainsKey(string.Empty) && CreateControls[string.Empty].ContainsKey(data.GetType())));
}
