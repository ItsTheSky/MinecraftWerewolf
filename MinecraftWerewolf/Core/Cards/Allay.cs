using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Media;
using MinecraftWerewolf.Core.Models;
using MinecraftWerewolf.ViewModels.Cards;
using MinecraftWerewolf.Views.Cards;

namespace MinecraftWerewolf.Core.Cards;

public class Allay : GameCard
{
    public const string HasUsedHealPotionKey = "allay_has_used_heal_potion";
    public const string HasUsedDeathPotionKey = "allay_has_used_death_potion";
    
    public override int Order => 90;
    public override string Id => "allay";
    public override string DisplayName => "Allay";
    public override string Description => "Peut utiliser 2 potions: une potion de Heal (résurrection) et une potion de Dégâts (mort). Une seule utilisation de chaque.";
    public override Color Color => Color.Parse("#49F3E8");
    public override List<CardTeam> Teams => [CardTeam.Villager, CardTeam.Healer];

    public override Control CreateCardControl(WerewolfGame game)
    {
        return new AllayView { DataContext = new AllayViewModel(game, this) };
    }
}