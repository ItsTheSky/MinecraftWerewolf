using System;
using CommunityToolkit.Mvvm.Input;
using MinecraftWerewolf.Core;
using MinecraftWerewolf.Core.Models;
using MinecraftWerewolf.ViewModels.Base;

namespace MinecraftWerewolf.ViewModels.Cards;

public partial class EndermanViewModel : BasePlayerSelectViewModel
{
    private WerewolfGame game;
    
    public EndermanViewModel(WerewolfGame game, GameCard source) : base(game, source,
        PlayerListType.OnlyAlive)
    {
        this.game = game;
        base.PlayerSelectCommand = PlayerSelectCommand;

        AllowSelf = true;
    }

    [RelayCommand]
    public void OnPlayerSelect(GamePlayer player)
    {
        player.PrepareDeath(DeathSource.Enderman);
        Console.Write($"{player.Name} has been selected to die.");
        
        game.NextCard();
    }

}