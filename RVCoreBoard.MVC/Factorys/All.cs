using RVCoreBoard.MVC.Services;
using System.Linq;
using System.Threading.Tasks;
using static RVCoreBoard.MVC.Models.BoardListInfoModel;

namespace RVCoreBoard.MVC.Factorys
{
    class All : Search
    {
        public All() { }

        public All(string searchString)
        {
            predicate = s => s.user.Name.Contains(searchString) || s.Content.Contains(searchString)
                                        || s.CommentList.Any(c => c.Content.Contains(searchString)) || s.Title.Contains(searchString);
        }
    }
}
