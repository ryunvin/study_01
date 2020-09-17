using static RVCoreBoard.MVC.Models.BoardListInfoModel;

namespace RVCoreBoard.MVC.Factorys
{
    class Title : Search
    {
        public Title() { }

        public Title(string searchString)
        {
            predicate = s => s.Title.Contains(searchString);
        }
    }
}
