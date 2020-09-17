using static RVCoreBoard.MVC.Models.BoardListInfoModel;

namespace RVCoreBoard.MVC.Factorys
{
    class Content : Search
    {
        public Content() { }

        public Content(string searchString)
        {
            predicate = s => s.Content.Contains(searchString);
        }
    }
}
