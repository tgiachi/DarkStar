using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DarkStar.Client.ViewModels;
using DarkStar.Client.Views;
using PropertyChanged;
using Splat;

namespace DarkStar.Client;

[DoNotNotify]
public partial class App : Application
{
    private SplashScreenWindow _splashScreenWindow;
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        _splashScreenWindow = new SplashScreenWindow();
        _splashScreenWindow.Show();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {

            desktop.MainWindow = new MainWindow
            {
                DataContext = Locator.Current.GetService<MainWindowViewModel>()
            };

            _splashScreenWindow.Close();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
