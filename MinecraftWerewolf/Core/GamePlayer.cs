using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MinecraftWerewolf.Core.Models;
using MinecraftWerewolf.ViewModels.PopOvers;
using MinecraftWerewolf.Views.Dialogs;
using Ursa.Controls;

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
    
    [ObservableProperty] private bool _isCharged;
    
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

    public async Task<List<PlayerDeath>> Die(WerewolfGame game, DeathSource? forceSource = null, int level = 0)
    {
        var source = (forceSource ?? LastDeathSource)!.Value;

        var deadPlayers = new List<PlayerDeath> { new(this, source, level) };
        
        if (IsProtected && source != DeathSource.Love && source != DeathSource.Creeper && source != DeathSource.DayVote)
            return [];

        if (Love != null)
        {
            Love.Love = null; // avoid circular references
            deadPlayers.AddRange(await Love.Die(game, DeathSource.Love, level + 1));
            Love.Love = this; // restore love
        }

        if (SleepingEndermite != null && !IsProtected)
        {
            deadPlayers.AddRange(await SleepingEndermite.Die(game, DeathSource.Endermite, level + 1));
        }

        if (Card!.Id == "creeper" && IsCharged)
        {
            var vm = new ChargedCreeperSelectorViewModel(game);
            await OverlayDialog.ShowModal<ChargedCreeperSelectorDialog, ChargedCreeperSelectorViewModel>(
                vm, "LocalHost", options: new OverlayDialogOptions()
                {
                    Mode = DialogMode.Info,
                    IsCloseButtonVisible = false,
                    CanResize = false,
                    Title = "Choix des Morts",
                    Buttons = DialogButton.OK
                });

            if (vm.LeftPlayer != null)
                deadPlayers.AddRange(await vm.LeftPlayer.Base.Die(game, DeathSource.Creeper, level + 1));
            if (vm.RightPlayer != null)
                deadPlayers.AddRange(await vm.RightPlayer.Base.Die(game, DeathSource.Creeper, level + 1));
        }

        ActuallyDie();
        return deadPlayers;
    }
}