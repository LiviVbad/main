using System;
using System.Linq;
using System.Windows;

namespace AppFramework.Services
{
    public class ResourceService : IResourceService
    {
        public void AddCustomResources(ResourceDictionary Resource)
        {
            Resource.MergedDictionaries.Add(new ResourceDictionary()
            {
                Source = new Uri("pack://application:,,,/AppFramework;component/Themes/Generic.xaml", UriKind.RelativeOrAbsolute)
            });
        }

        public void UpdateCustomResources(ResourceDictionary Resource, string themeName)
        {
            /*
             * 《切换主题说明》
             * 清空系统载入的资源文件
             * 同时保留Syncfusion提供的默认资源文件
             */

            var lastResource = Resource.MergedDictionaries.LastOrDefault();
            Resource.MergedDictionaries.Clear();
            if (lastResource != null) Resource.MergedDictionaries.Add(lastResource);

            /*
             * 《系统资源说明》
             * 为了保留Syncfusion主题的兼容性, 所有修改不影响
             * Syncfusion主题切换导致的不一致性,采取每次切换主题
             * 动态刷新程序集资源的主题引用
             *
             * 刷新资源后将系统提供默认资源重新载入程序资源当中。
             */

            AddCustomResources(Resource);
            RefreshResources(Resource, "Button", themeName);
        }

        /// <summary>
        /// 刷新特定资源的样式
        /// </summary>
        /// <param name="collection">资源字典</param>
        /// <param name="resources">资源名称</param>
        /// <param name="baseThemeName">资源的基础继承类型</param>
        private void RefreshResources(ResourceDictionary collection,
           string resources,
           string baseThemeName)
        {
            string basePath = "pack://application:,,,/AppFramework;component/Themes";
            var resource = new ResourceDictionary()
            {
                Source = new Uri($"{basePath}/{resources}.xaml", UriKind.RelativeOrAbsolute)
            };
            resource.MergedDictionaries.Add(new ResourceDictionary()
            {
                Source = new Uri($"/Syncfusion.Themes.{baseThemeName}.WPF;component/MSControl/{resources}.xaml", UriKind.RelativeOrAbsolute)
            });
            collection.MergedDictionaries.Add(resource);
        }
    }
}