using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Media;
using MinecraftWerewolf.Core.Models;
using MinecraftWerewolf.ViewModels.Cards;
using MinecraftWerewolf.Views.Cards;

namespace MinecraftWerewolf.Core.Cards;

public class IronGolem : GameCard
{
    public const string DoubleLifeKey = "double_life";
    
    public override int Order => 20;
    public override string Id => "iron_golem";
    public override string DisplayName => "Golem de Fer";
    public override string Description => "Peut proteger une personne par nuit. A deux vies.";
    public override Color Color => Color.Parse("#EEEEEE");
    public override List<CardTeam> Teams => [CardTeam.Villager, CardTeam.Healer];

    public override Control CreateCardControl(WerewolfGame game)
    {
        return new BasePlayerSelectView { DataContext = new IronGolemViewModel(game, this) };
    }

    public override bool ShouldActuallyDie(WerewolfGame game)
    {
        if (game.GameData.ContainsKey(DoubleLifeKey)) // if it contains, the golem was already dead one time
            return true;
        
        game.GameData.Add(DoubleLifeKey, true);
        return false;
    }
}