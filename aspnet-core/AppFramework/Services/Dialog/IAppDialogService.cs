using System;
using System.Collections.Generic;
using System.Text;

namespace AppFramework.Services.Dialog
{
    public interface IAppDialogService
    {
        void ShowLoading(string message);

        void HideLoading();
    }
}
