using System;
using CommunityToolkit.Mvvm.Input;
using MinecraftWerewolf.Core;
using MinecraftWerewolf.Core.Models;
using MinecraftWerewolf.ViewModels.Base;

namespace MinecraftWerewolf.ViewModels.Cards;

public partial class EndermanViewModel : BasePlayerSelectViewModel
{
    private WerewolfGame game;
    
    public EndermanViewModel(WerewolfGame game, GameCard source) : base(game, source)
    {
        this.game = game;
        AllowDeadPlayers = true;
        base.PlayerSelectCommand = PlayerSelectCommand;
        TaskText = "Les endermans doivent se décider pour choisir un joueur à tuer.";
    }

    [RelayCommand]
    public void OnPlayerSelect(GamePlayer player)
    {
        player.ShouldDie = true;
        Console.Write($"{player.Name} has been selected to die.");
        
        game.NextCard();
    }

}