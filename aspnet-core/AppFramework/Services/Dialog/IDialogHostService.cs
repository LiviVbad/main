using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.Services
{
    /// <summary>
    /// 对话主机服务接口
    /// </summary>
    public interface IDialogHostService : IDialogService
    {
        void ShowDialog(string name, Action<IDialogResult> callback);

        Task<IDialogResult> ShowDialog(string name, IDialogParameters parameters = null, string IdentifierName = "Root");
    }
}
