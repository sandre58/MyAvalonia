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

/// <summary>
/// Markup extension that provides a collection of translatable enumeration class values for data binding.
/// </summary>
public class EnumClassSourceExtension : MarkupExtension
{
    private IEnumerable<object>? _enumsToExclude;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnumClassSourceExtension"/> class.
    /// </summary>
    public EnumClassSourceExtension()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EnumClassSourceExtension"/> class with an enumeration type.
    /// </summary>
    /// <param name="enumType">The enumeration class type to enumerate.</param>
    /// <exception cref="ArgumentNullException">Thrown when enumType is null.</exception>
    public EnumClassSourceExtension(Type enumType) => EnumType = enumType ?? throw new ArgumentNullException(nameof(enumType));

    /// <summary>
    /// Initializes a new instance of the <see cref="EnumClassSourceExtension"/> class with an enumeration type and excluded values.
    /// </summary>
    /// <param name="enumType">The enumeration class type to enumerate.</param>
    /// <param name="enumsToExclude">The enumeration values to exclude from the result.</param>
    public EnumClassSourceExtension(Type enumType, object enumsToExclude)
        : this(enumType) => EnumsToExclude = enumsToExclude is Array enumsAsArray ? enumsAsArray.Cast<object>() : [enumsToExclude];

    /// <summary>
    /// Gets or initializes the enumeration class type to enumerate.
    /// </summary>
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

    /// <summary>
    /// Gets or sets the enumeration values to exclude from the result.
    /// Can be a single enumeration value or an array of enumeration values.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when the provided values are not of the correct enumeration type.</exception>
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

    /// <summary>
    /// Gets or sets a value indicating whether to order the enumeration values by their translated display text.
    /// </summary>
    public bool OrderByDisplay { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to add a null value at the beginning of the list.
    /// </summary>
    public bool AddNullValue { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to return translatable wrappers or raw enumeration values.
    /// When false, returns the raw <see cref="IEnumeration"/> values directly.
    /// When true (default), returns <see cref="EnumClassTranslatable"/> wrappers.
    /// </summary>
    public bool Translatable { get; set; }

    /// <inheritdoc />
    /// <returns>
    /// A list of <see cref="EnumClassTranslatable"/> objects (if Translatable is true).
    /// or raw <see cref="IEnumeration"/> values (if Translatable is false) representing the enumeration values.
    /// </returns>
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
