using System.Threading.Tasks;
using Avalonia.Controls;
using MinecraftWerewolf.Views.Dialogs;
using Ursa.Controls;

namespace MinecraftWerewolf.Utilities;

public static class DialogUtils
{

    public static async Task Show(string title, string description, DialogMode mode = DialogMode.Info)
    {
        await Dialog.ShowModal<SimpleDialog, SimpleDialogViewModel>(
            new SimpleDialogViewModel() { Content = description }, options: new DialogOptions()
            {
                Title = title,
                Mode = mode,
                StartupLocation = WindowStartupLocation.CenterOwner,
                IsCloseButtonVisible = false,
                CanResize = false,
                ShowInTaskBar = false,
                Button = DialogButton.OK
            });
    }
    
}