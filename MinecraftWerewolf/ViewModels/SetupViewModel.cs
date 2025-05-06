using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MinecraftWerewolf.Core.Models;
using MinecraftWerewolf.Utilities;
using MinecraftWerewolf.ViewModels.PopOvers;
using MinecraftWerewolf.Views.Dialogs;
using Ursa.Common;
using Ursa.Controls;
using Ursa.Controls.Options;

namespace MinecraftWerewolf.ViewModels;

public partial class SetupViewModel : ObservableObject
{
    private readonly MainViewModel _mainViewModel;

    public SetupViewModel(MainViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
    }
    
    [ObservableProperty] private ObservableCollection<PreGamePlayer> _players = [];
    [ObservableProperty] private ObservableCollection<GameCard> _availableCards = new(CardProvider.AllCards);

    [RelayCommand]
    public async Task AddPlayer()
    {
        var options = new DrawerOptions()
        {
            Position = Position.Bottom,
            Buttons = DialogButton.OKCancel,
            IsCloseButtonVisible = false,
            Title = "Ajouter un Joueur",
            CanResize = false
        };

        var vm = new AddPlayerViewModel(CardProvider.AllCards);
        await Drawer.ShowModal<AddPlayerDrawer, AddPlayerViewModel>(vm, "LocalHost", options: options);
        var player = vm.ToPreGamePlayer();
        if (player != null) 
            Players.Add(player);
    }
    
    [RelayCommand]
    public async Task EditPlayer(PreGamePlayer player)
    {
        var options = new DrawerOptions()
        {
            Position = Position.Bottom,
            Buttons = DialogButton.OKCancel,
            IsCloseButtonVisible = false,
            Title = "Modifier un Joueur",
            CanResize = false
        };
            
        var otherPlayers = Players.Where(p => p != player).ToList();
        var vm = new EditPlayerViewModel(CardProvider.AllCards, otherPlayers, player);
        await Drawer.ShowModal<EditPlayerDrawer, EditPlayerViewModel>(vm, "LocalHost", options: options);
        var editedPlayer = vm.ToPreGamePlayer();
        if (editedPlayer != null)
        {
            Players.Remove(player);
            Players.Add(editedPlayer);
        }
    }

    [RelayCommand]
    public void RemovePlayer(PreGamePlayer player)
    {
        Players.Remove(player);
        foreach (var p in Players)
        {
            if (p.Left == player) p.Left = null;
            if (p.Right == player) p.Right = null;
        }
    }

    [RelayCommand]
    public void StartGame()
    {
        _mainViewModel.StartGame();
    }
    
}