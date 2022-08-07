using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetZeroCore.Net;
using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AppFramework.Authorization.Users.Profile;
using AppFramework.Authorization.Users.Profile.Dto;
using AppFramework.Dto;
using AppFramework.Storage;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using AppFramework.Update.Dtos;
using Abp.Auditing;
using PayPalHttp;
using AppFramework.Update;
using Microsoft.AspNetCore.Hosting;

namespace AppFramework.Web.Controllers
{
    public abstract class ProfileControllerBase : AppFrameworkControllerBase
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IProfileAppService _profileAppService;
        private readonly IAbpVersionsAppService versionsAppService;
        private readonly IWebHostEnvironment environment;

        private const int MaxProfilePictureSize = 5242880; //5MB

        protected ProfileControllerBase(
            IAbpVersionsAppService versionsAppService,
            IWebHostEnvironment environment,
            ITempFileCacheManager tempFileCacheManager,
            IProfileAppService profileAppService)
        { 
            this.versionsAppService = versionsAppService;
            this.environment = environment;
            _tempFileCacheManager = tempFileCacheManager;
            _profileAppService = profileAppService;
        }

        [DisableAuditing]
        public async Task<ActionResult> UploadVersionFile(CreateOrEditAbpVersionDto input)
        {
            var file = Request.Form.Files.First();

            if (file == null)
                throw new UserFriendlyException(L("RequestedFileDoesNotExists"));

            var rootPath = environment.WebRootPath + "\\app\\version";

            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            //生成随机文件名
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName).ToLowerInvariant();
            string filePath = Path.Combine(rootPath, fileName);
            using (FileStream fs = System.IO.File.Create(filePath))
            {
                file.CopyTo(fs);
                fs.Flush();
            }

            input.DownloadUrl = filePath;

            await versionsAppService.CreateOrEdit(input);

            return Ok();
        }

        public UploadProfilePictureOutput UploadProfilePicture(FileDto input)
        {
            try
            {
                var profilePictureFile = Request.Form.Files.First();

                //Check input
                if (profilePictureFile == null)
                {
                    throw new UserFriendlyException(L("ProfilePicture_Change_Error"));
                }

                if (profilePictureFile.Length > MaxProfilePictureSize)
                {
                    throw new UserFriendlyException(L("ProfilePicture_Warn_SizeLimit",
                        AppConsts.MaxProfilePictureBytesUserFriendlyValue));
                }

                byte[] fileBytes;
                using (var stream = profilePictureFile.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                using (var image = Image.Load(fileBytes, out IImageFormat format))
                {
                    if (!format.IsIn(JpegFormat.Instance, PngFormat.Instance, GifFormat.Instance))
                    {
                        throw new UserFriendlyException(L("IncorrectImageFormat"));
                    }

                    _tempFileCacheManager.SetFile(input.FileToken, fileBytes);

                    return new UploadProfilePictureOutput
                    {
                        FileToken = input.FileToken,
                        FileName = input.FileName,
                        FileType = input.FileType,
                        Width = image.Width,
                        Height = image.Height
                    };
                }
            }
            catch (UserFriendlyException ex)
            {
                return new UploadProfilePictureOutput(new ErrorInfo(ex.Message));
            }
        }

        [AllowAnonymous]
        public FileResult GetDefaultProfilePicture()
        {
            return GetDefaultProfilePictureInternal();
        }

        public async Task<FileResult> GetProfilePictureByUser(long userId)
        {
            var output = await _profileAppService.GetProfilePictureByUser(userId);
            if (output.ProfilePicture.IsNullOrEmpty())
            {
                return GetDefaultProfilePictureInternal();
            }

            return File(Convert.FromBase64String(output.ProfilePicture), MimeTypeNames.ImageJpeg);
        }

        protected FileResult GetDefaultProfilePictureInternal()
        {
            return File(Path.Combine("Common", "Images", "default-profile-picture.png"), MimeTypeNames.ImagePng);
        }
    }
}