using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Media;
using MinecraftWerewolf.Core.Models;

namespace MinecraftWerewolf.Core.Cards;

public class Creeper : GameCard
{
    public override int Order => 80;
    public override string Id => "creeper";
    public override string DisplayName => "Creeper";

    public override string Description =>
        "Trop compliquer à expliquer comme ça, mais c'est vraiment plus simple que ce que l'on pense!";
    public override Color Color => Colors.Green;
    public override List<CardTeam> Teams => [CardTeam.Monster];

    public override bool ShouldActuallyDie(WerewolfGame game)
    {
        var player = game.FindPlayerWithCard<Creeper>();
        if (player != null)
        {
            if (player.IsCharged)
                return true;
            
            Console.WriteLine("Creeper is charged!");
            player.IsCharged = true;
            return false;
        }
        
        return base.ShouldActuallyDie(game);
    }

    public override Control CreateCardControl(WerewolfGame game)
    {
        throw new InvalidOperationException("Creeper card should not be created in the game.");
    }

    public override bool ShouldBeCalled(WerewolfGame game)
    {
        return false; // never called that way.
    }
}