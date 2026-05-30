using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace MyNet.Avalonia.PerfTest.Converters;

/// <summary>
/// Converts boolean to stock status text for optimized rendering
/// </summary>
public class StockStatusConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo? culture)
    {
        if (value is bool inStock)
        {
            return inStock ? "In Stock" : "Out of Stock";
        }
        return "Unknown";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo? culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Converts boolean to stock status color for optimized rendering
/// </summary>
public class StockColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo? culture)
    {
        if (value is bool inStock)
        {
            return inStock ? "Green" : "Red";
        }
        return "Gray";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo? culture)
    {
        throw new NotImplementedException();
    }
}
