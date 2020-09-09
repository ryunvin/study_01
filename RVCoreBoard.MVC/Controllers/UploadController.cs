using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RVCoreBoard.MVC.Attributes;
using static RVCoreBoard.MVC.Models.User;

namespace RVCoreBoard.MVC.Controllers
{
    public class UploadController : Controller
    {
        private readonly IHostingEnvironment _environment;

        public UploadController(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        /// <summary>
        /// 이미지 업로드
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        // TODO : 이 API도 인증 체크 해야지 아무나 파일 업로드 못함
        [HttpPost, Route("api/imageUpload")]
        [CustomAuthorize(RoleEnum = UserLevel.Senior | UserLevel.Senior | UserLevel.Admin)]
        public async Task<IActionResult> ImageUpload(IFormFile file)
        {
            var path = Path.Combine(_environment.WebRootPath, @"upload\images");

            var fileFullName = file.FileName.Split('.');
            // TODO : 이미지 확장자를 tmp로 하면 웹서버에서 접근할 수 없음.
            var fileName = $"{Guid.NewGuid()}.{fileFullName[1]}";
            //var fileName = $"{Guid.NewGuid()}.png";

            using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            //return Ok(new { file = "/images/upload/" + fileName, success = true });
            return Ok("/upload/images/" + fileName);
        }
    }
}
