using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Media;
using MinecraftWerewolf.Core.Models;
using MinecraftWerewolf.ViewModels.Cards;
using MinecraftWerewolf.Views.Cards;

namespace MinecraftWerewolf.Core.Cards;

public class Healer : GameCard
{

    public override int Order => 60;
    public override string Id => "healer";
    public override string DisplayName => "Guérisseurs";
    public override string Description => "Tous les joueurs ayant une carte \"guérisseur\" sont appelés, afin de choisir un joueur morant, et de le sauver.";
    public override Color Color => Colors.ForestGreen;
    public override List<CardTeam> Teams => [CardTeam.Villager];
    
    
    public override Control CreateCardControl(WerewolfGame game)
    {
        return new BasePlayerSelectView { DataContext = new HealerViewModel(game, this) };
    }

    public override bool ShouldBeCalled(WerewolfGame game)
    {
        return game.Players.Any(p => p.Card!.Teams.Contains(CardTeam.Healer));
    }
}