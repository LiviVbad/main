using Acr.UserDialogs;
using AppFramework.Common.Services.Layer;

namespace AppFramework.Shared.Services.Layer
{
    public class ApplayerService : IApplayerService
    { 
        /// <summary>
        /// 显示等待窗口
        /// </summary>
        /// <param name="message"></param>
        public void Show(string message)
        {
            UserDialogs.Instance.ShowLoading(message, MaskType.None);
        }

        /// <summary>
        /// 隐藏等待窗口
        /// </summary>
        public void Hide()
        {
            UserDialogs.Instance.HideLoading();
        }
    }
}