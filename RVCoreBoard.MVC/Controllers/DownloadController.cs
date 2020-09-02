namespace RVCoreBoard.MVC.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using RVCoreBoard.MVC.DataContext;
    using RVCoreBoard.MVC.Models;

    public class DownloadController : Controller
    {
        private readonly IHostingEnvironment _environment;
        private readonly RVCoreBoardDBContext _db;
        private string uploadPath;

        public DownloadController(IHostingEnvironment environment, RVCoreBoardDBContext db)
        {
            _environment = environment;
            _db = db;

            uploadPath = _environment.WebRootPath + @"\upload\files";
        }

        // TODO : Download액션에서 하드코딩으로 작성된 파일 경로는 상수로 빼는게 좋습니다.  2020. 09. 02
        [HttpGet("api/Download/{id}")]
        public async Task<IActionResult> Download(int id)
        {
            Attach attach = _db.Attachs.FirstOrDefault(p => p.ANo == id);

            // 404 오류 리턴
            if (attach == null)
                return NotFound();


            if (System.IO.File.Exists($"{uploadPath}/{attach.FileFullName}") == false)
            {
                // 실제 파일이 없는 경우, 404 리턴
                return NotFound();
            }

            MemoryStream memory = new MemoryStream();
            using (FileStream fs = new FileStream($"{uploadPath}/{attach.FileFullName}", FileMode.Open))
            {
                await fs.CopyToAsync(memory);
            }
            memory.Position = 0L;

            if (memory == null)
                return NotFound();

            return File(memory, "application/octet-stream", attach.FileFullName.Substring(attach.FileFullName.IndexOf(".")+1));
        }

        // 압축파일로 한꺼번에 다운받기
        [HttpGet("api/AllDownload/{id}")]
        public async Task<IActionResult> AllDownload(int id)
        {
            var attachs = await _db.Attachs
                                .OrderByDescending(p => p.BNo)
                                .ToListAsync();

            // 404 오류 리턴
            if (attachs == null)
                return NotFound();

            //압축파일 경로
            string zipPath = $@"\{id}.zip";

            foreach (var attach in attachs)
            {
                if (System.IO.File.Exists($@"{uploadPath}\{attach.FileFullName}") == false)
                {
                    // 실제 파일이 없는 경우, 404 리턴
                    return NotFound();
                }
            }

            if (CreateZipFile(zipPath, attachs.Select(s => s.FileFullName)) == false)
            {
                return NotFound();
            }

            MemoryStream memory = new MemoryStream();
            using (FileStream fs = new FileStream(uploadPath + zipPath, FileMode.Open))
            {
                await fs.CopyToAsync(memory);
            }
            memory.Position = 0L;

            System.IO.File.Delete(uploadPath + zipPath);

            if (memory == null)
                return NotFound();

            return File(memory, "application/octet-stream", zipPath);
        }

        /// <summary>
        /// 압축 파일 만들기
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="files"></param>
        public bool CreateZipFile(string fileName, IEnumerable<string> files)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var file in files)
                        {
                            var demoFile = archive.CreateEntry(file.Substring(file.IndexOf(".") + 1));

                            using (var entryStream = demoFile.Open())
                            using (var streamWriter = new StreamWriter(file))
                            {
                                streamWriter.Write(entryStream);
                            }
                        }
                    }

                    using (var fileStream = new FileStream(uploadPath + fileName, FileMode.Create))
                    {
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        memoryStream.CopyTo(fileStream);
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
