using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.Vision.Services
{
    public static class ServicesExtensions
    {
        public static void AddServices(this IContainerRegistry services)
        {
            services.Register<NccService>();
            services.Register<ShapeService>();
            services.Register<AnisoShapeService>();
            services.Register<ScaledShapeService>();
        } 
    }
}
