using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using DarkStar.Client.Controls;
using DarkStar.Client.PageViewModels;
using DarkStar.Client.Services;
using DarkStar.Network.Client.Interfaces;

namespace DarkStar.Client.ViewModels;

public class MainWindowViewModel : ViewModelBase
{

    private readonly WindowManager _windowManager;

    public MainWindowViewModel(IDarkStarNetworkClient client, WindowManager windowManager) => _windowManager = windowManager;

    public void SetPageViewControl(PageViewControl pageViewControl)
    {
        _windowManager.InitializePageView(pageViewControl);
        _ = Task.Run(
            async () =>
            {
                await _windowManager.NavigateToPage<LoginPageViewModel>();
            }
        );

    }

}
