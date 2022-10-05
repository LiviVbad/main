using AppFramework.Shared;
using AppFramework.Vision.Services;

namespace AppFramework.Vision.ViewModels
{
    public class NccViewModel: NavigationViewModel
    {
        private readonly IVisionMatchSerivce service;

        public NccViewModel(NccService service)
        {
            this.service = service;
        }
    }
}
