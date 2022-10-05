using Prism.Mvvm; 

namespace AppFramework.Vision.Models
{
    public class Settings : BindableBase
    {
        public float angleStart { get; set; }

        public float angleExtent { get; set; }

        public string angleStep { get; set; }

        public string metric { get; set; }
    }
}
