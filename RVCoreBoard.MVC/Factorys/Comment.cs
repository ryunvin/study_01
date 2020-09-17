using System.Linq;
using static RVCoreBoard.MVC.Models.BoardListInfoModel;

namespace RVCoreBoard.MVC.Factorys
{
    class Comment : Search
    {
        public Comment() { }

        public Comment(string searchString)
        {
            Predicate = s => s.CommentList.Any(c => c.Content.Contains(searchString));
        }
    }
}
