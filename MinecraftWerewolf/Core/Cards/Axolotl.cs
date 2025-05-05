using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Media;
using MinecraftWerewolf.Core.Models;
using MinecraftWerewolf.ViewModels.Cards;
using MinecraftWerewolf.Views.Cards;

namespace MinecraftWerewolf.Core.Cards;

public class Axolotl : GameCard
{
    public const string HasMadeLoverKey = "hax_made_lover";

    public override int Order => 1;
    public override string Id => "axolotl";
    public override string DisplayName => "Axolotl";
    public override string Description => "Lors de la première nuit, met deux personnes ensemble.";
    public override Color Color => Colors.DodgerBlue;
    public override List<CardTeam> Teams => [CardTeam.Villager, CardTeam.Healer];

    public override bool ShouldBeCalled(WerewolfGame game)
    {
        if (game.GameData.ContainsKey(HasMadeLoverKey))
            return false;
        
        game.GameData.Add(HasMadeLoverKey, true);
        return true;
    }

    public override Control CreateCardControl(WerewolfGame game)
    {
        return new AxolotlView { DataContext = new AxolotlViewModel(game, this) };
    }
}