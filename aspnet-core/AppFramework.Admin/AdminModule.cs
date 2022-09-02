using Prism.Ioc;
using Prism.Modularity; 

namespace AppFramework.Admin
{
    public class AdminModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        { }

        public void RegisterTypes(IContainerRegistry services)
        {
            services.AddValidators();
            services.AddServices();
            services.AddViews();
        }
    }
}
