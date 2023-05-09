using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PropertyChanged;

namespace DarkStar.Client.PageViews;

[DoNotNotify]
public partial class PlayerSelectPageView : UserControl
{
    public PlayerSelectPageView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}

