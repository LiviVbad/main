namespace AppFramework.Services.Dialog
{
    public interface IAppDialogService
    {
        void ShowLoading(string message);

        void HideLoading();
    }
}