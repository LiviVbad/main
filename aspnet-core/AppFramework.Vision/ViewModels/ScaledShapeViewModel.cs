using AppFramework.Shared;
using AppFramework.Vision.Services;

namespace AppFramework.Vision.ViewModels
{
    public class ScaledShapeViewModel: NavigationViewModel
    {
        private readonly IVisionMatchSerivce service;

        public ScaledShapeViewModel(ScaledShapeService service)
        {
            this.service = service;
        }
    }
}
