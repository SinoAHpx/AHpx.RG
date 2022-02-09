using System;
using AHpx.RG.Views;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace AHpx.RG.Services;

public class ServiceProvider
{
    public static Window GetMainWindow()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            return desktop.MainWindow;

        throw new ApplicationException("Internal error");
    }
}