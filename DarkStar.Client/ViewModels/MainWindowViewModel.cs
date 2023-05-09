using System;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using DarkStar.Client.Controls;
using DarkStar.Client.Models.Events;
using DarkStar.Client.PageViewModels;
using DarkStar.Client.PageViews;
using DarkStar.Client.Services;
using DarkStar.Network.Client.Interfaces;
using ReactiveUI;

namespace DarkStar.Client.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly WindowManager _windowManager;

    public int MaxProgressValue { get; set; } = 100;
    public int ProgressValue { get; set; } = 0;

    public string ProgressText { get; set; } = "";
    public string ServerName { get; set; } = "N/A";
    public string ServerVersion { get; set; } = "N/A";
    public ReactiveCommand<Unit, Task> ShowRenderViewCommand { get; set; }

    public MainWindowViewModel(IDarkStarNetworkClient client, WindowManager windowManager, ServiceContext serviceContext, TileService tileService)
    {
        _windowManager = windowManager;
        MessageBus.Current.Listen<ServerVersionEvent>()
            .Subscribe(
                @event => { ServerVersion = @event.ServerVersion; }
            );

        MessageBus.Current.Listen<ServerNameEvent>().Subscribe(s => ServerName = s.Name);

        MessageBus.Current.Listen<ProgressUpdateEvent>()
            .Subscribe(
                @event =>
                {
                    MaxProgressValue = @event.MaxProgress;
                    ProgressValue = @event.Progress;
                    ProgressText = @event.Message;
                }
            );



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
