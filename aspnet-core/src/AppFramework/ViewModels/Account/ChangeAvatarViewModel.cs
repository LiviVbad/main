using AppFramework.Authorization.Users.Profile;
using AppFramework.Authorization.Users.Profile.Dto;
using AppFramework.Common;
using AppFramework.Dto;
using AppFramework.ViewModels.Shared;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class ChangeAvatarViewModel : HostDialogViewModel
    {
        public ChangeAvatarViewModel(IProfileAppService profileAppService,
            ProxyProfileControllerService profileControllerService)
        {
            SelectedFileCommand = new DelegateCommand(SelectedFile);
            this.profileAppService = profileAppService;
            this.profileControllerService = profileControllerService;
        }

        private string imageFilePath;
        private readonly IProfileAppService profileAppService;
        private readonly ProxyProfileControllerService profileControllerService;

        public string IamgeFilePath
        {
            get { return imageFilePath; }
            set { imageFilePath = value; RaisePropertyChanged(); }
        }


        private void SelectedFile()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "图片文件(*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
            var dialogResult = (bool)fileDialog.ShowDialog();
            if (dialogResult)
            {
                IamgeFilePath = fileDialog.FileName;
            }
        }

        protected override async void Save()
        {
            if (!string.IsNullOrEmpty(IamgeFilePath))
            {
                string fileName = "ProfilePicture";
                var photoAsBytes = File.ReadAllBytes(IamgeFilePath);

                await SetBusyAsync(async () =>
                {
                    await WebRequest.Execute(async () => await UpdateProfilePhoto(photoAsBytes, fileName),
                        () =>
                        {
                            base.Save(photoAsBytes);
                            return Task.CompletedTask;
                        });
                });
            }
        }

        private async Task UpdateProfilePhoto(byte[] photoAsBytes, string fileName)
        {
            var fileToken = Guid.NewGuid().ToString();

            using (Stream photoStream = new MemoryStream(photoAsBytes))
            {
                await profileControllerService.UploadProfilePicture(content =>
                {
                    content.AddFile("file", photoStream, fileName);
                    content.AddString(nameof(FileDto.FileToken), fileToken);
                    content.AddString(nameof(FileDto.FileName), fileName);
                }).ContinueWith(uploadResult =>
                {
                    if (uploadResult == null)
                        return;

                    profileAppService.UpdateProfilePicture(new UpdateProfilePictureInput
                    {
                        FileToken = fileToken
                    });
                });
            }
        }

        public DelegateCommand SelectedFileCommand { get; private set; }

        public override void OnDialogOpened(IDialogParameters parameters)
        { }
    }
}
