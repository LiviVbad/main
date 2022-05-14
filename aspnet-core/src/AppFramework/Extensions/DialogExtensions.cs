﻿using AppFramework.Common;
using AppFramework.Services; 
using Prism.Services.Dialogs;
using System.Threading.Tasks;

namespace AppFramework
{
    public static class DialogExtensions
    {
        public static void Show(this IDialogService dialogService, string title, string message)
        {
            DialogParameters parameters = new DialogParameters();
            parameters.Add("Title", title);
            parameters.Add("Message", message);

            dialogService.ShowDialog(AppViewManager.MessageBox, parameters);
        }

        /// <summary>
        /// 扩展-打开指定对话框(模式)
        /// </summary>
        /// <param name="dialogService"></param>
        /// <param name="name">对话框名称</param>
        /// <param name="parameters">传递参数</param>
        /// <returns>返回对话框操作结果</returns>
        public static IDialogResult ShowDialog(this IDialogService dialogService, string name, IDialogParameters parameters = null)
        {
            IDialogResult result = null;
            dialogService.ShowDialog(name, parameters, callback =>
            {
                result = callback;
            });
            return result;
        }

        public static async Task<bool> Question(this IHostDialogService hostDialogService,
            string message,
            string IdentifierName = AppCommonConsts.RootIdentifier)
        {
            return await Question(hostDialogService, Local.Localize("AreYouSure"), message, IdentifierName);
        }

        public static async Task<bool> Question(this IHostDialogService hostDialogService,
            string title,
            string message,
            string IdentifierName = AppCommonConsts.RootIdentifier)
        {
            DialogParameters param = new DialogParameters();
            param.Add("Title", title);
            param.Add("Message", message);

            var dialogResult = await hostDialogService.ShowDialogAsync(AppViewManager.HostMessageBox, param, IdentifierName);

            return dialogResult.Result == ButtonResult.OK;
        }

        public static bool Question(this IDialogService dialogService, string title, string message)
        {
            if (string.IsNullOrWhiteSpace(title))
                title = Local.Localize("AreYouSure");

            DialogParameters parameters = new DialogParameters();
            parameters.Add("Title", title);
            parameters.Add("Message", message);

            bool dialogResult = false;
            dialogService.ShowDialog(AppViewManager.MessageBox, parameters, callback =>
            {
                dialogResult = callback.Result == ButtonResult.OK;
            });
            return dialogResult;
        }
    }
}