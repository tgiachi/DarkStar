using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DarkStar.Client.Controls;

public partial class PageViewControl : UserControl
{
    public StyledProperty<Control> ControlProperty { get; set; } =   AvaloniaProperty.Register<PageViewControl, Control>(nameof(Control));
    
    public PageViewControl()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {

        AvaloniaXamlLoader.Load(this);
    }
}

