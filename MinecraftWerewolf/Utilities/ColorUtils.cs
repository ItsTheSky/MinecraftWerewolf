using Avalonia.Media;

namespace MinecraftWerewolf.Utilities;

public static class ColorUtils
{
    
    public static Color Lighter(this Color color, double factor = 1.5)
    {
        var r = (byte)(color.R * (1 - factor));
        var g = (byte)(color.G * (1 - factor));
        var b = (byte)(color.B * (1 - factor));
        return Color.FromArgb(color.A, r, g, b);
    }
    
    public static Color Darker(this Color color, double factor = 0.5)
    {
        var r = (byte)(color.R * (1 + factor));
        var g = (byte)(color.G * (1 + factor));
        var b = (byte)(color.B * (1 + factor));
        return Color.FromArgb(color.A, r, g, b);
    }
    
}