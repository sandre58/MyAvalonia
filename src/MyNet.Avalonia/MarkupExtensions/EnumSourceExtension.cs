// -----------------------------------------------------------------------
// <copyright file="EnumSourceExtension.cs" company="Stéphane ANDRE">
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
/// Markup extension that provides a collection of translatable enum values for data binding.
/// </summary>
public class EnumSourceExtension : MarkupExtension
{
    private IEnumerable<object>? _enumsToExclude;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnumSourceExtension"/> class.
    /// </summary>
    public EnumSourceExtension()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EnumSourceExtension"/> class with an enum type.
    /// </summary>
    /// <param name="enumType">The enum type to enumerate.</param>
    /// <exception cref="ArgumentNullException">Thrown when enumType is null.</exception>
    public EnumSourceExtension(Type enumType) => EnumType = enumType ?? throw new ArgumentNullException(nameof(enumType));

    /// <summary>
    /// Initializes a new instance of the <see cref="EnumSourceExtension"/> class with an enum type and excluded values.
    /// </summary>
    /// <param name="enumType">The enum type to enumerate.</param>
    /// <param name="enumsToExclude">The enum values to exclude from the result.</param>
    public EnumSourceExtension(Type enumType, object enumsToExclude)
        : this(enumType) => EnumsToExclude = enumsToExclude is Array enumsAsArray ? enumsAsArray.Cast<object>() : [enumsToExclude];

    /// <summary>
    /// Gets or initializes the enum type to enumerate.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2325:Make 'EnumType' a static property.", Justification = "EnumType must be instance for MarkupExtension usage.")]
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
    /// Gets or sets the enum values to exclude from the result.
    /// Can be a single enum value or an array of enum values.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when the provided values are not of the correct enum type.</exception>
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
            var invalidEnumType = enumsToExclude.Select(v => Nullable.GetUnderlyingType(v.GetType()) ?? v.GetType()).FirstOrDefault(e => !e.IsEnum || e != EnumType);
            if (invalidEnumType != null)
            {
                throw new ArgumentException("Wrong type : {0}".InvariantFormatWith(invalidEnumType.Name));
            }

            _enumsToExclude = enumsToExclude;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to order the enum values by their translated display text.
    /// </summary>
    public bool OrderByDisplay { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to add a null value at the beginning of the list.
    /// </summary>
    public bool AddNullValue { get; set; }

    /// <inheritdoc />
    /// <returns>A list of <see cref="EnumTranslatable"/> objects representing the enum values.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (EnumType == null) return new List<EnumTranslatable>();

        var enumValues = Enum.GetValues(EnumType).Cast<Enum>().Where(x => _enumsToExclude?.Contains(x) != true).Select(x => new EnumTranslatable(x));

        if (OrderByDisplay)
            enumValues = enumValues.OrderBy(x => x.Display);

        var values = enumValues.ToList();

        if (AddNullValue)
            values.Insert(0, null!);

        return values;
    }
}
