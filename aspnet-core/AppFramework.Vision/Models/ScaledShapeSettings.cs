
namespace AppFramework.Vision.Models
{
    public class ScaledShapeSettings : Settings
    {  
        public float scaleMin { get; set; }

        public float scaleMax { get; set; }

        public string scaleStep { get; set; }

        public string optimization { get; set; }
          
        public string contrast { get; set; }

        public string mincontrast { get; set; }
    }
}
