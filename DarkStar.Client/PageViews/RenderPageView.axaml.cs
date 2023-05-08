using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DarkStar.Client.Controls;

namespace DarkStar.Client.PageViews;

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

    }
}

