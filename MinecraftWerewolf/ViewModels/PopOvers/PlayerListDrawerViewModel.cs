using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MinecraftWerewolf.Core;
using Ursa.Controls;

namespace MinecraftWerewolf.ViewModels.PopOvers;

public partial class PlayerListDrawerViewModel(GameViewModel vm) : ObservableObject
{
    
    [ObservableProperty] private ObservableCollection<GamePlayer> _players;

    [RelayCommand]
    public void StopGame()
    {
        vm.StopGame();
    }
}