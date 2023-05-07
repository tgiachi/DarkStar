using Avalonia;
using Avalonia.Controls;
using DarkStar.Client.Controls;
using DarkStar.Network.Client.Interfaces;

namespace DarkStar.Client.ViewModels;

public class MainWindowViewModel : ViewModelBase
{

    public PageViewControl PageViewControl { get; set; }

    public MainWindowViewModel(IDarkStarNetworkClient client)
    {

    }

    public void SetPageViewControl(PageViewControl pageViewControl)
    {
        PageViewControl = pageViewControl;
        pageViewControl.ControlProperty.Content = new Button()
        {
            Content = "press me"
        };

    }

}
