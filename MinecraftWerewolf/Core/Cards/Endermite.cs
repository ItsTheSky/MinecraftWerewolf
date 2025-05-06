using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Media;
using MinecraftWerewolf.Core.Models;
using MinecraftWerewolf.ViewModels.Cards;
using MinecraftWerewolf.Views.Cards;

namespace MinecraftWerewolf.Core.Cards;

public class Endermite : GameCard
{
    public override int Order => 40;
    public override string Id => "endermite";
    public override string DisplayName => "Endermite";
    public override string Description => "Peut choisir de \"dormir\" chez quelqu'un; l'endermite sera alors protégé si elle se fait attaquer.";
    public override Color Color => Color.Parse("#756089");
    public override List<CardTeam> Teams => [CardTeam.Monster];
    

    public override Control CreateCardControl(WerewolfGame game)
    {
        return new BasePlayerSelectView { DataContext = new EndermiteViewModel(game, this) };
    }

    public override bool ShouldActuallyDie(WerewolfGame game)
    {
        // don't die if we're not at home :)
        return !game.Players.Any(p =>
        {
            Console.WriteLine($"Has sleeping endermite: {p.SleepingEndermite != null} - {p.Name}");
            return p.SleepingEndermite != null;
        });
    }
}