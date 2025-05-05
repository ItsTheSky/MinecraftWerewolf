using CommunityToolkit.Mvvm.ComponentModel;
using MinecraftWerewolf.Core.Models;

namespace MinecraftWerewolf.Core;

public partial class GamePlayer : ObservableObject
{
    public GamePlayer(string name, GameCard? card)
    {
        Name = name;
        Card = card;
    }
    
    [ObservableProperty] private string _name;
    [ObservableProperty] private bool _isProtected;
    [ObservableProperty] private GameCard? _card;
    [ObservableProperty] private bool _shouldDie;

    #region Card-Specific Data

    [ObservableProperty] private GamePlayer? _love;
    [ObservableProperty] private GamePlayer? _sleepingEndermite;

    #endregion

    /// <summary>
    /// Should only be changed when the night is over!
    /// Use <see cref="ShouldDie"/> property instead!
    /// </summary>
    [ObservableProperty] private bool _isAlive = true;

    public void Die()
    {
        IsAlive = false;
        IsProtected = false;
        ShouldDie = false;
        
        Love = null;
        SleepingEndermite = null;
    }
}