using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> ImageUpload(IFormFile file)
        {
            var path = Path.Combine(_environment.WebRootPath, @"images\upload");

            var fileFullName = Path.GetTempFileName().Split('.');
            var fileName = $"{Guid.NewGuid()}.{fileFullName[1]}";

            using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            //return Ok(new { file = "/images/upload/" + fileName, success = true });
            return Ok("/images/upload/" + fileName);
        }
    }
}
