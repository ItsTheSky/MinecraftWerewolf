using System;
using CommunityToolkit.Mvvm.Input;
using MinecraftWerewolf.Core;
using MinecraftWerewolf.Core.Models;
using MinecraftWerewolf.ViewModels.Base;

namespace MinecraftWerewolf.ViewModels.Cards;

public partial class IronGolemViewModel : BasePlayerSelectViewModel
{
    private WerewolfGame game;
    
    public IronGolemViewModel(WerewolfGame game, GameCard source) : base(game, source)
    {
        this.game = game;
        AllowDeadPlayers = true;
        base.PlayerSelectCommand = PlayerSelectCommand;
        TaskText = "Le golem de fer protège une personne au choix.";
    }

    [RelayCommand]
    public void OnPlayerSelect(GamePlayer player)
    {
        player.IsProtected = true;
        game.NextCard();
    }

}