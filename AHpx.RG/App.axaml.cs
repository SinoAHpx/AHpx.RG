using System;
using AHpx.RG.ViewModels;
using AHpx.RG.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Material.Styles.Themes;
using Material.Styles.Themes.Base;

namespace AHpx.RG
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);

            //auto changing theme here

            var now = DateTime.Now.Hour;
            if (now is >= 19 or <= 7)
            {
                var paletteHelper = new PaletteHelper();
                var theme = paletteHelper.GetTheme();

                theme.SetBaseTheme(BaseThemeMode.Dark.GetBaseTheme());

                paletteHelper.SetTheme(theme);
            }
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}