using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MinecraftWerewolf.Core;
using MinecraftWerewolf.Core.Cards;
using MinecraftWerewolf.Core.Models;
using MinecraftWerewolf.ViewModels.Base;

namespace MinecraftWerewolf.ViewModels.Cards;

public partial class AllayViewModel : BasePlayerSelectViewModel
{
    private WerewolfGame game;
    
    public AllayViewModel(WerewolfGame game, GameCard source) : base(game, source, PlayerListType.All)
    {
        this.game = game;
        base.PlayerSelectCommand = PlayerSelectCommand;
        AllowSelf = true;
  
        // Check which potions have been used
        HasUsedHealPotion = game.GameData.ContainsKey(Allay.HasUsedHealPotionKey);
        HasUsedDeathPotion = game.GameData.ContainsKey(Allay.HasUsedDeathPotionKey);
        
        OnPropertyChanged(nameof(HasPotionsAvailable));
        OnPropertyChanged(nameof(HasBothPotions));

        // Get dying players for heal selection
        DyingPlayers = new System.Collections.ObjectModel.ObservableCollection<GamePlayer>();
        foreach (var player in game.Players)
        {
            if (player.ShouldDie)
            {
                DyingPlayers.Add(player);
            }
        }
        
        // Determine which list type to show based on available potions
        UpdateListTypeAndCommand();
    }
    
    [ObservableProperty] private bool _hasUsedHealPotion;
    [ObservableProperty] private bool _hasUsedDeathPotion;
    [ObservableProperty] private bool _showDeathOption;
    [ObservableProperty] private bool _showHealOption;
    [ObservableProperty] private System.Collections.ObjectModel.ObservableCollection<GamePlayer> _dyingPlayers;
    
    public bool HasPotionsAvailable => !HasUsedHealPotion || !HasUsedDeathPotion;
    public bool HasBothPotions => !HasUsedHealPotion && !HasUsedDeathPotion;
    
    private void UpdateListTypeAndCommand()
    {
        // If both potions are available, show choice buttons but default to heal if there are dying players
        if (HasBothPotions)
        {
            if (DyingPlayers.Count > 0)
            {
                // Default to heal option but allow switching
                ShowHealOption = true;
                ShowDeathOption = false;
                Filter = PlayerListType.OnlyDying;
            }
            else
            {
                // No dying players, default to death option
                ShowDeathOption = true;
                ShowHealOption = false;
                Filter = PlayerListType.OnlyPartialAlive;
            }
        }
        else if (!HasUsedHealPotion && DyingPlayers.Count > 0)
        {
            ShowHealOption = true;
            ShowDeathOption = false;
            Filter = PlayerListType.OnlyDying;
        }
        else if (!HasUsedDeathPotion)
        {
            ShowDeathOption = true;
            ShowHealOption = false;
            Filter = PlayerListType.OnlyPartialAlive;
        }
        else
        {
            // No potions available, show empty list
            ShowDeathOption = false;
            ShowHealOption = false;
        }
    }
    
    [RelayCommand]
    public void OnPlayerSelect(GamePlayer player)
    {
        if (ShowHealOption && !HasUsedHealPotion)
        {
            // Resurrect the player
            player.ShouldDie = false;
            game.GameData.Add(Allay.HasUsedHealPotionKey, true);
            HasUsedHealPotion = true;
        }
        else if (ShowDeathOption && !HasUsedDeathPotion)
        {
            // Kill the player
            player.PrepareDeath(DeathSource.Allay);
            game.GameData.Add(Allay.HasUsedDeathPotionKey, true);
            HasUsedDeathPotion = true;
        }
        
        game.NextCard();
    }
    
    [RelayCommand]
    public void OnNoAction()
    {
        game.NextCard();
    }

    [RelayCommand]
    public void SwitchToHealPotion()
    {
        if (!HasUsedHealPotion)
        {
            ShowHealOption = true;
            ShowDeathOption = false;
            Filter = PlayerListType.OnlyDying;
        }
    }
    
    [RelayCommand]
    public void SwitchToDeathPotion()
    {
        if (!HasUsedDeathPotion)
        {
            ShowDeathOption = true;
            ShowHealOption = false;
            Filter = PlayerListType.OnlyPartialAlive;
        }
    }
}