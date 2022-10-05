using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.Vision.Models
{
    public class ShapeSettings : Settings
    {
        public string optimization { get; set; }

        public string metric { get; set; }

        public string contrast { get; set; }

        public string minContrast { get; set; }

        public string angleStep { get; set; }
    }
}
