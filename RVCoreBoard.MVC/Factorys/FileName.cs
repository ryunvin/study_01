using System.Linq;
using static RVCoreBoard.MVC.Models.BoardListInfoModel;

namespace RVCoreBoard.MVC.Factorys
{
    class FileName : Search
    {
        public FileName() { }

        public FileName(string searchString)
        {
            predicate = s => s.CommentList.Any(c => c.Content.Contains(searchString));
        }
    }
}
