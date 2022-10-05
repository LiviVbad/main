using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.Vision.Models
{
    public class Settings : BindableBase
    {
        public float angleStart { get; set; }

        public float angleExtent { get; set; }

        public float scaleMin { get; set; }

        public float scaleMax { get; set; }

        public float minScore { get; set; }

        public int numMatches { get; set; }

        public float maxOverlap { get; set; }

        public string subPixel { get; set; }

        public int numLevels { get; set; }

        public float greediness { get; set; }
    }
}
