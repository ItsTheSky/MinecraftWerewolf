using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using MinecraftWerewolf.Core.Models;
using MinecraftWerewolf.ViewModels.Dialogs;
using MinecraftWerewolf.Views.Dialogs;
using Ursa.Controls;

namespace MinecraftWerewolf.Core;

public partial class WerewolfGame : ObservableObject
{
    public Dictionary<string, object> GameData { get; } = new();

    [ObservableProperty] private ObservableCollection<GamePlayer> _players = new();

    #region Night Data

    [ObservableProperty] private List<GameCard> _nightCards = new();
    [ObservableProperty] private GameCard _playingCard;
    [ObservableProperty] private int _currentNightIndex = 1;
    [ObservableProperty] private bool _isNight = true;
    
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
            // player.Love = null;
            // TODO: add love
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
        var cards = new List<GameCard>();
        foreach (var player in Players)
        {
            if (player.Card is not null && !cards.Contains(player.Card) && player.IsAlive)
                cards.Add(player.Card);
        }

        cards = cards.Where(card => card.ShouldBeCalled(this)).ToList();
        // then, sort by card order (from lower to higher)
        cards.Sort((x, y) => x.Order.CompareTo(y.Order));
        
        Console.WriteLine($"Cards: {string.Join(", ", cards.Select(x => x.DisplayName))}");
        
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
        
        _currentView = null;
        OnPropertyChanged(nameof(CurrentCardView));
    }
    
    public async Task ProcessNightEnd()
    {
        var killedPlayers = new List<GamePlayer>();
        foreach (var player in Players)
        {
            if (player.ShouldDie && player.Card!.ShouldActuallyDie(this))
            {
                killedPlayers.AddRange(player.Die());
            }
        }
        await Dialog.ShowModal<DeathsSumUpDialog, DeathsSumUpViewModel>(new DeathsSumUpViewModel(killedPlayers), options: new DialogOptions()
        {
            Mode = DialogMode.Info,
            StartupLocation = WindowStartupLocation.CenterScreen,
            IsCloseButtonVisible = false,
            CanResize = false,
            Title = "Résumé des Morts",
        });

        SetupDay();
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