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
        public static async Task<List<Board>> GetSearchBoardList(SearchType searchType, string searchString, IBoardService boardService)
        {
            List<Board> Data = new List<Board>();

            switch (searchType)
            {
                case SearchType.All:
                    All all = new All(searchString);
                    Data = await boardService.GetBoardList(all.predicate);
                    break;
                case SearchType.Title:
                    Title title = new Title(searchString);
                    Data = await boardService.GetBoardList(title.predicate);
                    break;
                case SearchType.Writer:
                    Writer writer = new Writer(searchString);
                    Data = await boardService.GetBoardList(writer.predicate);
                    break;
                case SearchType.Content:
                    Content content = new Content(searchString);
                    Data = await boardService.GetBoardList(content.predicate);
                    break;
                case SearchType.Comment:
                    Comment comment = new Comment(searchString);
                    Data = await boardService.GetBoardList(comment.predicate);
                    break;
                case SearchType.FileName:
                    FileName fileName = new FileName(searchString);
                    Data = await boardService.GetBoardList(fileName.predicate);
                    break;
            }
            return Data;
        }
    }
}
