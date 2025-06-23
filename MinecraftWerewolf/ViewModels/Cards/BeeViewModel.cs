using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MinecraftWerewolf.Core;
using MinecraftWerewolf.Core.Models;
using MinecraftWerewolf.ViewModels.Base;

namespace MinecraftWerewolf.ViewModels.Cards;

public partial class BeeViewModel : BasePlayerSelectViewModel
{
    private WerewolfGame game;

    public BeeViewModel(WerewolfGame game, GameCard source) : base(game, source,
        PlayerListType.OnlyAlive)
    {
        this.game = game;
        base.PlayerSelectCommand = PlayerSelectCommand;

        AllowSelf = true;
        AllowNoSelection = true;
    }

    [ObservableProperty] private bool _isProtect = true;

    [RelayCommand]
    public void OnPlayerSelect(GamePlayer player)
    {
        if (IsProtect)
            player.IsProtected = true;
        else
            player.PrepareDeath(DeathSource.Bee);
        
        game.NextCard();
    }
}