using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MinecraftWerewolf.Core;
using MinecraftWerewolf.Core.Models;

namespace MinecraftWerewolf.ViewModels.Base;

public partial class BasePlayerSelectViewModel(WerewolfGame game, GameCard source) : PlayerListViewModel(game.Players, source)
{

    [ObservableProperty] private bool _allowNoSelection;
    [ObservableProperty] private string _taskText;

    public PlayerListViewModel ViewModel => this;

    [RelayCommand]
    public void NoSelection()
    {
        game.NextCard();
    }
}