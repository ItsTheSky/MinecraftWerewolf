using System;
using CommunityToolkit.Mvvm.Input;
using MinecraftWerewolf.Core;
using MinecraftWerewolf.Core.Cards;
using MinecraftWerewolf.Core.Models;
using MinecraftWerewolf.ViewModels.Base;

namespace MinecraftWerewolf.ViewModels.Cards;

public partial class EndermiteViewModel : BasePlayerSelectViewModel
{
    private WerewolfGame game;
    
    public EndermiteViewModel(WerewolfGame game, GameCard source) : base(game, source,
        PlayerListType.OnlyAlive)
    {
        this.game = game;
        base.PlayerSelectCommand = PlayerSelectCommand;
    }

    [RelayCommand]
    public void OnPlayerSelect(GamePlayer player)
    {
        if (player != null!)
            player.SleepingEndermite = game.FindPlayerWithCard<Endermite>();
        
        game.NextCard();
    }
    
}