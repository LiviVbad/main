using AppFramework.Authorization.Users.Profile;
using AppFramework.Authorization.Users.Profile.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Dto;
using AppFramework.MultiTenancy.Dto;
using AppFramework.Version;
using AppFramework.Version.Dtos;
using AppFramework.ViewModels.Shared;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace AppFramework.ViewModels.Version
{
    public class VersionManagerDetailsViewModel : HostDialogViewModel
    {
        public VersionManagerDetailsViewModel(IAbpVersionsAppService appService,
            ProxyProfileControllerService profileControllerService)
        {
            this.appService = appService;
            this.profileControllerService = profileControllerService;

            SelectedFileCommand = new DelegateCommand(SelectedFile);
        }
         
        private VersionListModel model;

        public VersionListModel Model
        {
            get { return model; }
            set { model = value; RaisePropertyChanged(); }
        }

        private string filePath;
        private readonly IAbpVersionsAppService appService;
        private readonly ProxyProfileControllerService profileControllerService;

        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; RaisePropertyChanged(); }
        }

        public DelegateCommand SelectedFileCommand { get; private set; }

        private void SelectedFile()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "压缩文件(*.rar;*.zip)|*.rar;*.zip";
            var dialogResult = (bool)fileDialog.ShowDialog();
            if (dialogResult)
            {
                FilePath = fileDialog.FileName;
            }
        }

        protected override async void Save()
        {
            if (string.IsNullOrWhiteSpace(FilePath)) return;

            if (!Verify(Model).IsValid) return;

            await SetBusyAsync(async () =>
            {
                var fileBytes = File.ReadAllBytes(FilePath);

                using (Stream photoStream = new MemoryStream(fileBytes))
                {
                    await WebRequest.Execute(() => profileControllerService.UploadVersionFile(content =>
                    {
                        content.AddFile("file", photoStream, "versionfile");
                        content.AddString(nameof(CreateOrEditAbpVersionDto.Name), Model.Name);
                        content.AddString(nameof(CreateOrEditAbpVersionDto.Version), Model.Version);
                        content.AddString(nameof(CreateOrEditAbpVersionDto.IsForced), Model.IsForced.ToString());
                        content.AddString(nameof(CreateOrEditAbpVersionDto.IsEnable), Model.IsEnable.ToString());
                    }), async () =>
                    {
                        base.Save();
                        await Task.CompletedTask;
                    });
                }
            });
        }

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            await SetBusyAsync(async () =>
            {
                if (parameters.ContainsKey("Value"))
                {
                    var tenant = parameters.GetValue<AbpVersionDto>("Value");
                    Model = Map<VersionListModel>(tenant);
                }
                else
                    Model = new VersionListModel();

                await Task.CompletedTask;
            });
        }
    }
}
