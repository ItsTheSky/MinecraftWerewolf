using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MinecraftWerewolf.Core.Models;

namespace MinecraftWerewolf.ViewModels.PopOvers;

public partial class EditPlayerViewModel : ObservableObject
{

    public EditPlayerViewModel(List<GameCard> availableCards, List<PreGamePlayer> otherPlayers, PreGamePlayer player)
    {
        AvailableCards = new ObservableCollection<GameCard>(availableCards);
        OtherPlayers = new ObservableCollection<PreGamePlayer>(otherPlayers);
        
        Name = player.Name;
        Card = player.Card;
        Left = player.Left;
        Right = player.Right;
        
        PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName is nameof(Name) or nameof(Card))
                OnPropertyChanged(nameof(IsValid));
        };
    }
    
    [ObservableProperty] private ObservableCollection<GameCard> _availableCards;
    [ObservableProperty] private ObservableCollection<PreGamePlayer> _otherPlayers;
    [ObservableProperty] private string? _name;
    [ObservableProperty] private GameCard? _card;

    [ObservableProperty] private PreGamePlayer? _left;
    [ObservableProperty] private PreGamePlayer? _right;
    
    public bool IsValid => !string.IsNullOrWhiteSpace(Name) && Card != null;
    
    public PreGamePlayer? ToPreGamePlayer()
    {
        if (!IsValid)
            return null;
        
        return new PreGamePlayer
        {
            Name = Name!,
            Card = Card!,
            
            Left = Left,
            Right = Right
        };
    }
    
}