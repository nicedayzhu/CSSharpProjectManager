using System;
using Avalonia.Data.Converters;
using System.Globalization;
using Avalonia;

namespace CSSharpProjectManager.Converters;

public class EnumToBooleanConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return false;
        
        return value.Equals(parameter);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return AvaloniaProperty.UnsetValue;
        
        return (bool)value ? parameter : AvaloniaProperty.UnsetValue;
    }
}