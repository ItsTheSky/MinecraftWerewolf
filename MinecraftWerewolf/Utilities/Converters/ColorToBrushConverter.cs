using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace MinecraftWerewolf.Utilities.Converters;

public class ColorToBrushConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is not Color color ? null : new SolidColorBrush(color);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is SolidColorBrush brush ? brush.Color : null;
    }
}