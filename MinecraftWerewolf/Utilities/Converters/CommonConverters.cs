using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace MinecraftWerewolf.Utilities.Converters;

public static class CommonConverters
{
    
    public static readonly IValueConverter IsGreaterThan = new IsGreaterThanConverter(); 
    public static readonly IValueConverter IsLessThan = new IsLessThanConverter();
    
}

public class IsGreaterThanConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int intValue && parameter is int threshold)
            return intValue > threshold;
        return AvaloniaProperty.UnsetValue;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class IsLessThanConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int intValue && parameter is int threshold)
            return intValue < threshold;
        return AvaloniaProperty.UnsetValue;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}