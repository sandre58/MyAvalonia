// -----------------------------------------------------------------------
// <copyright file="EnumClassSourceExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Markup.Xaml;
using MyNet.Observable.Translatables;
using MyNet.Utilities;

namespace MyNet.Avalonia.MarkupExtensions;

public class EnumClassSourceExtension : MarkupExtension
{
    private IEnumerable<object>? _enumsToExclude;

    public EnumClassSourceExtension()
    {
    }

    public EnumClassSourceExtension(Type enumType) => EnumType = enumType ?? throw new ArgumentNullException(nameof(enumType));

    public EnumClassSourceExtension(Type enumType, object enumsToExclude)
        : this(enumType) => EnumsToExclude = enumsToExclude is Array enumsAsArray ? enumsAsArray.Cast<object>() : [enumsToExclude];

    public Type? EnumType
    {
        get;

        init
        {
            if (value == null || field == value)
            {
                return;
            }

            field = value;
        }
    }

    public object? EnumsToExclude
    {
        get => _enumsToExclude;

        set
        {
            if (Equals(_enumsToExclude, value) || value == null)
            {
                return;
            }

            var list = value as IEnumerable<object> ?? [value];
            var enumsToExclude = list.ToList();
            var invalidEnumType = enumsToExclude.Select(v => Nullable.GetUnderlyingType(v.GetType()) ?? v.GetType()).FirstOrDefault(e => e != EnumType);
            if (invalidEnumType != null)
            {
                throw new ArgumentException("Wrong type : {0}".InvariantFormatWith(invalidEnumType.Name));
            }

            _enumsToExclude = enumsToExclude;
        }
    }

    public bool OrderByDisplay { get; set; }

    public bool AddNullValue { get; set; }

    public bool Translatable { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (EnumType == null) return new List<EnumTranslatable>();

        var enumValues = EnumClass.GetAll(EnumType).Cast<IEnumeration>().Where(x => _enumsToExclude?.Contains(x) != true).Select(x => new EnumClassTranslatable(x));

        if (OrderByDisplay)
            enumValues = enumValues.OrderBy(x => x.Display);

        var values = enumValues.ToList();

        if (AddNullValue)
            values.Insert(0, null!);

        return !Translatable ? values.ConvertAll(x => x.Value) : values;
    }
}
