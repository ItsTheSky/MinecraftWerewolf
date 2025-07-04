using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MinecraftWerewolf.Core.Models;

/// <summary>
/// Represent a card with its quantity for game setup.
/// </summary>
public partial class CardQuantity : ObservableObject
{
    public CardQuantity(GameCard card, int quantity = 0)
    {
        Card = card;
        Quantity = quantity;
    }

    [ObservableProperty] private GameCard _card;
    [ObservableProperty] private int _quantity;

    partial void OnQuantityChanged(int value)
    {
        OnPropertyChanged(nameof(CanIncrease));
    }

    /// <summary>
    /// Maximum quantity allowed for this card.
    /// </summary>
    public int MaxQuantity => Card.AllowMultiple ? 10 : 1;

    /// <summary>
    /// Whether the quantity can be increased.
    /// </summary>
    public bool CanIncrease => Quantity < MaxQuantity;

    /// <summary>
    /// Generate players from this card quantity.
    /// </summary>
    public List<PreGamePlayer> GeneratePlayers()
    {
        var players = new List<PreGamePlayer>();
        
        for (int i = 0; i < Quantity; i++)
        {
            var name = Quantity == 1 ? Card.DisplayName : $"{Card.DisplayName} {i + 1}";
            players.Add(new PreGamePlayer
            {
                Name = name,
                Card = Card
            });
        }
        
        return players;
    }
}
