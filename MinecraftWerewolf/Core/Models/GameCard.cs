using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Microsoft.VisualBasic;
using MinecraftWerewolf.Utilities;

namespace MinecraftWerewolf.Core.Models;

/// <summary>
/// Represent a game card of Minecraft werewolf.
/// </summary>
public abstract class GameCard
{
    
    public abstract int Order { get; }
    public abstract string Id { get; }
    public abstract string DisplayName { get; }
    public abstract string Description { get; }
    
    public abstract Color Color { get; }
    public virtual Color LigherColor => Color.Lighter();
    public virtual Color DarkerColor => Color.Darker();
    
    public virtual string ImagePath => "Cards/" + Id + ".png";
    public virtual bool AllowMultiple => false;

    public abstract List<CardTeam> Teams { get; }

    public virtual bool ShouldBeCalled(WerewolfGame game)
    {
        return true;
    }

    public abstract Control CreateCardControl(WerewolfGame game);

    private Bitmap? _image;
    public Bitmap Image 
    {
        get
        {
            if (_image != null)
                return _image;
        
            _image = GetBitmap(ImagePath);
            return _image;
        }
    }

    #region Utilities

    public static Stream GetResourceStream(string path)
    {
        return AssetLoader.Open(new Uri("avares://MinecraftWerewolf/Assets/" + path));
    }
    
    public static Bitmap GetBitmap(string path)
    {
        return new Bitmap(GetResourceStream(path));
    }

    #endregion

    public virtual bool ShouldActuallyDie(WerewolfGame game)
    {
        return true;
    }
}