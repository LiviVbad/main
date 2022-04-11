using Xamarin.Forms.Internals;

namespace AppFrameworkDemo.Shared.Behaviors
{
    [Preserve(AllMembers = true)]
    public interface IAction
    {
        bool Execute(object sender, object parameter);
    }
}