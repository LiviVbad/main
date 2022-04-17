using System.Windows;

namespace AppFramework.Services
{
    public interface IResourceService
    {
        void AddCustomResources(ResourceDictionary Resource);

        void UpdateCustomResources(ResourceDictionary Resource, string themeName);
    }
}