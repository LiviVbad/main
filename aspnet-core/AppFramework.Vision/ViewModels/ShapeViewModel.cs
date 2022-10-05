using AppFramework.Shared;
using AppFramework.Vision.Services;

namespace AppFramework.Vision.ViewModels
{
    public class ShapeViewModel: NavigationViewModel
    {
        private readonly IVisionMatchSerivce service;

        public ShapeViewModel(ShapeService service)
        {
            this.service = service;
        }
    }
}
