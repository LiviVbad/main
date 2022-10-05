using AppFramework.Vision.Models;

namespace AppFramework.Vision.Services
{
    public interface IVisionMatchSerivce
    {
        Settings QueryDefaultSettings();

        void ApplySettings(Settings settings);

        void FindResult();
    }
}
