using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.Input;
using MinecraftWerewolf.Core.Models;
using MinecraftWerewolf.ViewModels.Cards;
using MinecraftWerewolf.Views.Base;
using MinecraftWerewolf.Views.Cards;

namespace MinecraftWerewolf.Core.Cards;

public class Enderman : GameCard
{

    public override int Order => 70;
    public override string Id => "enderman";
    public override string DisplayName => "Enderman";
    public override string Description => "Similaire aux \"Loup-Garou\"; ils se réunissent chaque nuit pour décider d'une personne à tuer.";
    public override Color Color => Color.Parse("#CA00F8");
    public override List<CardTeam> Teams => [CardTeam.Monster];
    public override bool AllowMultiple => true;

    public override bool ShouldBeCalled(WerewolfGame game)
    {
        // Check if at least one Enderman is not paralyzed and alive
        return game.Players.Any(p => p.Card?.Id == "enderman" && p is { IsAlive: true, IsParalyzed: false });
    }

    public override Control CreateCardControl(WerewolfGame game)
    {
        return new BasePlayerSelectView { DataContext = new EndermanViewModel(game, this) };
    }
}