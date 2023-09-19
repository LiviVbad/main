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
using AppFramework.Version.Dtos;
using Abp.Auditing;
using Microsoft.AspNetCore.Hosting;
using AppFramework.Version;
using System.Security.Cryptography;
using System.Text;

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

        [RequestFormLimits(MultipartBodyLengthLimit = 1024000000)]
        [RequestSizeLimit(1024000000)]
        public async Task<ActionResult> UploadVersionFile(CreateOrEditAbpVersionDto input)
        {
            var file = Request.Form.Files.FirstOrDefault();

            if (file == null && input.Id == null)
                throw new UserFriendlyException(L("RequestedFileDoesNotExists"));

            if (file != null)
            {
                var rootPath = environment.WebRootPath + @"\app\version";

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

                input.AlgorithmValue = GetMd5HashFromFile(filePath);
                input.HashingAlgorithm = "MD5";
                input.DownloadUrl = $"app/version/{fileName}";
            }

            await versionsAppService.CreateOrEdit(input);

            return Ok();

            static string GetMd5HashFromFile(string fileName)
            {
                FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                    sb.Append(retVal[i].ToString("x2"));

                return sb.ToString();
            }
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