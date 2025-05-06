using CommunityToolkit.Mvvm.Input;
using MinecraftWerewolf.Core;
using MinecraftWerewolf.Core.Models;
using MinecraftWerewolf.ViewModels.Base;

namespace MinecraftWerewolf.ViewModels.Cards;

public partial class HealerViewModel : BasePlayerSelectViewModel
{
    private WerewolfGame game;
    
    public HealerViewModel(WerewolfGame game, GameCard source) : base(game, source)
    {
        this.game = game;
        
        ListType = PlayerListType.OnlyDying;
        AllowSelf = true;
        AllowNoSelection = true;
        
        base.PlayerSelectCommand = PlayerSelectCommand;
    }

    [RelayCommand]
    public void OnPlayerSelect(GamePlayer player)
    {
        player.ShouldDie = false;
        game.NextCard();
    }

}