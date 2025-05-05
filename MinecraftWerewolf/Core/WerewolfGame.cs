using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

    [ObservableProperty] private Queue<GameCard> _nightCards = new();
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
        NextCard();
    }

    public void SetupNight()
    {
        IsNight = true;
        NightCards.Clear();

        // first, gather all different cards
        var cards = new List<GameCard>();
        foreach (var player in Players)
        {
            if (player.Card is not null && !cards.Contains(player.Card))
                cards.Add(player.Card);
        }
        
        // then, sort by card order (from lower to higher)
        cards.Sort((x, y) => x.Order.CompareTo(y.Order));
        
        // finally, add them to the night cards stack
        foreach (var card in cards)
            NightCards.Enqueue(card);
    }

    public void SetupDay()
    {
        IsNight = false;
        
        foreach (var player in Players)
        {
            player.IsProtected = false;
            player.ShouldDie = false;
        }
    }

    public void NextCard()
    {
        if (NightCards.Count == 0)
        {
            _ = Dispatcher.UIThread.InvokeAsync(ProcessNightEnd);
            return;
        }

        PlayingCard = NightCards.Dequeue();
        if (!PlayingCard.ShouldBeCalled(this))
        {
            NextCard();
            return;
        }
        
        _currentView = null;
        OnPropertyChanged(nameof(CurrentCardView));
    }
    
    public async Task ProcessNightEnd()
    {
        var killedPlayers = new List<GamePlayer>();
        var lovers = new List<GamePlayer>();
        foreach (var player in Players)
        {
            if (player.ShouldDie && player.Card!.ShouldActuallyDie(this))
            {
                var love = player.Love;
                player.Die();
                killedPlayers.Add(player);
                
                if (love != null)
                {
                    love.Die();
                    killedPlayers.Add(love);
                    
                    lovers.Add(love);
                    lovers.Add(player);
                }
            }
        }

        await Dialog.ShowModal<NightSumUpDialog, NightSumUpViewModel>(new NightSumUpViewModel(killedPlayers,
            lovers.Count > 0 ? lovers[0] : null,
            lovers.Count > 1 ? lovers[1] : null), options: new DialogOptions()
        {
            Mode = DialogMode.Info,
            StartupLocation = WindowStartupLocation.CenterScreen,
            IsCloseButtonVisible = false,
            CanResize = false
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
}