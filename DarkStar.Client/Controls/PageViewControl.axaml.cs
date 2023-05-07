using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Markup.Xaml;

namespace DarkStar.Client.Controls;

public partial class PageViewControl : UserControl
{
    public ContentPresenter ControlProperty { get; set; } = null!;

    public PageViewControl()
    {

        InitializeComponent();

    }

    private void InitializeComponent()
    {

        AvaloniaXamlLoader.Load(this);
        ControlProperty = this.GetControl<ContentPresenter>("Presenter");
    }
}

