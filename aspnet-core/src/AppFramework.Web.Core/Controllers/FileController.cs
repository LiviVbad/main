using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Extensions;
using Abp.MimeTypes;
using Microsoft.AspNetCore.Mvc;
using AppFramework.Dto;
using AppFramework.Storage;
using AppFramework.Update.Dtos;
using Abp.UI;
using Microsoft.AspNetCore.Http;
using PayPalHttp;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using AppFramework.Update;
using Twilio.TwiML.Messaging;

namespace AppFramework.Web.Controllers
{
    public class FileController : AppFrameworkControllerBase
    {
        private readonly IAbpVersionsAppService versionsAppService;
        private readonly IWebHostEnvironment environment;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IMimeTypeMap _mimeTypeMap;

        public FileController(
            IAbpVersionsAppService versionsAppService,
            IWebHostEnvironment environment,
            ITempFileCacheManager tempFileCacheManager,
            IBinaryObjectManager binaryObjectManager,
            IMimeTypeMap mimeTypeMap
        )
        {
            this.versionsAppService = versionsAppService;
            this.environment = environment;
            _tempFileCacheManager = tempFileCacheManager;
            _binaryObjectManager = binaryObjectManager;
            _mimeTypeMap = mimeTypeMap;
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

        [DisableAuditing]
        public ActionResult DownloadTempFile(FileDto file)
        {
            var fileBytes = _tempFileCacheManager.GetFile(file.FileToken);
            if (fileBytes == null)
            {
                return NotFound(L("RequestedFileDoesNotExists"));
            }

            return File(fileBytes, file.FileType, file.FileName);
        }

        [DisableAuditing]
        public async Task<ActionResult> DownloadBinaryFile(Guid id, string contentType, string fileName)
        {
            var fileObject = await _binaryObjectManager.GetOrNullAsync(id);
            if (fileObject == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }

            if (fileName.IsNullOrEmpty())
            {
                if (!fileObject.Description.IsNullOrEmpty() &&
                    !Path.GetExtension(fileObject.Description).IsNullOrEmpty())
                {
                    fileName = fileObject.Description;
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.BadRequest);
                }
            }

            if (contentType.IsNullOrEmpty())
            {
                if (!Path.GetExtension(fileName).IsNullOrEmpty())
                {
                    contentType = _mimeTypeMap.GetMimeType(fileName);
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.BadRequest);
                }
            }

            return File(fileObject.Bytes, contentType, fileName);
        }
    }
}
