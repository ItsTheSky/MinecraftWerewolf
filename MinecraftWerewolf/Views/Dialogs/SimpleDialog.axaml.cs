using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MinecraftWerewolf.Views.Dialogs;

public partial class SimpleDialog : UserControl
{
    public SimpleDialog()
    {
        InitializeComponent();
    }
}

public partial class SimpleDialogViewModel : ObservableObject
{

    [ObservableProperty] private string _content;

}