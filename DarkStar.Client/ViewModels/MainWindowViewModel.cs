using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using DarkStar.Client.Controls;
using DarkStar.Client.PageViewModels;
using DarkStar.Client.PageViews;
using DarkStar.Client.Services;
using DarkStar.Network.Client.Interfaces;
using ReactiveUI;

namespace DarkStar.Client.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly WindowManager _windowManager;

    public ReactiveCommand<Unit, Task> ShowRenderViewCommand { get; set; }

    public MainWindowViewModel(IDarkStarNetworkClient client, WindowManager windowManager, ServiceContext serviceContext)
    {
        _windowManager = windowManager;
        ShowRenderViewCommand = ReactiveCommand.Create(
            async () => { await _windowManager.NavigateToPage<RenderPageViewModel>(); }
        );
    }

    public void SetPageViewControl(PageViewControl pageViewControl)
    {
        _windowManager.InitializePageView(pageViewControl);
        _ = Task.Run(
            async () => { await _windowManager.NavigateToPage<LoginPageViewModel>(); }
        );
    }
}
