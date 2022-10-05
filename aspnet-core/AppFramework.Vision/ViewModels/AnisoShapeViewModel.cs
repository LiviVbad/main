using AppFramework.Shared;
using AppFramework.Vision.Services;
using System.Runtime.CompilerServices;

namespace AppFramework.Vision.ViewModels
{
    public class AnisoShapeViewModel : NavigationViewModel
    {
        private readonly IVisionMatchSerivce service;

        public AnisoShapeViewModel(AnisoShapeService service)
        {
            this.service = service;
        }
    }
}
