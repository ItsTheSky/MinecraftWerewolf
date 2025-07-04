using System;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MinecraftWerewolf.Core.Models;
using MinecraftWerewolf.Utilities;

namespace MinecraftWerewolf.ViewModels;

public partial class SetupViewModel : ObservableObject
{
    private readonly MainViewModel _mainViewModel;

    public SetupViewModel(MainViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
        
        // Initialize card quantities with all available cards
        CardQuantities = new ObservableCollection<CardQuantity>(
            CardProvider.AllCards.Select(card => new CardQuantity(card, 0))
        );
        
        // Subscribe to quantity changes for all cards
        foreach (var cardQuantity in CardQuantities)
        {
            cardQuantity.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(CardQuantity.Quantity))
                {
                    OnPropertyChanged(nameof(TotalPlayers));
                    OnPropertyChanged(nameof(CanStartGame));
                }
            };
        }
    }
    
    [ObservableProperty] private ObservableCollection<CardQuantity> _cardQuantities = [];

    public int TotalPlayers => CardQuantities.Sum(cq => cq.Quantity);
    public bool CanStartGame => TotalPlayers >= 4; // Minimum players for a game

    [RelayCommand]
    public void IncreaseQuantity(CardQuantity cardQuantity)
    {
        if (cardQuantity.CanIncrease)
            cardQuantity.Quantity++;
    }

    [RelayCommand]
    public void DecreaseQuantity(CardQuantity cardQuantity)
    {
        if (cardQuantity.Quantity > 0)
            cardQuantity.Quantity--;
    }

    [RelayCommand]
    public void ResetQuantities()
    {
        foreach (var cardQuantity in CardQuantities)
        {
            cardQuantity.Quantity = 0;
        }
    }

    [RelayCommand]
    public void SetPreset(string presetName)
    {
        ResetQuantities();
        
        switch (presetName.ToLower())
        {
            case "small":
                // 4-6 players preset
                SetCardQuantity("enderman", 1);
                SetCardQuantity("allay", 1);
                SetCardQuantity("iron_golem", 1);
                SetCardQuantity("spider", 1);
                break;
                
            case "medium":
                // 7-10 players preset
                SetCardQuantity("enderman", 2);
                SetCardQuantity("allay", 1);
                SetCardQuantity("iron_golem", 1);
                SetCardQuantity("spider", 1);
                SetCardQuantity("bee", 1);
                SetCardQuantity("axolotl", 1);
                SetCardQuantity("blaze", 1);
                break;
                
            case "large":
                // 11+ players preset
                SetCardQuantity("enderman", 3);
                SetCardQuantity("allay", 1);
                SetCardQuantity("iron_golem", 1);
                SetCardQuantity("spider", 1);
                SetCardQuantity("bee", 2);
                SetCardQuantity("axolotl", 1);
                SetCardQuantity("blaze", 1);
                SetCardQuantity("endermite", 1);
                SetCardQuantity("creeper", 1);
                break;
        }
    }

    public void ApplyPreset(string presetName)
    {
        SetPreset(presetName);
    }

    private void SetCardQuantity(string cardId, int quantity)
    {
        var cardQuantity = CardQuantities.FirstOrDefault(cq => cq.Card.Id == cardId);
        if (cardQuantity != null)
        {
            // Respect the max quantity limit
            var maxQuantity = cardQuantity.MaxQuantity;
            cardQuantity.Quantity = Math.Min(quantity, maxQuantity);
        }
    }

    [RelayCommand]
    public void StartGame()
    {
        if (!CanStartGame)
            return;
            
        // Generate players from card quantities
        var players = new List<PreGamePlayer>();
        foreach (var cardQuantity in CardQuantities.Where(cq => cq.Quantity > 0))
        {
            players.AddRange(cardQuantity.GeneratePlayers());
        }

        // Shuffle players to randomize seating
        var shuffledPlayers = players.OrderBy(_ => Random.Shared.Next()).ToList();
        
        // Link left/right players in a circle
        for (var i = 0; i < shuffledPlayers.Count; i++)
        {
            var leftIndex = (i - 1 + shuffledPlayers.Count) % shuffledPlayers.Count;
            var rightIndex = (i + 1) % shuffledPlayers.Count;
            
            shuffledPlayers[i].Left = shuffledPlayers[leftIndex];
            shuffledPlayers[i].Right = shuffledPlayers[rightIndex];
        }

        _mainViewModel.StartGameWithPlayers(shuffledPlayers);
    }
}
