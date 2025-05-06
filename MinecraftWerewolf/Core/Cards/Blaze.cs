using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Media;
using MinecraftWerewolf.Core.Models;
using MinecraftWerewolf.ViewModels.Cards;
using MinecraftWerewolf.Views.Cards;

namespace MinecraftWerewolf.Core.Cards;

public class Blaze : GameCard
{
    public override int Order => 50;
    public override string Id => "blaze";
    public override string DisplayName => "Blaze";
    public override string Description => "Peut 'enflammer' (= rendre un joueur 'mourant') un joueur de son choix, une fois par nui.";
    public override Color Color => Colors.Gold;
    public override List<CardTeam> Teams => [CardTeam.Monster];
    
    public override Control CreateCardControl(WerewolfGame game)
    {
        return new BasePlayerSelectView() { DataContext = new BlazeViewModel(game, this) };
    }
}