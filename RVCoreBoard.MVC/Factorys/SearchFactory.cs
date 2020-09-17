using RVCoreBoard.MVC.Models;
using RVCoreBoard.MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static RVCoreBoard.MVC.Models.BoardListInfoModel;

namespace RVCoreBoard.MVC.Factorys
{
    public class SearchFactory
    {
        public static async Task<List<Board>> GetSearchBoardList(int id, SearchType searchType, string searchString, IBoardService boardService)
        {
            List<Board> Data = new List<Board>();

            switch (searchType)
            {
                case SearchType.All:
                    All all = new All(searchString);
                    Data = await boardService.GetBoardList(all.Predicate);
                    break;
                case SearchType.Title:
                    Title title = new Title(searchString);
                    Data = await boardService.GetBoardList(title.Predicate);
                    break;
                case SearchType.Writer:
                    Writer writer = new Writer(searchString);
                    Data = await boardService.GetBoardList(writer.Predicate);
                    break;
                case SearchType.Content:
                    Content content = new Content(searchString);
                    Data = await boardService.GetBoardList(content.Predicate);
                    break;
                case SearchType.Comment:
                    Comment comment = new Comment(searchString);
                    Data = await boardService.GetBoardList(comment.Predicate);
                    break;
                case SearchType.FileName:
                    FileName fileName = new FileName(searchString);
                    Data = await boardService.GetBoardList(fileName.Predicate);
                    break;
            }
            Data = Data.Where(d => d.category.Id.Equals(id)).ToList();
            return Data;
        }
    }
}
