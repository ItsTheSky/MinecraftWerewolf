using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MinecraftWerewolf.Core.Models;
using MinecraftWerewolf.Utilities;

namespace MinecraftWerewolf.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    
    public MainViewModel()
    {
        SetupViewModel = new SetupViewModel(this);

        // Initialize with a sample configuration for testing
        SetupViewModel.ApplyPreset("medium");
    }
    
    [ObservableProperty] private GameViewModel _gameViewModel;
    [ObservableProperty] private SetupViewModel _setupViewModel;

    [ObservableProperty] private bool _isInSetup = true;

    [RelayCommand]
    public void StartGame()
    {
        // This method is kept for backward compatibility, but now delegates to SetupViewModel
        SetupViewModel.StartGame();
    }
    
    public void StartGameWithPlayers(List<PreGamePlayer> players)
    {
        GameViewModel = new GameViewModel(players, this);
        IsInSetup = false;
    }
    
    [RelayCommand]
    public void StopGame()
    {
        GameViewModel = null;
        IsInSetup = true;
    }
}
