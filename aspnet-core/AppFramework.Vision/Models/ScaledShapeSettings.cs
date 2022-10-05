using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.Vision.Models
{
    public class ScaledShapeSettings : Settings
    {
        public string angleStep { get; set; }
        
        public string scaleStep { get; set; }

        public string optimization { get; set; }

        public string metric { get; set; }

        public string contrast { get; set; }

        public string mincontrast { get; set; }
    }
}
