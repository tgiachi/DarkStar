using System.Threading.Tasks;

namespace DarkStar.Client.ViewModels;

public class PageViewModelBase : ViewModelBase
{
    public virtual Task OnClose() => Task.CompletedTask;

}
