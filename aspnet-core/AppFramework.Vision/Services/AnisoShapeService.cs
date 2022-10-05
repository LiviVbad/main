using AppFramework.Vision.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.Vision.Services
{
    public class AnisoShapeService : IVisionMatchSerivce
    {
        public void ApplySettings(Settings settings)
        { }

        public void FindResult()
        { }

        public Settings QueryDefaultSettings()
        {
            return new AnisoShapeSettings()
            {
                angleStart = -0.39f,
                angleExtent = 0.79f,
                angleStep = "auto",
                scaleRMin = "0.9",
                scaleRMax = "1.1",
                scaleRStep = "auto",
                scaleCMin = "0.9",
                scaleCMax = "1.1",
                scaleCStep = "auto",
                optimizatin = "auto",
                metric = "use_polarity",
                contrast = "auto",
                minContrast = "auto"
            };
        }
    }
}
