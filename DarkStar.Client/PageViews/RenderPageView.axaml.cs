using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DarkStar.Client.Controls;
using DarkStar.Client.PageViewModels;
using PropertyChanged;

namespace DarkStar.Client.PageViews;

[DoNotNotify]
public partial class RenderPageView : UserControl
{
    private RenderControl _canvas;


    public RenderPageView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        _canvas = this.GetControl<RenderControl>("RenderControl");
        DataContextChanged += (sender, args) =>
        {
            if (DataContext is RenderPageViewModel renderPageViewModel)
            {
                renderPageViewModel.SetupRenderControl(_canvas);
            }
        };

        KeyDown += (sender, args) =>
        {
            if (DataContext is RenderPageViewModel renderPageViewModel)
            {
                renderPageViewModel.DispatchKey(args.Key, args.KeyModifiers);
            }
        };

        PointerWheelChanged += (sender, args) =>
        {
            if (DataContext is RenderPageViewModel renderPageViewModel)
            {
                renderPageViewModel.DispatchMouseWheel(args.Delta);
            }
        };

        PointerMoved += (sender, args) =>
        {
            if (DataContext is RenderPageViewModel renderPageViewModel)
            {
                renderPageViewModel.DispatchMouseMove(args.GetPosition(this));
            }
        };

        PointerPressed += (sender, args) =>
        {
            if (DataContext is RenderPageViewModel renderPageViewModel)
            {
            }
        };
    }
}
