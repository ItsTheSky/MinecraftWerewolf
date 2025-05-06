using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using MinecraftWerewolf.Utilities;
using MinecraftWerewolf.ViewModels;
using MinecraftWerewolf.ViewModels.PopOvers;
using MinecraftWerewolf.Views.Dialogs;
using Ursa.Common;
using Ursa.Controls;
using Ursa.Controls.Options;

namespace MinecraftWerewolf.Views;

public partial class MainView : UserControl
{
    
    public MainView()
    {
        InitializeComponent();
    }

    public MainViewModel ViewModel => (DataContext as MainViewModel)!;
}