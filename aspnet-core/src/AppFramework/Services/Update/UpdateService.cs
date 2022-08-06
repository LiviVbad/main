using AppFramework.Common;
using AppFramework.Update;
using AppFramework.Update.Dtos;
using AutoUpdaterDotNET;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace AppFramework.Services.Update
{
    public class UpdateService : IUpdateService
    {
        private readonly IAppUpdateService appService;

        public UpdateService(IAppUpdateService appService)
        {
            this.appService = appService;
        }

        public async Task CheckVersion()
        {
#pragma warning disable CS8602 
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
#pragma warning restore CS8602

            await WebRequest.Execute(() => appService.CheckVersion(new AppFramework.Update.Dtos.CheckVersionInput()
            {
                Version = version,
                ApplicationName = AppSettings.AppName
            }), CheckVersionFinish);
        }

        private async Task CheckVersionFinish(UpdateFileOutput output)
        {
            AutoUpdater.ShowSkipButton = false;
            AutoUpdater.ShowRemindLaterButton = false;
            AutoUpdater.LetUserSelectRemindLater = false;
            AutoUpdater.Mandatory = true;
            AutoUpdater.UpdateMode = Mode.Forced;
            AutoUpdater.InstallationPath = Environment.CurrentDirectory;
            AutoUpdater.ReportErrors = true;
            AutoUpdater.ShowUpdateForm(new UpdateInfoEventArgs()
            {
                ChangelogURL = output.ChangelogURL,
                CurrentVersion = $"{AppSettings.AppName}{output.Version}",
                DownloadURL = output.DownloadURL,
                Mandatory = new Mandatory
                {
                    Value = false,
                    UpdateMode = Mode.Forced,
                },
            });

            await Task.CompletedTask;
        }
    }
}
