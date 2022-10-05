using Microsoft.Xaml.Behaviors.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.Vision.Models
{
    public class AnisoShapeSettings : Settings
    {
        public string angleStep { get; set; }

        public string scaleRMin { get; set; }

        public string scaleRMax { get; set; }

        public string scaleRStep { get; set; }

        public string scaleCMin { get; set; }

        public string scaleCMax { get; set; }

        public string scaleCStep { get; set; }

        public string optimizatin { get; set; }

        public string metric { get; set; }

        public string contrast { get; set; }

        public string minContrast { get; set; }
    }
}
