using Prism.Services.Dialogs;

namespace AppFramework
{
    public static class DialogExtensions
    {
        /// <summary>
        /// 扩展-打开指定对话框(模式)
        /// </summary>
        /// <param name="dialogService"></param>
        /// <param name="name">对话框名称</param>
        /// <param name="parameters">传递参数</param>
        /// <returns>返回对话框操作结果</returns>
        public static IDialogResult ShowViewDialog(this IDialogService dialogService,
            string name,
            IDialogParameters parameters = null)
        {
            IDialogResult result = null;

            dialogService.ShowDialog(name, parameters, callback =>
              {
                  result = callback;
              });

            return result;
        }

        /// <summary>
        /// 扩展-打开指定对话框(非模式)
        /// </summary>
        /// <param name="dialogService"></param>
        /// <param name="name">对话框名称</param>
        /// <param name="parameters">传递参数</param>
        /// <returns>返回对话框操作结果</returns>
        public static IDialogResult ShowView(this IDialogService dialogService,
            string name,
            IDialogParameters parameters = null)
        {
            IDialogResult result = null;

            dialogService.Show(name, parameters, callback =>
            {
                result = callback;
            });

            return result;
        }
    }
}