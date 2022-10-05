using AppFramework.Vision.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.Vision.Services
{
    public class NccService : IVisionMatchSerivce
    {
        public void ApplySettings(Settings settings)
        {

        }

        public void FindResult()
        {

        }

        public Settings QueryDefaultSettings()
        {
            return new NccSettings()
            {
                angleStart = 0.39f,
                angleExtent = 0.79f,
                angleStep = "auto",
                metric = "use_polarity"
            };
        }
    }
}
