using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using MinecraftWerewolf.Core.Cards;
using MinecraftWerewolf.Core.Models;
using MinecraftWerewolf.Views.Dialogs;
using Ursa.Controls;
using DeathsSumUpViewModel = MinecraftWerewolf.ViewModels.PopOvers.DeathsSumUpViewModel;

namespace MinecraftWerewolf.Core;

public partial class WerewolfGame : ObservableObject
{
    public WerewolfGame()
    {
        PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName is nameof(CurrentCardIndex) or nameof(MaxCards))
            {
                OnPropertyChanged(nameof(CurrentCardProgress));
                OnPropertyChanged(nameof(CurrentCardProgressText));
            }
        };
    }
    
    public Dictionary<string, object> GameData { get; } = new();

    [ObservableProperty] private ObservableCollection<GamePlayer> _players = new();

    #region Night Data

    [ObservableProperty] private List<GameCard> _nightCards = new();
    [ObservableProperty] private GameCard _playingCard;
    [ObservableProperty] private int _currentNightIndex = 1;
    [ObservableProperty] private bool _isNight = true;

    [ObservableProperty] private int _maxCards;
    [ObservableProperty] private int _currentCardIndex;
    
    public float CurrentCardProgress => ((float)CurrentCardIndex / MaxCards) * 100f;
    public string CurrentCardProgressText => $"{CurrentCardIndex}/{MaxCards} Cartes";
    
    private Control? _currentView;
    public Control? CurrentCardView
    {
        get
        {
            if (_currentView == null)
            {
                if (PlayingCard == null!)
                    return null;
                
                _currentView = PlayingCard.CreateCardControl(this);
            }
            
            return _currentView;
        }
    }

    #endregion

    public void StartGame()
    {
        foreach (var player in Players)
        {
            player.IsAlive = true;
            player.IsProtected = false;
            player.ShouldDie = false;
            player.Love = null;
        }

        CurrentNightIndex = 1;
        GameData.Clear();

        SetupNight();
    }

    public void SetupNight()
    {
        IsNight = true;
        NightCards.Clear();

        // first, gather all different cards
        var cards = new List<GameCard>() { new Healer() };
        foreach (var player in Players)
        {
            if (player.Card is not null && !cards.Contains(player.Card) && player.IsAlive)
                cards.Add(player.Card);
        }

        cards = cards.Where(card => card.ShouldBeCalled(this)).ToList();
        // then, sort by card order (from lower to higher)
        cards.Sort((x, y) => x.Order.CompareTo(y.Order));
        
        Console.WriteLine($"Cards: {string.Join(", ", cards.Select(x => x.DisplayName))}");
        
        MaxCards = cards.Count;
        CurrentCardIndex = 0;
        
        // finally, add them to the night cards
        foreach (var card in cards)
            NightCards.Add(card);
        
        NextCard();
    }

    public void SetupDay()
    {
        IsNight = false;
        
        foreach (var player in Players)
            player.SetupDayVote();
    }

    public void NextCard()
    {
        if (NightCards.Count == 0)
        {
            _ = Dispatcher.UIThread.InvokeAsync(ProcessNightEnd);
            return;
        }

        PlayingCard = NightCards.First();
        if (!PlayingCard.ShouldBeCalled(this))
        {
            NightCards.RemoveAt(0);
            NextCard();
            return;
        }
        
        NightCards.RemoveAt(0);
        CurrentCardIndex++;
        
        _currentView = null;
        OnPropertyChanged(nameof(CurrentCardView));
    }
    
    public async Task ProcessNightEnd()
    {
        try
        {
            var killedPlayers = new List<PlayerDeath>();
            
            // First, determine who should actually die (to avoid order dependency)
            var playersToKill = new List<GamePlayer>();
            foreach (var player in Players)
            {
                if (player.ShouldDie && player.Card!.ShouldActuallyDie(this, player))
                {
                    playersToKill.Add(player);
                }
            }
            
            // Then apply all deaths at once
            foreach (var player in playersToKill)
            {
                killedPlayers.AddRange(await player.Die(this));
            }

            await OverlayDialog.ShowModal<DeathsSumUpDialog, DeathsSumUpViewModel>(
                new DeathsSumUpViewModel(killedPlayers), "LocalHost", options: new OverlayDialogOptions()
                {
                    Mode = DialogMode.Info,
                    IsCloseButtonVisible = false,
                    CanResize = false,
                    Title = "Résumé des Morts",
                    Buttons = DialogButton.OK
                });

            SetupDay();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public GamePlayer? FindPlayerWithCard<T>()
    {
        foreach (var player in Players)
            if (player.Card is T)
                return player;

        return null;
    }

    public GamePlayer? FindPlayerWithCard(string cardId)
    {
        foreach (var player in Players)
            if (player.Card?.Id == cardId)
                return player;

        return null;
    }
}