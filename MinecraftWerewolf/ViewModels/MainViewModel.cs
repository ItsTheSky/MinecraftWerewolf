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

        const int samplePlayers = 15;
        var usedCards = new HashSet<GameCard>();
        for (var i = 0; i < samplePlayers; i++)
        {
            var card = CardProvider.AllCards[i % CardProvider.AllCards.Count];
            while (!card.AllowMultiple && usedCards.Contains(card))
            {
                i++;
                
                card = CardProvider.AllCards[i % CardProvider.AllCards.Count];
            }
            
            usedCards.Add(card);
            var index = SetupViewModel.Players.Count;
            SetupViewModel.Players.Add(new PreGamePlayer()
            {
                Name = $"Player {index + 1}",
                Card = card
            });
        }
        
        // now, link all left/right players of players randomly
        for (var i = 0; i < SetupViewModel.Players.Count; i++)
        {
            var player = SetupViewModel.Players[i];
            var leftIndex = (i - 1 + SetupViewModel.Players.Count) % SetupViewModel.Players.Count;
            var rightIndex = (i + 1) % SetupViewModel.Players.Count;
            
            player.Left = SetupViewModel.Players[leftIndex];
            player.Right = SetupViewModel.Players[rightIndex];
        }
    }
    
    [ObservableProperty] private GameViewModel _gameViewModel;
    [ObservableProperty] private SetupViewModel _setupViewModel;

    [ObservableProperty] private bool _isInSetup = true;

    [RelayCommand]
    public void StartGame()
    {
        GameViewModel = new GameViewModel(SetupViewModel.Players.ToList(), this);
        IsInSetup = false;
    }
    
    [RelayCommand]
    public void StopGame()
    {
        GameViewModel = null;
        IsInSetup = true;
    }
}