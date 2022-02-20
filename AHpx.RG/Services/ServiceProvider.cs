using System;
using System.Threading.Tasks;
using AHpx.RG.Views;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using MessageBox.Avalonia.Models;

namespace AHpx.RG.Services;

public class ServiceProvider
{
    public static IMessageBus MessageBus = new MessageBus();

    public static Window GetMainWindow()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            return desktop.MainWindow;

        throw new ApplicationException("Internal error");
    }

    public static async Task<string> PromptExceptionDialog(Exception e)
    {
        return await MessageBoxManager.GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ContentHeader = "An exception occurred",
                ContentTitle = "Oops",
                ContentMessage = e.ToString(),
                MaxHeight = 300,
                MaxWidth = 600,
                SizeToContent = SizeToContent.Height,
                ButtonDefinitions = new[]
                {
                    new ButtonDefinition {Name = "Copy"},
                    new ButtonDefinition {Name = "OK"},
                }
            })
            .ShowDialog(GetMainWindow());
    }
}