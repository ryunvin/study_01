using static RVCoreBoard.MVC.Models.BoardListInfoModel;

namespace RVCoreBoard.MVC.Factorys
{
    class Writer : Search
    {
        public Writer() { }

        public Writer(string searchString)
        {
            Predicate = s => s.user.Name.Contains(searchString);
        }
    }
}
