using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Media;
using MinecraftWerewolf.Core.Models;
using MinecraftWerewolf.ViewModels.Cards;
using MinecraftWerewolf.Views.Cards;

namespace MinecraftWerewolf.Core.Cards;

public class Spider : GameCard
{
    public override int Order => 10;
    public override string Id => "spider";
    public override string DisplayName => "Araignée";
    public override string Description => "Peut paralyser une personne. Cette personne ne sera alors pas appelée lors de la nuit en cours.";
    public override Color Color => Colors.Crimson;
    public override List<CardTeam> Teams => [CardTeam.Villager];
    
    public override Control CreateCardControl(WerewolfGame game)
    {
        return new BasePlayerSelectView { DataContext = new SpiderViewModel(game, this) };
    }
}