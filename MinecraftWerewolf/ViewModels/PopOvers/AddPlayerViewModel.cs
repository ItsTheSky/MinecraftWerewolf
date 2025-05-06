using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MinecraftWerewolf.Core.Models;

namespace MinecraftWerewolf.ViewModels.PopOvers;

public partial class AddPlayerViewModel : ObservableObject
{

    public AddPlayerViewModel(List<GameCard> availableCards)
    {
        AvailableCards = new ObservableCollection<GameCard>(availableCards);
        
        PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName is nameof(Name) or nameof(Card))
                OnPropertyChanged(nameof(IsValid));
        };
    }
    
    [ObservableProperty] private ObservableCollection<GameCard> _availableCards;
    [ObservableProperty] private string? _name;
    [ObservableProperty] private GameCard? _card;
    
    public bool IsValid => !string.IsNullOrWhiteSpace(Name) && Card != null;
    
    public PreGamePlayer? ToPreGamePlayer()
    {
        if (!IsValid)
            return null;
        
        return new PreGamePlayer
        {
            Name = Name!,
            Card = Card!,
        };
    }
    
}