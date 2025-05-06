using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Media;
using MinecraftWerewolf.Core.Models;
using MinecraftWerewolf.ViewModels.Cards;
using MinecraftWerewolf.Views.Cards;

namespace MinecraftWerewolf.Core.Cards;

public class Bee : GameCard
{
    public override int Order => 40;
    public override string Id => "bee";
    public override string DisplayName => "Abeille";

    public override string Description =>
        "A le choix entre: soit polliniser (= protéger), soit piquer (= 'mourant') un joueur.";
    public override Color Color => Colors.Yellow;
    public override List<CardTeam> Teams => [CardTeam.Villager];
    
    public override Control CreateCardControl(WerewolfGame game)
    {
        return new BeeView { DataContext = new BeeViewModel(game, this) };
    }
}