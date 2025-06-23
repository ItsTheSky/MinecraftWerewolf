using System;
using CommunityToolkit.Mvvm.Input;
using MinecraftWerewolf.Core;
using MinecraftWerewolf.Core.Models;
using MinecraftWerewolf.ViewModels.Base;

namespace MinecraftWerewolf.ViewModels.Cards;

public partial class BlazeViewModel : BasePlayerSelectViewModel
{
    private WerewolfGame game;
    
    public BlazeViewModel(WerewolfGame game, GameCard source) : base(game, source,
        PlayerListType.OnlyAlive)
    {
        this.game = game;
        base.PlayerSelectCommand = PlayerSelectCommand;
    }

    [RelayCommand]
    public void OnPlayerSelect(GamePlayer player)
    {
        player.PrepareDeath(DeathSource.Blaze);
        game.NextCard();
    }

}