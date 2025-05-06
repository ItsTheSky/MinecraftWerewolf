using System;
using System.Collections.Generic;
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
    [ObservableProperty] private bool _isParalyzed;
    
    [ObservableProperty] private DeathSource? _lastDeathSource;

    #endregion

    /// <summary>
    /// Should only be changed when the night is over!
    /// Use <see cref="ShouldDie"/> property instead!
    /// </summary>
    [ObservableProperty] private bool _isAlive = true;

    public void PrepareDeath(DeathSource source)
    {
        ShouldDie = true;
        LastDeathSource = source;
    }
    
    private void ActuallyDie()
    {
        IsAlive = false;
        LastDeathSource = null;
        IsProtected = false;
        ShouldDie = false;
        IsParalyzed = false;
        
        SleepingEndermite = null;
    }
    
    public void SetupDayVote()
    {
        IsProtected = false;
        ShouldDie = false;
        IsParalyzed = false;
        SleepingEndermite = null;
    }

    public List<GamePlayer> Die(DeathSource? forceSource = null)
    {
        var source = forceSource ?? LastDeathSource;
        
        if (source == DeathSource.DayVote) // always die when vote.
        {
            Love?.ActuallyDie();
            ActuallyDie();
            return [this];
        }
        
        if (IsProtected) // is protected: don't die
            return [];

        var deadPlayers = new List<GamePlayer> { this };

        if (Love != null)
        {
            Love.Love = null; // avoid circular references
            deadPlayers.AddRange(Love.Die(DeathSource.Love));
            Love.Love = this; // restore love
        }

        if (SleepingEndermite != null)
        {
            deadPlayers.AddRange(SleepingEndermite.Die(DeathSource.Endermite));
        }

        ActuallyDie();
        return deadPlayers;
    }
}