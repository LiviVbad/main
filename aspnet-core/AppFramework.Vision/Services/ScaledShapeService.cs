using AppFramework.Vision.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.Vision.Services
{
    public class ScaledShapeService : IVisionMatchSerivce
    {
        public void ApplySettings(Settings settings)
        {

        }

        public void FindResult()
        {

        }

        public Settings QueryDefaultSettings()
        {
            return new ScaledShapeSettings()
            {
                angleStart = -0.39f,
                angleExtent = 0.79f,
                angleStep = "auto",
                scaleMin = 0.9f,
                scaleMax = 1.1f,
                scaleStep = "auto",
                optimization = "auto",
                metric = "use_polarity",
                contrast = "auto",
                mincontrast = "auto",
            };
        }
    }
}
