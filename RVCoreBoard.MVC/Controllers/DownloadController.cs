namespace RVCoreBoard.MVC.Controllers
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using RVCoreBoard.MVC.DataContext;
    using RVCoreBoard.MVC.Models;

    public class DownloadController : Controller
    {
        private readonly IHostingEnvironment _environment;
        private readonly RVCoreBoardDBContext _db;

        public DownloadController(IHostingEnvironment environment, RVCoreBoardDBContext db)
        {
            _environment = environment;
            _db = db;
        }

        // TODO : Download액션에서 하드코딩으로 작성된 파일 경로는 상수로 빼는게 좋습니다.  2020. 09. 02
        [HttpGet("api/Download/{id}")]
        public async Task<IActionResult> Download(int id)
        {
            Attach attach = _db.Attachs.FirstOrDefault(p => p.ANo == id);

            // 404 오류 리턴
            if (attach == null)
                return NotFound();

            // 업로드된 경로
            string uploadPath = Path.Combine(_environment.WebRootPath);

            if (System.IO.File.Exists($"{uploadPath}/files/upload/{attach.FileFullName}") == false)
            {
                // 실제 파일이 없는 경우, 404 리턴
                return NotFound();
            }

            MemoryStream memory = new MemoryStream();
            using (FileStream fs = new FileStream($"{uploadPath}/files/upload/{attach.FileFullName}", FileMode.Open))
            {
                await fs.CopyToAsync(memory);
            }
            memory.Position = 0L;

            if (memory == null)
                return NotFound();

            return File(memory, "application/octet-stream", attach.FileFullName);
        }
    }
}
