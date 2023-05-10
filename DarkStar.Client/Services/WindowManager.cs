using System;
using System.Reflection;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using DarkStar.Client.Attributes;
using DarkStar.Client.Controls;
using DarkStar.Client.Models.Events;
using DarkStar.Client.ViewModels;
using Microsoft.Extensions.Logging;
using ReactiveUI;

namespace DarkStar.Client.Services;

public class WindowManager
{
    private readonly ILogger _logger;

    private readonly IServiceProvider _serviceProvider;
    private PageViewControl _pageViewControl;

    public WindowManager(ILogger<WindowManager> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        MessageBus.Current.Listen<NavigateToViewEvent>()
            .Subscribe(
                @event => { _ = Task.Run(async () => await NavigateToPage(@event.ViewType)); }
            );
    }

    public void InitializePageView(PageViewControl pageViewControl) => _pageViewControl = pageViewControl;

    public async Task NavigateToPage(Type type)
    {
        await Dispatcher.UIThread.InvokeAsync(
            async () =>
            {
                try
                {
                    var pageViewAttribute = type.GetCustomAttribute<PageViewAttribute>();
                    if (pageViewAttribute == null)
                    {
                        _logger.LogError("PageViewAttribute not found for {Name}", type.Name);
                        throw new Exception($"PageViewAttribute not found for {type.Name}");
                    }

                    var pageView = _serviceProvider.GetService(pageViewAttribute.View) as UserControl;
                    var pageViewModel = _serviceProvider.GetService(type) as PageViewModelBase;

                    pageView.DataContext = pageViewModel;

                    if (_pageViewControl.ControlProperty.Content is UserControl
                        {
                            DataContext: PageViewModelBase pageViewModelBase
                        })
                    {
                        await pageViewModelBase.OnClose();
                    }


                    pageView.Focus();

                    _pageViewControl.ControlProperty.Content = pageView;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        );
    }

    public Task NavigateToPage<T>() where T : PageViewModelBase => NavigateToPage(typeof(T));
}
