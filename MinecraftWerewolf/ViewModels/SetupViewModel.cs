using System;
using System.Collections.Generic;
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
        
        // Initialize with default distribution
        DistributeCardsForPlayerCount(_targetPlayerCount);
    }
    
    [ObservableProperty] private ObservableCollection<CardQuantity> _cardQuantities = [];
    [ObservableProperty] private int _targetPlayerCount = 8;

    public int TotalPlayers => CardQuantities.Sum(cq => cq.Quantity);
    public bool CanStartGame => TotalPlayers >= 4; // Minimum players for a game

    partial void OnTargetPlayerCountChanged(int value)
    {
        if (value >= 4 && value <= 20) // Reasonable limits
        {
            DistributeCardsForPlayerCount(value);
        }
    }

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
    public void RedistributeCards()
    {
        DistributeCardsForPlayerCount(TargetPlayerCount);
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

    /// <summary>
    /// Distribue automatiquement les cartes pour atteindre le nombre de joueurs cible
    /// </summary>
    private void DistributeCardsForPlayerCount(int targetCount)
    {
        ResetQuantities();
        
        if (targetCount < 4) return;

        // Priorité des cartes essentielles (dans l'ordre de priorité)
        var essentialCards = new[]
        {
            ("enderman", 0.3), // 30% de la partie environ
            ("allay", 1),       // 1 seul
            ("iron_golem", 1),  // 1 seul  
            ("spider", 1),      // 1 seul
            ("axolotl", 1),     // 1 seul si assez de joueurs
            ("bee", 0.15),      // ~15% du total
            ("blaze", 1),       // 1 seul pour équilibrer
            ("endermite", 1),   // 1 seul si assez de joueurs
            ("creeper", 1)      // 1 seul pour les grandes parties
        };

        var currentTotal = 0;
        
        // Phase 1: Cartes essentielles uniques
        foreach (var (cardId, ratio) in essentialCards)
        {
            if (currentTotal >= targetCount) break;
            
            var quantity = ratio < 1 ? Math.Max(1, (int)(targetCount * ratio)) : (int)ratio;
            
            // Logique spécifique selon le nombre de joueurs
            switch (cardId)
            {
                case "axolotl":
                    if (targetCount >= 6) 
                    {
                        SetCardQuantity(cardId, 1);
                        currentTotal += 1;
                    }
                    break;
                case "endermite":
                    if (targetCount >= 9)
                    {
                        SetCardQuantity(cardId, 1);
                        currentTotal += 1;
                    }
                    break;
                case "creeper":
                    if (targetCount >= 12)
                    {
                        SetCardQuantity(cardId, 1);
                        currentTotal += 1;
                    }
                    break;
                case "enderman":
                    // Ajuster les endermen selon la taille
                    var endermanCount = Math.Max(1, Math.Min(quantity, targetCount / 3));
                    SetCardQuantity(cardId, endermanCount);
                    currentTotal += endermanCount;
                    break;
                case "bee":
                    // Ajouter des abeilles selon la taille
                    var beeCount = targetCount >= 8 ? Math.Max(1, quantity) : 0;
                    if (beeCount > 0)
                    {
                        SetCardQuantity(cardId, beeCount);
                        currentTotal += beeCount;
                    }
                    break;
                default:
                    if (currentTotal + quantity <= targetCount)
                    {
                        SetCardQuantity(cardId, quantity);
                        currentTotal += quantity;
                    }
                    break;
            }
        }

        // Phase 2: Compléter avec des cartes additionnelles si nécessaire
        while (currentTotal < targetCount)
        {
            var remaining = targetCount - currentTotal;
            
            if (remaining >= 2)
            {
                // Ajouter plus d'endermen si possible
                var endermanCard = CardQuantities.FirstOrDefault(cq => cq.Card.Id == "enderman");
                if (endermanCard?.CanIncrease == true)
                {
                    endermanCard.Quantity++;
                    currentTotal++;
                    continue;
                }
                
                // Ajouter plus d'abeilles si possible
                var beeCard = CardQuantities.FirstOrDefault(cq => cq.Card.Id == "bee");
                if (beeCard?.CanIncrease == true)
                {
                    beeCard.Quantity++;
                    currentTotal++;
                    continue;
                }
            }
            
            // Si on ne peut plus ajouter, on s'arrête
            break;
        }

        // Mise à jour des notifications
        OnPropertyChanged(nameof(TotalPlayers));
        OnPropertyChanged(nameof(CanStartGame));
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
