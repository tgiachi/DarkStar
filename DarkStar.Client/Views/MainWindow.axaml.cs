using Avalonia.Controls;
using DarkStar.Client.Controls;
using DarkStar.Client.ViewModels;
using PropertyChanged;

namespace DarkStar.Client.Views;

[DoNotNotify]
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContextChanged += (_, _) =>
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                viewModel.SetPageViewControl( this.GetControl<PageViewControl>("ViewControl"));
            }
        };
    }


}
